using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TradingTools.Shared.Dto;

namespace TradingTools.ExchangeServices.Interfaces
{
    public interface IExchangeService
    {
        Task<T2OrderDto> GetOrderAsync(string symbol, long orderId);
        Task<IEnumerable<T2SymbolInfoDto>> GetAllSymbolsAsync();
        Task<IEnumerable<T2TradeDto>> GetTradesAsync(string symbol, long? fromId = null);
        Task<decimal> GetPriceAsync(string symbol);
        Task<IEnumerable<ExchangePriceDto>> GetPricesAsync();
        Task<IEnumerable<BinanceUserCoinDto>> GetUserCoinsAsync();
        Task<IEnumerable<T2OrderDto>> GetOpenOrders(string symbol = null);
    }
}
