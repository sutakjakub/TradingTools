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
        private readonly TaxStrategy _strategy;

        public IList<CalcHistoryModel> CalcHistoryItems { get; set; }

        public IList<TaxReportItem> TaxReportItems { get; private set; }

        public TaxCalculator(TaxStrategy strategy)
        {
            _strategy = strategy;
        }

        public void Calculate(IEnumerable<T2TradeEntity> entities)
        {
            var roots = CreateRootElements(entities);
            _strategy.Load(roots);
            _strategy.Process();

            TaxReportItems = ConvertToItems(_strategy.TaxData).OrderByDescending(o => o.DateSold).ToList();
        }

        public static IEnumerable<TaxReportItem> ConvertToItems(IList<TaxDataRoot> source)
        {
            foreach (var root in source)
            {
                var commisionInUsd = root.SellItem.Commision.Amount * root.SellItem.Commision.AssetUsdValue;
                commisionInUsd /= root.BuyItems.Count;

                foreach (var buyItem in root.BuyItems)
                {
                    yield return TaxReportItem.CreateBuyItem(
                       buyItem.Amount,
                       buyItem.AssetName,
                       buyItem.QuoteAssetName,
                       root.SellItem.AssetUsdValue,
                       buyItem.When.Date,
                       root.WhenRealizeProfit.Date,
                       buyItem.Price,
                       root.SellItem.Price,
                       buyItem.Commision.Amount,
                       buyItem.Commision.AssetName,
                       buyItem.Commision.AssetUsdValue,
                       commisionInUsd
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
