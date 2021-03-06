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

        public decimal CurrentPrice { get; set; }
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
                    decimalPlaces = GetDecimalPlaces(TradeGroup.SymbolInfo.QuoteAsset);
                }
                return $"{decimal.Round(GainQuoteAsset, decimalPlaces)}{TradeGroup.SymbolInfo?.QuoteAsset}";
            }
        }

        public int GetDecimalPlaces(string asset)
        {
            return asset == "USDT" ? 2 : 8;
        }

        public decimal RemaingPositionPercentage { get; private set; }
        public string RemaingPositionPercentageString => $"{decimal.Round(RemaingPositionPercentage * 100, 2)}%";

        public decimal ClosestProfit
        {
            get
            {
                if (TradeGroup.Orders?.Any() == true && CurrentPrice > 0)
                {
                    var minPrice = TradeGroup.Orders.Min(o => o.Price);
                    return Math.Round((100 * CurrentPrice / minPrice) - 100, 2);
                }

                return 0;
            }
        }

        public string ClosestProfitString => $"{ClosestProfit}%";

        public decimal CurrentChangePercentage { get; private set; }
        public string CurrentChangePercentageString
        {
            get
            {
                if (CurrentChangePercentage == 0)
                {
                    return "0%";
                }
                else
                {
                    var d = Math.Round(CurrentChangePercentage, 2);
                    return $"{d}%";
                }
            }
        }

        public decimal RemaingPosition { get; private set; }

        public string RemaingPositionString => $"{Math.Round(RemaingPosition, 8)}{TradeGroup?.SymbolInfo?.BaseAsset}";

        public decimal BuyQuoteQuantity { get; private set; }

        public decimal SellQuoteQuantity { get; private set; }

        public decimal RemaingQuotePosition { get; private set; }

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
                BuyQuoteQuantity = _business.BuyQuantity(trades, true);
                SellQuoteQuantity = _business.SellQuantity(trades, true);
                Gain = _business.TotalGain(trades);
                GainQuoteAsset = _business.TotalGain(trades);
                RemaingPosition = _business.RemaingPosition(trades);
                RemaingPositionPercentage = _business.RemaingPositionPercentage(trades);
                RemaingQuotePosition = _business.RemaingPosition(trades, true);

                if (AverageBuyPrice > 0)
                {
                    CurrentChangePercentage = (CurrentPrice - AverageCost) / AverageBuyPrice * 100;
                }

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

                    if (vmTrade.Trade.T2SymbolInfo != null)
                    {
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
                    }


                    //add to collection
                    Trades.Add(vmTrade);
                }
            }
        }
    }
}
