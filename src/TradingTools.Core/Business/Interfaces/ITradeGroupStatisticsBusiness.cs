using System.Collections.Generic;
using System.Threading.Tasks;
using TradingTools.Db.Entities;

namespace TradingTools.Core.Business.Interfaces
{
    public interface ITradeGroupStatisticsBusiness
    {
        decimal AverageBuyPrice(IEnumerable<T2TradeEntity> trades);
        decimal AverageCost(IEnumerable<T2TradeEntity> trades);
        decimal AverageSellPrice(IEnumerable<T2TradeEntity> trades);
        decimal BuyQuantity(IEnumerable<T2TradeEntity> trades, bool quoteAsset = false);
        decimal TotalGain(IEnumerable<T2TradeEntity> trades);
        decimal SellQuantity(IEnumerable<T2TradeEntity> trades, bool quoteAsset = false);
        decimal RemaingPositionPercentage(IEnumerable<T2TradeEntity> trades);
        decimal RemaingPosition(IEnumerable<T2TradeEntity> trades, bool quoteAsset = false);
    }
}