using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingTools.Taxes.Models;

namespace TradingTools.Taxes.Strategies
{
    public class FifoTaxStrategy : TaxStrategy
    {
        private readonly ILogger<FifoTaxStrategy> _logger;

        public FifoTaxStrategy(
            ILogger<FifoTaxStrategy> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        private List<TaxReportItem> taxReportItems;

        public override void Process()
        {
            taxReportItems = new List<TaxReportItem>();

            foreach (var root in Roots)
            {
                var buyList = new Queue<Element>(root.Items.Where(p => p.IsBuyer).OrderBy(o => o.TradeDateTime));
                var sellQ = new Queue<Element>(root.Items.Where(p => !p.IsBuyer));

                while (sellQ.Count > 0)
                {
                    if (!buyList.Any())
                    {
                        _logger.LogWarning($"You sold more then bought! Remaining {sellQ.Sum(s => s.Amount)}{root.AssetName}");
                        break;
                    }

                    var sellElement = sellQ.Dequeue();
                    var result = ProcessSellElement(sellElement, buyList);

                    TaxData.Add(TaxDataRoot.Create(sellElement, result));
                }
            }
        }

        private IList<Element> ProcessSellElement(Element sellElement, Queue<Element> buyList)
        {
            var result = new List<Element>();

            decimal remainder = 0;
            RemainderRecurse(ref remainder, buyList, sellElement, result);

            return result;
        }

        private void RemainderRecurse(ref decimal remainder, Queue<Element> buyList, Element sellElement, List<Element> result)
        {
            if (!buyList.Any())
            {
                _logger.LogWarning($"No more buy element for Remainder={remainder}.");
                return;
            }

            var buyElement = buyList.First();
            remainder = buyElement.Amount - sellElement.Amount;

            //remainder <= 0 is ok but
            if (remainder > 0)
            {
                buyElement.Amount -= remainder;
                //protože potřebuju buy element nechat v kolekci pro další sellElement, ale zároveŃ potřebuju kus buy elementu do
                //resultu ... potřebuju jakoby rozpůlit element
                var cloned = buyElement.Clone();
                result.Add((Element)cloned);
            }
            else
            {
                result.Add(buyList.Dequeue());
            }

            if (remainder > 0)
            {
                return;
            }

            RemainderRecurse(ref remainder, buyList, sellElement, result);
        }

        public override void Generate(GenerateType type)
        {
            throw new NotImplementedException();
        }
    }
}
