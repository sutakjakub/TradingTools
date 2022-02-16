using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingTools.Db.Entities;

namespace TradingTools.Persistence.Queries.Interfaces
{
    public interface IT2OrderQuery
    {
        Task<IEnumerable<T2OrderEntity>> All();
        Task<T2OrderEntity> Find(long id);
        Task<T2OrderEntity> FindByOrderId(long orderId);
        Task<IEnumerable<T2OrderEntity>> FindBySymbol(string symbol);
    }
}
