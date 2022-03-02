using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingTools.Core.Business.Interfaces;
using TradingTools.Db.Entities;
using TradingTools.ExchangeServices.Interfaces;
using TradingTools.MathLib;

namespace TradingTools.Core.Business
{
    public class TradeGroupStatisticsBusiness : ITradeGroupStatisticsBusiness
    {
        private const int DECIMAL_PLACES = 8;

        public decimal AverageBuyPrice(IEnumerable<T2TradeEntity> trades)
        {
            var entries = CreateEntries(trades, true);
            return BasicCalculator.AverageCost(entries);
        }

        public decimal AverageSellPrice(IEnumerable<T2TradeEntity> trades)
        {
            var entries = CreateEntries(trades, false);
            return BasicCalculator.AverageCost(entries);
        }

        public decimal AverageCost(IEnumerable<T2TradeEntity> trades)
        {
            return BasicCalculator.AverageCost(CreateEntries(trades, true), CreateEntries(trades, false), DECIMAL_PLACES);
        }

        public decimal BuyQuantity(IEnumerable<T2TradeEntity> trades, bool quoteAsset = false)
        {
            return Quantity(trades, true, quoteAsset);
        }

        public decimal SellQuantity(IEnumerable<T2TradeEntity> trades, bool quoteAsset = false)
        {
            return Quantity(trades, false, quoteAsset);
        }

        public decimal TotalGain(IEnumerable<T2TradeEntity> trades)
        {

            var averageBuyPrice = AverageBuyPrice(trades);
            var averageSellPrice = AverageSellPrice(trades);
            if (averageSellPrice == 0)
            {
                return 0;
            }

            return (averageSellPrice - averageBuyPrice) / averageSellPrice;
        }

        //public decimal TotalGain(IEnumerable<T2TradeEntity> trades)
        //{
        //    var totalGain = TotalGain(trades);
        //    var sum = trades.Where(p => p.IsBuyer).Sum(s => s.Quantity);

        //    return totalGain * sum;
        //}

         public decimal RemaingPosition(IEnumerable<T2TradeEntity> trades, bool quoteAsset = false)
        {
            var total = BuyQuantity(trades, quoteAsset);
            if (total == 0)
            {
                return 0;
            }
            return total - SellQuantity(trades, quoteAsset);
        }

        public decimal RemaingPositionPercentage(IEnumerable<T2TradeEntity> trades) 
        {
            var total = BuyQuantity(trades);
            if (total == 0)
            {
                return 0;
            }
            var diff = total - SellQuantity(trades);
            return diff / total;
        }

        private static decimal Quantity(IEnumerable<T2TradeEntity> trades, bool isBuyer, bool quoteAsset = false)
        {
            return trades
                .Where(p => p.IsBuyer == isBuyer)
                .Sum(s => quoteAsset ? s.QuoteQuantity : s.Quantity);
        }

        private static IEnumerable<(decimal quantity, decimal price)> CreateEntries(IEnumerable<T2TradeEntity> trades, bool isBuyer)
        {
            return trades
               .Where(p => p.IsBuyer == isBuyer)
               .Select(c => new { c.Quantity, c.Price })
               .AsEnumerable()
               .Select(s => (quantity: s.Quantity, price: s.Price));
        }
    }
}
