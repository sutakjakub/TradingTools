using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradingTools.Db.Entities;
using TradingTools.Shared.Dto;

namespace TradingTools.Web.ViewModels
{
    public class BinanceUserCoinViewModel
    {
        public BinanceUserCoinViewModel(T2PortfolioCoinEntity coin)
        {
            Coin = coin;
        }

        public T2PortfolioCoinEntity Coin { get; }

        public decimal CurrentTimeLineDollarValue { get; set; }
    }
}
