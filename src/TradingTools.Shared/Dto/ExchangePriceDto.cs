using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingTools.Shared.Dto
{
    public class ExchangePriceDto
    {
        public decimal Price { get; set; }
        public string Symbol { get; set; }
        public DateTime? Timestamp { get; set; }
    }
}
