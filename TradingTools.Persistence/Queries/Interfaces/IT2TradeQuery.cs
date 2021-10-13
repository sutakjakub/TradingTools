using System.Collections.Generic;
using System.Threading.Tasks;
using TradingTools.Db.Entities;

namespace TradingTools.Persistence.Queries.Interfaces
{
    public interface IT2TradeQuery
    {
        Task<IEnumerable<T2TradeEntity>> All();
        Task<T2TradeEntity> Find(long id);
        Task<IEnumerable<T2TradeEntity>> FindBySymbol(string symbol);
        Task<T2TradeEntity> FindLastTradeBySymbol(string symbol);
        Task<IEnumerable<T2TradeEntity>> FindWithoutSymbolInfo();
        Task<T2TradeEntity> FindByTradeId(long tradeId);
        IEnumerable<(T2SymbolInfoEntity symbol, List<T2TradeEntity> trades)> GroupBySymbol();
        IEnumerable<(string baseAsset, List<T2TradeEntity> trades)> GroupByBaseAsset();
        Task<IEnumerable<T2TradeEntity>> FindBySymbolOutsideTradeGroup(string symbol, long tradeGroupId);
    }
}