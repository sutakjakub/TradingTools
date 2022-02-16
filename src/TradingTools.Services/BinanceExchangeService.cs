using Binance.Net.Enums;
using Binance.Net.Interfaces;
using Binance.Net.Objects.Spot.SpotData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradingTools.ExchangeServices.Interfaces;
using TradingTools.Shared.Dto;
using TradingTools.Shared.Enums;

namespace TradingTools.ExchangeServices
{
    public class BinanceExchangeService : IBinanceExchangeService
    {
        private readonly IBinanceClient _client;
        private bool disposedValue;

        public BinanceExchangeService(IBinanceClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<IEnumerable<T2SymbolInfoDto>> GetAllSymbolsAsync()
        {
            var result = await _client.Spot.System.GetExchangeInfoAsync();
            var symbols = result.Data?.Symbols;
            if (symbols == null)
            {
                return Enumerable.Empty<T2SymbolInfoDto>();
            }

            var list = new List<T2SymbolInfoDto>();
            foreach (var symbol in symbols)
            {
                list.Add(
                    new T2SymbolInfoDto
                    {
                        BaseAsset = symbol.BaseAsset,
                        BaseAssetCommissionPrecision = symbol.BaseCommissionPrecision,
                        BaseAssetPrecision = symbol.BaseAssetPrecision,
                        ExchangeType = T2ExchangeType.Binance,
                        QuoteAsset = symbol.QuoteAsset,
                        QuoteAssetCommissionPrecision = symbol.QuoteCommissionPrecision,
                        QuoteAssetPrecision = symbol.QuoteAssetPrecision,
                        Symbol = symbol.Name
                    });
            }

            return list;
        }

        public async Task<T2OrderDto> GetOrderAsync(string symbol, long orderId)
        {
            var result = await _client.Spot.Order.GetOrderAsync(symbol, orderId);
            var order = result?.Data;

            if (order == null)
            {
                return null;
            }
            
            return ConvertTo(order);
        }

        private static T2OrderDto ConvertTo(BinanceOrder order)
        {
            return new T2OrderDto
            {
                AverageFillPrice = order.AverageFillPrice,
                ClientOrderId = order.ClientOrderId,
                CreateTime = order.CreateTime,
                IsWorking = order.IsWorking,
                OrderId = order.OrderId,
                OrderListId = order.OrderListId,
                OriginalClientOrderId = order.OriginalClientOrderId,
                Price = order.Price,
                Quantity = order.Quantity,
                QuantityFilled = order.QuantityFilled,
                QuoteQuantity = order.QuoteQuantity,
                QuoteQuantityFilled = order.QuoteQuantityFilled,
                QuantityRemaining = order.QuantityRemaining,
                Side = ConvertTo(order.Side),
                Status = ConvertTo(order.Status),
                StopPrice = order.StopPrice,
                Symbol = order.Symbol,
                TimeInForce = ConvertTo(order.TimeInForce),
                Type = ConvertTo(order.Type),
                UpdateTime = order.UpdateTime
            };
        }

        public async Task<IEnumerable<T2OrderDto>> GetOpenOrders(string symbol = null)
        {
            var openOrders = await _client.Spot.Order.GetOpenOrdersAsync(symbol);

            var result = new List<T2OrderDto>();
            foreach (var item in openOrders?.Data)
            {
                result.Add(ConvertTo(item));
            }

            return result;
        }

        //public async Task<IEnumerable<T2OrderDto>> GetOpenOcoOrders()
        //{
        //    var openOcoOrders = await _client.Spot.Order.GetOpenOcoOrdersAsync();

        //    var result = new List<T2OrderDto>();
        //    foreach (var oco in openOcoOrders?.Data)
        //    {
        //        result.Add(ConvertTo(oco.));
        //    }

        //    return result;
        //}

        private static T2OrderType ConvertTo(OrderType type)
        {
            return type switch
            {
                OrderType.Limit => T2OrderType.Limit,
                OrderType.LimitMaker => T2OrderType.LimitMaker,
                OrderType.Liquidation => T2OrderType.Liquidation,
                OrderType.Market => T2OrderType.Market,
                OrderType.Stop => T2OrderType.Stop,
                OrderType.StopLoss => T2OrderType.StopLoss,
                OrderType.StopLossLimit => T2OrderType.StopLossLimit,
                OrderType.StopMarket => T2OrderType.StopMarket,
                OrderType.TakeProfit => T2OrderType.TakeProfit,
                OrderType.TakeProfitLimit => T2OrderType.TakeProfitLimit,
                OrderType.TakeProfitMarket => T2OrderType.TakeProfitMarket,
                OrderType.TrailingStopMarket => T2OrderType.TrailingStopMarket,
                _ => throw new InvalidOperationException($"Unknown type {type}"),
            };
        }

        private static T2TimeInForce ConvertTo(TimeInForce timeInForce)
        {
            return timeInForce switch
            {
                TimeInForce.FillOrKill => T2TimeInForce.FillOrKill,
                TimeInForce.GoodTillCancel => T2TimeInForce.GoodTillCancel,
                TimeInForce.GoodTillCrossing => T2TimeInForce.GoodTillCrossing,
                TimeInForce.GoodTillExpiredOrCanceled => T2TimeInForce.GoodTillExpiredOrCanceled,
                TimeInForce.ImmediateOrCancel => T2TimeInForce.ImmediateOrCancel,
                _ => throw new InvalidOperationException($"Unknown timeInForce {timeInForce}"),
            };
        }

        private static T2OrderStatus ConvertTo(OrderStatus status)
        {
            return status switch
            {
                OrderStatus.Adl => T2OrderStatus.Adl,
                OrderStatus.Canceled => T2OrderStatus.Canceled,
                OrderStatus.Expired => T2OrderStatus.Expired,
                OrderStatus.Filled => T2OrderStatus.Filled,
                OrderStatus.Insurance => T2OrderStatus.Insurance,
                OrderStatus.New => T2OrderStatus.New,
                OrderStatus.PartiallyFilled => T2OrderStatus.PartiallyFilled,
                OrderStatus.PendingCancel => T2OrderStatus.PendingCancel,
                OrderStatus.Rejected => T2OrderStatus.Rejected,
                _ => throw new InvalidOperationException($"Unknown status {status}"),
            };
        }

        private static T2OrderSide ConvertTo(OrderSide side)
        {
            return side switch
            {
                OrderSide.Buy => T2OrderSide.Buy,
                OrderSide.Sell => T2OrderSide.Sell,
                _ => throw new InvalidOperationException($"Unknown side {side}"),
            };
        }

        public async Task<IEnumerable<T2TradeDto>> GetTradesAsync(string symbol, long? fromId = null)
        {
            var result = await _client.Spot.Order.GetUserTradesAsync(symbol, null, new DateTime(2020, 1, 1), null, null, fromId);
            var trades = result?.Data.ToList();

            if (trades == null)
            {
                return Enumerable.Empty<T2TradeDto>();
            }

            if (fromId != null)
            {
                var itemToRemove = trades.SingleOrDefault(r => r.Id == fromId);
                if (itemToRemove != null)
                {
                    trades.Remove(itemToRemove);
                }
            }

            var list = new List<T2TradeDto>();
            foreach (var trade in trades)
            {
                list.Add(
                    new T2TradeDto
                    {
                        Commission = trade.Commission,
                        CommissionAsset = trade.CommissionAsset,
                        ExchangeType = T2ExchangeType.Binance,
                        OrderId = trade.OrderId,
                        Price = trade.Price,
                        Quantity = trade.Quantity,
                        QuoteQuantity = trade.QuoteQuantity,
                        Symbol = trade.Symbol,
                        TradeTime = trade.TradeTime,
                        TradeId = trade.Id,
                        IsBestMatch = trade.IsBestMatch,
                        IsBuyer = trade.IsBuyer,
                        IsMaker = trade.IsMaker
                    });
            }

            return list;
        }

        public async Task<decimal> GetPriceAsync(string symbol)
        {
            var result = await _client.Spot.Market.GetPriceAsync(symbol);
            return result.Data.Price;
        }

        public async Task<IEnumerable<ExchangePriceDto>> GetPricesAsync()
        {
            var result = await _client.Spot.Market.GetPricesAsync();
            var list = result.Data.ToList();

            return list.Select(s => new ExchangePriceDto { Price = s.Price, Symbol = s.Symbol, Timestamp = s.Timestamp });
        }

        public async Task<IEnumerable<BinanceUserCoinDto>> GetUserCoinsAsync()
        {
            var result = await _client.General.GetUserCoinsAsync();
            var list = result.Data.ToList();

            return list.Select(s => new BinanceUserCoinDto { Coin = s.Coin, Free = s.Free, Locked = s.Locked, Name = s.Name });
        }
    }
}
