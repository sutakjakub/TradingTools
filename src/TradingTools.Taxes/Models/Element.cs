using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingTools.Taxes.Models
{
    public class Element
    {
        public long T2TradeId { get; set; }
        public decimal Price { get; set; }
        public bool IsBuyer { get; set; }
        public decimal Amount { get; set; }
        public DateTimeOffset TradeDateTime { get; set; }

        /// <summary>
        /// Subtract amount and returns remainder.
        /// </summary>
        /// <example>
        /// Amount=0.8, amount=0.2 => Amount=0.6;returns 0;
        /// Amount=0.2, amount=0.8 => Amount=0;returns 0.6;
        /// </example>
        /// <param name="amount">Amount for subtract</param>
        /// <returns>Returns remainder</returns>
        public decimal Subtract(decimal amount)
        {
            var sub = Amount - amount;
            if (sub >= 0)
            {
                Amount = sub;
                return 0;
            }
            else
            {
                Amount = 0;
                return sub * -1;
            }

            
        }
    }

    public class RootElement
    {
        public IList<Element> Items { get; set; }
        public string AssetName { get; set; }

        public RootElement()
        {
            Items = new List<Element>();
        }
    }
}
