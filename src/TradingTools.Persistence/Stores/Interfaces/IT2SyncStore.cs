using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingTools.Db.Entities;

namespace TradingTools.Persistence.Stores.Interfaces
{
    public interface IT2SyncStore
    {
        Task<T2SyncEntity> Create(T2SyncEntity entity);
        Task<T2SyncEntity> Update(T2SyncEntity entity);
    }
}
