using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradingTools.Db.Entities;

namespace TradingTools.Web.ViewModels.Interfaces
{
    public interface ITradeGroupViewModel
    {
        decimal CurrentPrice { get; set; }
        decimal AverageBuyPrice { get; }
        decimal AverageCost { get; }
        decimal AverageSellPrice { get; }
        decimal BuyQuantity { get; }
        decimal Gain { get; }
        string GainString { get; }
        decimal GainQuoteAsset { get; }
        string GainQuoteAssetString { get; }
        decimal RemaingPositionPercentage { get; }
        string RemaingPositionPercentageString { get; }
        decimal SellQuantity { get; }
        T2TradeGroupEntity TradeGroup { get; set; }
        IList<TradeTimeLineValueViewModel> Trades { get; }
        decimal ClosestProfit { get; }
        string ClosestProfitString { get; }

        int GetDecimalPlaces(string asset);
        void Init(T2TradeGroupEntity tradeGroup);
    }
}
