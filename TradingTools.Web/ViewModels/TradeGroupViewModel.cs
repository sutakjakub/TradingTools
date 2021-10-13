using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradingTools.Core.Business.Interfaces;
using TradingTools.Db.Entities;
using TradingTools.Web.ViewModels.Interfaces;

namespace TradingTools.Web.ViewModels
{
    public class TradeGroupViewModel : ITradeGroupViewModel
    {
        private readonly ITradeGroupStatisticsBusiness _business;

        public TradeGroupViewModel(
            ITradeGroupStatisticsBusiness business
            )
        {
            _business = business;
        }

        public T2TradeGroupEntity TradeGroup { get; set; }

        public decimal AverageBuyPrice { get; private set; }
        public decimal AverageSellPrice { get; private set; }
        public decimal AverageCost { get; private set; }
        public decimal BuyQuantity { get; private set; }
        public decimal SellQuantity { get; private set; }
        public decimal Gain { get; private set; }
        public string GainString => $"{decimal.Round(Gain * 100, 2)}%";
        public decimal RemaingPositionPercentage { get; private set; }
        public string RemaingPositionPercentageString => $"{decimal.Round(RemaingPositionPercentage * 100, 2)}%";

        public async Task Init(T2TradeGroupEntity tradeGroup)
        {
            TradeGroup = tradeGroup;

            var trades = tradeGroup.Trades.ToList();
            AverageBuyPrice = _business.AverageBuyPrice(trades);
            AverageSellPrice = _business.AverageSellPrice(trades);
            AverageCost = _business.AverageCost(trades);
            BuyQuantity = _business.BuyQuantity(trades);
            SellQuantity = _business.SellQuantity(trades);
            Gain = await _business.Gain(trades);
            RemaingPositionPercentage = _business.RemaingPositionPercentage(trades);
        }
    }
}
