using System.Collections.Generic;
using System.Threading.Tasks;
using TradingTools.Db.Entities;

namespace TradingTools.Persistence.Queries.Interface
{
    public interface IT2TradeQuery
    {
        Task<IEnumerable<T2TradeEntity>> All();
        Task<T2TradeEntity> Find(long id);
        Task<IEnumerable<T2TradeEntity>> FindBySymbol(string symbol);
        Task<T2TradeEntity> FindLastTradeBySymbol(string symbol);
        Task<IEnumerable<T2TradeEntity>> FindWithoutSymbolInfo();
        IEnumerable<(string symbol, List<T2TradeEntity> trades)> GroupBySymbol();
    }
}