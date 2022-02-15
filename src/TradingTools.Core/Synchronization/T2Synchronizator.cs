using Binance.Net.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingTools.Core.Synchronization.Interfaces;
using TradingTools.Db;
using TradingTools.Db.Entities;
using TradingTools.ExchangeServices.Interfaces;
using TradingTools.Persistence.Queries;
using TradingTools.Persistence.Queries.Interfaces;
using TradingTools.Persistence.Stores;
using TradingTools.Persistence.Stores.Interfaces;
using TradingTools.Shared.Dto;

namespace TradingTools.Core.Synchronization
{
    public class T2Synchronizator : IT2Synchronizator
    {
        private readonly ILogger<T2Synchronizator> _logger;
        private readonly IT2SymbolInfoQuery _symbolInfoQuery;
        private readonly IT2TradeQuery _tradeQuery;
        private readonly IT2TradeStore _tradeStore;
        private readonly IT2TradeGroupQuery _tradeGroupQuery;
        private readonly IT2TradeGroupStore _tradeGroupStore;
        private readonly IT2OrderQuery _orderQuery;
        private readonly IT2OrderStore _orderStore;
        private readonly IT2PortfolioCoinStore _coinStore;
        private readonly IT2SyncStore _syncStore;
        private readonly IT2NotificationStore _notificationStore;
        private readonly IBinanceExchangeService _exchangeService;

        public T2Synchronizator(
            ILogger<T2Synchronizator> logger,
            IT2SymbolInfoQuery symbolInfoQuery,
            IT2TradeQuery tradeQuery,
            IT2TradeStore tradeStore,
            IT2TradeGroupQuery tradeGroupQuery,
            IT2TradeGroupStore tradeGroupStore,
            IT2OrderQuery orderQuery,
            IT2OrderStore orderStore,
            IT2PortfolioCoinStore coinStore,
            IT2SyncStore syncStore,
            IT2NotificationStore notificationStore,
            IBinanceExchangeService exchangeService)
        {
            _logger = logger;
            _symbolInfoQuery = symbolInfoQuery;
            _tradeQuery = tradeQuery;
            _tradeStore = tradeStore;
            _tradeGroupQuery = tradeGroupQuery;
            _tradeGroupStore = tradeGroupStore;
            _orderQuery = orderQuery;
            _orderStore = orderStore;
            _coinStore = coinStore;
            _syncStore = syncStore;
            _notificationStore = notificationStore;
            _exchangeService = exchangeService;
        }

        public async Task<IEnumerable<long>> SyncBySymbol(string symbol)
        {
            var symbolInfo = await _symbolInfoQuery.FindByName(symbol);
            var lastEntity = await _tradeQuery.FindLastTradeBySymbol(symbolInfo.Symbol);

            var trades = await _exchangeService.GetTradesAsync(symbolInfo.Symbol, lastEntity?.TradeId);
            _logger.LogInformation($"{symbolInfo.Symbol} __ {trades.Count()} ___ {lastEntity?.TradeId}");

            List<long> createdIds = new();
            var collection = ConvertToTrades(trades, symbolInfo.Id);
            if (collection.Any())
            {
                await _notificationStore.Create(new T2NotificationEntity
                {
                    NotificationType = Db.Enums.NotificationType.LogOnly,
                    Message = $"Found {collection.Count()} for {symbol}"
                });
                foreach (var item in collection)
                {
                    var exist = await _tradeQuery.FindByTradeId(item.TradeId);
                    if (exist == null)
                    {
                        var tradeGroup = await _tradeGroupQuery.FindLastGroupBySymbol(symbol);
                        if (tradeGroup == null)
                        {
                            tradeGroup = await _tradeGroupQuery.FindDefaultByBaseAsset(symbolInfo.BaseAsset);
                        }

                        if (tradeGroup == null)
                        {
                            tradeGroup = new T2TradeGroupEntity
                            {
                                BaseAsset = symbolInfo.BaseAsset,
                                Name = $"Default_{symbolInfo.BaseAsset}",
                                IsDefault = true
                            };
                            tradeGroup = await _tradeGroupStore.Create(tradeGroup);
                            await _notificationStore.Create(new T2NotificationEntity
                            {
                                NotificationType = Db.Enums.NotificationType.MustDo,
                                Message = $"Created {tradeGroup.Name}. You must create TradeGroup with symbol {symbol} and assing these trades to it."
                            });
                        }

                        item.T2TradeGroupId = tradeGroup.Id;
                        var created = await _tradeStore.Create(item);
                        await _notificationStore.Create(new T2NotificationEntity
                        {
                            NotificationType = Db.Enums.NotificationType.ForCheck,
                            Message = $"Added trade with T2TradeId={created.T2TradeId} to TradeGroup with Name={tradeGroup.Name}"
                        });

                        createdIds.Add(created.Id);
                    }
                }
            }

            return createdIds;
        }

