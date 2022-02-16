using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingTools.Shared.Dto
{
    public class BinanceUserCoinDto
    {
        public string Coin { get; set; }
        public decimal Free { get; set; }
        public decimal Locked { get; set; }
        public string Name { get; set; }

    }
}
