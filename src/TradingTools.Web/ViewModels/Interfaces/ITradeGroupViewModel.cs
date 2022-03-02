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
        decimal BuyQuoteQuantity { get; }
        decimal Gain { get; }
        string GainString { get; }
        decimal GainQuoteAsset { get; }
        string GainQuoteAssetString { get; }
        decimal RemaingPosition { get; }
        string RemaingPositionString { get; }
        decimal RemaingQuotePosition { get; }
        decimal RemaingPositionPercentage { get; }
        string RemaingPositionPercentageString { get; }
        decimal SellQuantity { get; }
        decimal SellQuoteQuantity { get; }
        T2TradeGroupEntity TradeGroup { get; set; }
        IList<TradeTimeLineValueViewModel> Trades { get; }
        decimal ClosestProfit { get; }
        string ClosestProfitString { get; }

        int GetDecimalPlaces(string asset);
        void Init(T2TradeGroupEntity tradeGroup);

        decimal CurrentChangePercentage { get; }
        string CurrentChangePercentageString { get; }
    }
}