        public async Task<IEnumerable<long>> SyncByBaseAsset(string baseAsset)
        {
            List<long> createdIds = new();
            var symbols = await _symbolInfoQuery.All();
            foreach (var item in symbols.Where(p => p.BaseAsset == baseAsset))
            {
                var ids = await SyncBySymbol(item.Symbol);
                createdIds.AddRange(ids);
            }

            return createdIds;
        }

        public async Task SyncOpenOrdersBySymbol(string symbol = null, IEnumerable<T2OrderDto> openOrders = null)
        {
            if (openOrders == null)
            {
                openOrders = await _exchangeService.GetOpenOrders(symbol);
            }
            foreach (var orderDto in openOrders)
            {
                var order = await _orderQuery.FindByOrderId(orderDto.OrderId);
                if (order == null)
                {
                    try
                    {
                        order = ConvertToOrder(orderDto);
                        var tradeGroup = await _tradeGroupQuery.FindLastGroupBySymbol(order.Symbol);
                        var symbolInfo = await _symbolInfoQuery.FindByName(order.Symbol);
                        order.T2TradeGroupId = tradeGroup?.Id;
                        order.T2SymbolInfoId = symbolInfo.Id;

                        await _orderStore.Create(order);
                        await _notificationStore.Create(new T2NotificationEntity
                        {
                            NotificationType = Db.Enums.NotificationType.ForCheck,
                            Message = $"Assing new OpenOrder: TradeGroupName={tradeGroup?.Name} | {symbolInfo.Symbol} | {order.Side} | {order.Price}{symbolInfo.QuoteAsset} | {order.Quantity}{symbolInfo.BaseAsset}"
                        });
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }

            IEnumerable<T2OrderEntity> allOrders;
            if (symbol == null)
            {
                allOrders = await _orderQuery.All();
            }
            else
            {
                allOrders = await _orderQuery.FindBySymbol(symbol);
            }

            var ordersIds = openOrders.Select(s => s.OrderId);
            var deleteCandidatesIds = allOrders.Where(p => !ordersIds.Contains(p.OrderId)).Select(o => o.Id).ToArray();

            await _orderStore.Delete(deleteCandidatesIds);
            await _notificationStore.Create(new T2NotificationEntity
            {
                NotificationType = Db.Enums.NotificationType.ForCheck,
                Message = $"Deleted open orders Count={deleteCandidatesIds.Count()} for symbol={symbol ?? "All"}"
            });
        }

        public async Task SyncByPortfolio()
        {
            throw new NotImplementedException();
            //var sync = await _syncStore.Create(new T2SyncEntity { StartDate = DateTime.Now, State = Db.Enums.SyncState.InProgress });
            //var lastPortfolio = await _coinStore.FindLastPortfolio();
            //var currentPortfolio = await _exchangeService.GetUserCoinsAsync();

            //foreach (var lastItem in lastPortfolio)
            //{
            //    var symbol = lastItem.T2SymbolInfo.Symbol;
            //    var currentItem = currentPortfolio.FirstOrDefault(f => f.Coin == symbol);
            //    if (currentItem == null)
            //    {
            //        _logger.LogWarning($"{symbol} with syncId={lastItem.T2SyncId} was not found in current portfolio.");
            //        continue;
            //    }

            //    var entity = await ConvertToPortfolioCoin(currentItem);
            //    entity.T2SyncId = sync.Id;

            //    await _coinStore.Create(entity);

            //    var tradeGroup = await _tradeGroupQuery.FindLastGroupBySymbol(symbol);
            //    if (tradeGroup == null || tradeGroup.TradeGroupState == Db.Enums.TradeGroupState.Done)
            //    {
            //        ////create tradeGroup
            //        //var tradeGroup = new T2TradeGroupEntity
            //        //{
            //        //    Name = symbol,
            //        //    Trades = 
            //        //};
            //    }
            //    else
            //    {
            //        var needUpdate = false;
            //        if (entity.Free != currentItem.Free)
            //        {
            //            //TODO: notification for buy
            //            needUpdate = true;
            //        }
            //        if (entity.Locked != currentItem.Locked)
            //        {
            //            needUpdate = true;
            //            if (entity.Locked < currentItem.Locked)
            //            {
            //                //TODO: notification removing order
            //            }
            //            else if (entity.Locked > currentItem.Locked)
            //            {
            //                //TODO: notification for creating order 
            //            }
            //        }
            //        if (needUpdate)
            //        {
            //            await SyncBySymbol(symbol);
            //            await SyncOpenOrdersBySymbol(symbol);
            //        }
            //    }
            //    //TODO: notification
            //}
        }

        public async Task SyncPortfolio()
        {
            var prices = await _exchangeService.GetPricesAsync();
            var btcUsdtPrice = prices.First(f => f.Symbol == "BTCUSDT").Price;

            var version = await _coinStore.GetLastVersion();
            version++;
            var coins = await _exchangeService.GetUserCoinsAsync();
            foreach (var item in coins)
            {
                var coin = ConvertToPortfolioCoin(item);
                coin.Version = version;

                //check quote asset
                var quoteAsset = "USDT";
                var price = prices.FirstOrDefault(f => f.Symbol == item.Coin + "USDT");
                if (price == null)
                {
                    price = prices.FirstOrDefault(f => f.Symbol == item.Coin + "BTC");
                    quoteAsset = "BTC";
                }

                coin.TotalQuoteValue = (coin.Free + coin.Locked) * price?.Price ?? 0;
                coin.TotalQuoteAssetName = quoteAsset;
                coin.TotalDollarAssetName = "USDT";

                if (quoteAsset == "USDT")
                {
                    coin.TotalQuoteValue = Math.Round(coin.TotalQuoteValue, 2);
                    coin.TotalDollarValue = coin.TotalQuoteValue;
                }
                else if (quoteAsset == "BTC")
                {
                    coin.TotalQuoteValue = Math.Round(coin.TotalQuoteValue, 8);
                    coin.TotalDollarValue = Math.Round(coin.TotalQuoteValue * btcUsdtPrice, 2);
                }
                else
                {
                    coin.TotalDollarValue = 0;
                }

                if (coin.TotalDollarValue > 3)
                {
                    await _coinStore.Create(coin);
                }
            }
            var usdt = coins.FirstOrDefault(f => f.Coin == "USDT");
            var usdtCoin = ConvertToPortfolioCoin(usdt);
            usdtCoin.Version = version;
            await _coinStore.Create(usdtCoin);
        }

        private T2PortfolioCoinEntity ConvertToPortfolioCoin(BinanceUserCoinDto dto)
        {
            return new T2PortfolioCoinEntity
            {
                Free = dto.Free,
                Locked = dto.Locked,
                Coin = dto.Coin,
                CoinName = dto.Name
            };
        }

        private T2OrderEntity ConvertToOrder(T2OrderDto order)
        {
            return new T2OrderEntity
            {
                AverageFillPrice = order.AverageFillPrice,
                ClientOrderId = order.ClientOrderId,
                OrderId = order.OrderId,
                CreateTime = order.CreateTime,
                IsWorking = order.IsWorking,
                OrderListId = order.OrderListId,
                OriginalClientOrderId = order.OriginalClientOrderId,
                Price = order.Price,
                Quantity = order.Quantity,
                QuantityFilled = order.QuantityFilled,
                QuantityRemaining = order.QuantityRemaining,
                QuoteQuantity = order.QuoteQuantity,
                QuoteQuantityFilled = order.QuoteQuantityFilled,
                Side = order.Side,
                Status = order.Status,
                StopPrice = order.StopPrice,
                Symbol = order.Symbol,
                TimeInForce = order.TimeInForce,
                Type = order.Type,
                UpdateTime = order.UpdateTime
            };
        }

        private static IEnumerable<T2TradeEntity> ConvertToTrades(IEnumerable<T2TradeDto> trades, long symbolId)
        {
            return trades.Select(s => new T2TradeEntity
            {
                Commission = s.Commission,
                CommissionAsset = s.CommissionAsset,
                ExchangeType = s.ExchangeType,
                OrderId = s.OrderId,
                Price = s.Price,
                Quantity = s.Quantity,
                QuoteQuantity = s.QuoteQuantity,
                Symbol = s.Symbol,
                TradeId = s.TradeId,
                TradeTime = s.TradeTime,
                IsBestMatch = s.IsBestMatch,
                IsBuyer = s.IsBuyer,
                IsMaker = s.IsMaker,
                T2SymbolInfoId = symbolId
            });
        }


    }
}
