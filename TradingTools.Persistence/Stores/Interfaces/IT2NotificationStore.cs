using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingTools.Db.Entities;

namespace TradingTools.Persistence.Stores.Interfaces
{
    public interface IT2NotificationStore
    {
        Task<T2NotificationEntity> Create(T2NotificationEntity source);
        Task<IEnumerable<T2NotificationEntity>> All();
    }
}
