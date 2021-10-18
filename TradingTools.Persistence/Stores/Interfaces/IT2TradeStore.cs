using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingTools.Db.Entities;

namespace TradingTools.Persistence.Stores.Interfaces
{
    public interface IT2TradeStore
    {
        Task<T2TradeEntity> Update(T2TradeEntity trade);
        Task<T2TradeEntity> Create(T2TradeEntity trade);
        Task MoveTo(IEnumerable<long> tradeIds, long tradeGroupId);
    }
}
