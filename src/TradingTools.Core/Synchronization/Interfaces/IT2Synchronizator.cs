using System.Collections.Generic;
using System.Threading.Tasks;
using TradingTools.Shared.Dto;

namespace TradingTools.Core.Synchronization.Interfaces
{
    public interface IT2Synchronizator
    {
        Task<IEnumerable<long>> SyncBySymbol(string symbol);
        Task<IEnumerable<long>> SyncByBaseAsset(string baseAsset);
        Task SyncOpenOrdersBySymbol(string symbol = null, IEnumerable<T2OrderDto> openOrders = null);
        Task SyncByPortfolio();

        Task SyncPortfolio();
    }
}