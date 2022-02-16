using System.Collections.Generic;
using System.Threading.Tasks;
using TradingTools.Db.Entities;

namespace TradingTools.Persistence.Queries.Interfaces
{
    public interface IT2SymbolInfoQuery
    {
        Task<IEnumerable<T2SymbolInfoEntity>> All();
        Task<T2SymbolInfoEntity> Find(long id);
        Task<T2SymbolInfoEntity> FindByName(string symbol);
    }
}