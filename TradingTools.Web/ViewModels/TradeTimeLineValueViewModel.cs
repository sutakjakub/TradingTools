using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradingTools.Db.Entities;

namespace TradingTools.Web.ViewModels
{
    public class TradeTimeLineValueViewModel
    {
        public TradeTimeLineValueViewModel(T2TradeEntity trade)
        {
            Trade = trade;
        }

        public T2TradeEntity Trade { get; private set; }

        public decimal CurrentBaseAssetQuantityPercentage { get; set; }
        public decimal CurrentBaseAssetQuantity { get; set; }
        public decimal CurrentQuoteAssetQuantity { get; set; }
    }
}
