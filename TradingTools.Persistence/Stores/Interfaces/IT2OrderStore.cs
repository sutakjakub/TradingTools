using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingTools.Db.Entities;

namespace TradingTools.Persistence.Stores.Interfaces
{
    public interface IT2OrderStore
    {
        Task<T2OrderEntity> Create(T2OrderEntity source);
        Task<T2OrderEntity> Update(T2OrderEntity source);
        Task<bool> Delete(params long[] list);
    }
}
