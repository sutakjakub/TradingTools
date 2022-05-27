using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingTools.Db.Entities;
using TradingTools.Taxes.Models;
using TradingTools.Taxes.Strategies;

namespace TradingTools.Taxes
{
    public class TaxCalculator
    {
        public IEnumerable<TaxReportItem> TaxReportItems { get; private set; }
        public IEnumerable<T2TradeEntity> Source { get; private set; }
        public TaxStrategy Strategy { get; }

        public TaxCalculator(TaxStrategy strategy)
        {
            Strategy = strategy ?? throw new ArgumentNullException(nameof(strategy));
        }

        public decimal TotalGain()
        {
            if (TaxReportItems == null)
                return 0;

            return TaxReportItems.Sum(s => s.GainUsdValue);
        }

        public void Calculate(IEnumerable<T2TradeEntity> entities)
        {
            Source = entities.ToList();
            var roots = CreateRootElements(entities);
            Strategy.Load(roots);
            Strategy.Process();

            TaxReportItems = ConvertToItems(Strategy.TaxData).OrderBy(o => o.DateSold).ToList();
        }

        public static IEnumerable<TaxReportItem> ConvertToItems(IList<TaxDataRoot> source)
        {
            var numberPair = source
                .SelectMany(s => s.BuyItems)
                .Select(s => s.Id)
                .GroupBy(x => x)
                .ToDictionary(x => x.Key, y => y.Count());

            foreach (var root in source)
            {
                var sellCommisionInUsd = root.SellItem.Commision.Amount * root.SellItem.Commision.AssetUsdValue;
                sellCommisionInUsd /= root.BuyItems.Count;

                foreach (var buyItem in root.BuyItems)
                {
                    var buyCommision = buyItem.Commision.Amount * buyItem.Commision.AssetUsdValue;
                    buyCommision /= numberPair[buyItem.Id];
                    yield return TaxReportItem.CreateBuyItem(
                       buyItem.Amount,
                       buyItem.AssetName,
                       buyItem.When.Date,
                       root.WhenRealizeProfit.Date,
                       buyItem.Price * buyItem.AssetUsdValue,
                       root.SellItem.Price * root.SellItem.AssetUsdValue,
                       sellCommisionInUsd + buyCommision
                       );
                }
            }
        }

        public static IEnumerable<RootElement> CreateRootElements(IEnumerable<T2TradeEntity> trades)
        {
            //group by base asset
            foreach (var trade in trades.GroupBy(
                p => p.T2SymbolInfo?.BaseAsset,
                p => p,
                (baseAsset, entities) => new { baseAsset, entities }))
            {
                //create root element
                var rootElement = new RootElement();

                //create collection of elements and assign to root
                foreach (var entity in trade.entities.OrderBy(o => o.TradeTime))
                {
                    var element = Converter.Convert(entity);
                    rootElement.Items.Add(element);
                }

                yield return rootElement;
            }
        }
    }
}
