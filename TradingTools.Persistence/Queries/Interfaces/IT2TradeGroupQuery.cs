using System.Collections.Generic;
using System.Threading.Tasks;
using TradingTools.Db.Entities;

namespace TradingTools.Persistence.Queries.Interfaces
{
    public interface IT2TradeGroupQuery
    {
        Task<T2TradeGroupEntity> Find(long id);
        Task<IEnumerable<T2TradeGroupEntity>> All();

        Task<IEnumerable<T2TradeGroupEntity>> FindByBaseAsset(string baseAsset);

        Task<IEnumerable<T2TradeGroupEntity>> FindBySymbol(string symbol);
    }
}