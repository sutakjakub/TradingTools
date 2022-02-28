using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingTools.Shared.Enums
{
    public enum T2ExchangeType : byte
    {
        None = 0,
        Binance = 1,
        CoinMate = 2,
        Coinbase = 4,
        Huobi = 8,
        Ftx = 16
    }
}
