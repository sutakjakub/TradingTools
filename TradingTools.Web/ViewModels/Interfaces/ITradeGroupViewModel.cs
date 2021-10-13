using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradingTools.Db.Entities;

namespace TradingTools.Web.ViewModels.Interfaces
{
    public interface ITradeGroupViewModel
    {
        decimal AverageBuyPrice { get; }
        decimal AverageCost { get; }
        decimal AverageSellPrice { get; }
        decimal BuyQuantity { get; }
        decimal Gain { get; }
        string GainString { get; }
        decimal RemaingPositionPercentage { get; }
        string RemaingPositionPercentageString { get; }
        decimal SellQuantity { get; }
        T2TradeGroupEntity TradeGroup { get; set; }

        Task Init(T2TradeGroupEntity tradeGroup);
    }
}
