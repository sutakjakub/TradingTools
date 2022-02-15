using System;
using System.Collections.Generic;
using System.Linq;
using TradingTools.Db.Entities;
using TradingTools.Taxes.Models;

namespace TradingTools.Taxes
{
    public class TaxReport
    {
        public IList<TaxReportItem> Items { get; private set; }

        public void Load(IEnumerable<T2TradeEntity> trades)
        {
            var rootElements = CreateRootElements(trades);

            foreach (var root in rootElements)
            {
                var buyList = new List<Element>(root.Items.Where(p => p.IsBuyer));
                var sellQ = new Queue<Element>(root.Items.Where(p => !p.IsBuyer));

                foreach (var sellElement in sellQ)
                {
                    if (buyList.Count > 1)
                    {
                        break;
                    }

                    var buyElement = buyList[0];
                    var buyPreviousAmount = buyElement.Amount;
                    var remainder = buyElement.Subtract(sellElement.Amount);

                    var taxReportItem = TaxReportItem.Create(
                        buyElement.Amount == 0 ? buyPreviousAmount : sellElement.Amount,
                        root.AssetName,
                        buyElement.TradeDateTime,
                        sellElement.TradeDateTime,
                        buyElement.Price,
                        sellElement.Price);

                    if (buyElement.Amount == 0)
                    {
                        buyList.RemoveAt(0);
                    }

                    RemainderRecurse(remainder, buyList);
                }
            }
        }

        private static void RemainderRecurse(decimal remainder, List<Element> buyList)
        {
            if (remainder <= 0)
            {
                return;
            }

            var buyElement = buyList[0];
            var newRemainder = buyElement.Subtract(remainder);
            if (buyElement.Amount == 0)
            {
                buyList.RemoveAt(0);
            }

            RemainderRecurse(newRemainder, buyList);
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
                var rootElement = new RootElement
                {
                    AssetName = trade.baseAsset
                };

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
