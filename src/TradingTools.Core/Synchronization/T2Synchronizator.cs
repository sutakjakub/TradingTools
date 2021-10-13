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
        private readonly IBinanceExchangeService _exchangeService;

        public T2Synchronizator(
            ILogger<T2Synchronizator> logger,
            IT2SymbolInfoQuery symbolInfoQuery,
            IT2TradeQuery tradeQuery,
            IT2TradeStore tradeStore,
            IT2TradeGroupQuery tradeGroupQuery,
            IBinanceExchangeService exchangeService)
        {
            _logger = logger;
            _symbolInfoQuery = symbolInfoQuery;
            _tradeQuery = tradeQuery;
            _tradeStore = tradeStore;
            _tradeGroupQuery = tradeGroupQuery;
            _exchangeService = exchangeService;
        }

        public async Task<IEnumerable<long>> SyncBySymbol(string symbol)
        {
            var symbolInfo = await _symbolInfoQuery.FindByName(symbol);
            var lastEntity = await _tradeQuery.FindLastTradeBySymbol(symbolInfo.Symbol);

            var trades = await _exchangeService.GetTradesAsync(symbolInfo.Symbol, lastEntity?.TradeId);
            _logger.LogInformation($"{symbolInfo.Symbol} __ {trades.Count()} ___ {lastEntity?.TradeId}");

            List<long> createdIds = new();
            foreach (var item in ConvertToTrades(trades, symbolInfo.Id))
            {
                var exist = await _tradeQuery.FindByTradeId(item.TradeId);
                if (exist == null)
                {
                    var tradeGroup = await _tradeGroupQuery.FindLastGroupBySymbol(symbol);
                    if (tradeGroup == null)
                    {
                        tradeGroup = await _tradeGroupQuery.FindDefaultByBaseAsset(symbolInfo.BaseAsset);
                    }
                    item.T2TradeGroupId = tradeGroup.Id;
                    var created = await _tradeStore.Create(item);
                    createdIds.Add(created.Id);
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
