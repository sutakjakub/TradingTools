using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingTools.Db.Entities;

namespace TradingTools.Persistence.Stores.Interfaces
{
    public interface IT2PortfolioCoinStore
    {
        Task<T2PortfolioCoinEntity> Create(T2PortfolioCoinEntity entity);
        Task<T2PortfolioCoinEntity> Update(T2PortfolioCoinEntity entity);
        Task<IEnumerable<T2PortfolioCoinEntity>> All();
        Task<IEnumerable<T2PortfolioCoinEntity>> FindLastPortfolio();
        Task<int> GetLastVersion();
    }
}
