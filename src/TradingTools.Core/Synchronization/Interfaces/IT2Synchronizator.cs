using System.Collections.Generic;
using System.Threading.Tasks;

namespace TradingTools.Core.Synchronization.Interfaces
{
    public interface IT2Synchronizator
    {
        Task<IEnumerable<long>> SyncBySymbol(string symbol);
        Task<IEnumerable<long>> SyncByBaseAsset(string baseAsset);
    }
}