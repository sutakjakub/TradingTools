using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingTools.Db.Entities;

namespace TradingTools.Persistence.Queries.Interfaces
{
    public interface IT2SyncQuery
    {
        Task<IEnumerable<T2SyncEntity>> All();
        Task<T2SyncEntity> Find(long id);
        Task<T2SyncEntity> FindLast();
    }
}
