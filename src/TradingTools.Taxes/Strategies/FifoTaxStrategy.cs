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
        private const int DEFAULT_REMAINDER = -999999999;

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
            TaxData = new List<TaxDataRoot>();

            foreach (var root in Roots)
            {
                if (root.Symbol.StartsWith("VET"))
                { 
                }
                var buyList = new Queue<Element>(root.Items.Where(p => p.IsBuyer).OrderBy(o => o.TradeDateTime));
                var sellQ = new Queue<Element>(root.Items.Where(p => !p.IsBuyer).OrderBy(o => o.TradeDateTime));

                while (sellQ.Count > 0)
                {
                    if (!buyList.Any())
                    {
                        _logger.LogWarning($"You sold more then bought! Remaining {sellQ.Sum(s => s.Amount)}{root.Symbol}");
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

            RemainderRecurse(buyList, sellElement, result);

            return result;
        }

        private void RemainderRecurse(Queue<Element> buyList, Element sellElement, List<Element> result)
        {
            if (!buyList.Any())
            {
                _logger.LogWarning($"No more buy element.");
                return;
            }

            var buyElement = buyList.First();
            var remainder = buyElement.Amount - sellElement.Amount;

            //remainder <= 0 is ok but
            if (remainder > 0)
            {
                buyElement.Amount -= sellElement.Amount;
                //protože potřebuju buy element nechat v kolekci pro další sellElement, ale zároveŃ potřebuju kus buy elementu do
                //resultu ... potřebuju jakoby rozpůlit element
                var cloned = buyElement.Clone();
                result.Add((Element)cloned);
            }
            else
            {
                result.Add(buyList.Dequeue());
                //toto je zbytek, který potřebuju v dalším cyklu "dodělat"
                sellElement.Amount = Math.Abs(remainder);
            }


            if (remainder >= 0)
            {
                //protože jsem všechnu quantity sell item vecpal do buy item
                return;
            }

            //if (remainder <= 0)
            //{
            //    return;
            //}

            RemainderRecurse(buyList, sellElement, result);
        }

        public override void Generate(GenerateType type)
        {
            throw new NotImplementedException();
        }
    }
}
