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

        public TaxCalculator(TaxStrategy strategy)
        {
            _strategy = strategy;
        }

        public void Calculate(IEnumerable<T2TradeEntity> entities)
        {
            var roots = CreateRootElements(entities);
            _strategy.Load(roots);
            _strategy.Process();

            //_strategy.TaxData
        }

        public static IEnumerable<RootElement> CreateRootElements(IEnumerable<T2TradeEntity> trades)
        {
            //group by base asset
            foreach (var trade in trades.GroupBy(
                p => p.T2SymbolInfo.BaseAsset,
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
