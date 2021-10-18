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

        public IList<TradeTimeLineValueViewModel> Trades { get; } = new List<TradeTimeLineValueViewModel>();

        public decimal AverageBuyPrice { get; private set; }
        public decimal AverageSellPrice { get; private set; }
        public decimal AverageCost { get; private set; }
        public decimal BuyQuantity { get; private set; }
        public decimal SellQuantity { get; private set; }
        public decimal Gain { get; private set; }
        public string GainString => $"{decimal.Round(Gain * 100, 2)}%";
        public decimal GainQuoteAsset { get; private set; }
        public string GainQuoteAssetString
        {
            get
            {
                int decimalPlaces = 8;
                if (TradeGroup.SymbolInfo != null)
                {
                    decimalPlaces = TradeGroup.SymbolInfo.QuoteAsset == "USDT" ? 2 : 8;
                }
                return $"{decimal.Round(GainQuoteAsset, decimalPlaces)}{TradeGroup.SymbolInfo?.QuoteAsset}";
            }
        }
        public decimal RemaingPositionPercentage { get; private set; }
        public string RemaingPositionPercentageString => $"{decimal.Round(RemaingPositionPercentage * 100, 2)}%";

        public void Init(T2TradeGroupEntity tradeGroup)
        {
            TradeGroup = tradeGroup;

            var trades = tradeGroup.Trades.ToList();
            if (trades?.Any() == true)
            {
                AverageBuyPrice = _business.AverageBuyPrice(trades);
                AverageSellPrice = _business.AverageSellPrice(trades);
                AverageCost = _business.AverageCost(trades);
                BuyQuantity = _business.BuyQuantity(trades);
                SellQuantity = _business.SellQuantity(trades);
                Gain = _business.TotalGain(trades);
                GainQuoteAsset = _business.TotalGainQuoteAsset(trades);
                RemaingPositionPercentage = _business.RemaingPositionPercentage(trades);

                decimal lastCurrentValue = 0;
                decimal lastBtcQuoteValue = 0;
                decimal lastUsdtQuoteValue = 0;
                for (int i = 0; i < trades.Count; i++)
                {
                    var vmTrade = new TradeTimeLineValueViewModel(trades[i]);
                    if (vmTrade.Trade.IsBuyer)
                    {
                        lastCurrentValue += vmTrade.Trade.Quantity;
                    }
                    else
                    {
                        lastCurrentValue -= vmTrade.Trade.Quantity;
                    }
                    vmTrade.CurrentBaseAssetQuantity = lastCurrentValue;

                    if (vmTrade.Trade.T2SymbolInfo.QuoteAsset == "USDT")
                    {
                        if (vmTrade.Trade.IsBuyer)
                        {
                            lastUsdtQuoteValue += vmTrade.Trade.QuoteQuantity;
                        }
                        else
                        {
                            lastUsdtQuoteValue -= vmTrade.Trade.QuoteQuantity;
                        }
                        vmTrade.CurrentQuoteAssetQuantity = lastUsdtQuoteValue;
                    }
                    else if (vmTrade.Trade.T2SymbolInfo.QuoteAsset == "BTC")
                    {
                        if (vmTrade.Trade.IsBuyer)
                        {
                            lastBtcQuoteValue += vmTrade.Trade.QuoteQuantity;
                        }
                        else
                        {
                            lastBtcQuoteValue -= vmTrade.Trade.QuoteQuantity;
                        }
                        vmTrade.CurrentQuoteAssetQuantity = lastBtcQuoteValue;
                    }


                    //add to collection
                    Trades.Add(vmTrade);
                }
            }
        }
    }
}
