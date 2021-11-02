using System.Threading.Tasks;
using TradingTools.Db.Entities;

namespace TradingTools.Persistence.Stores.Interfaces
{
    public interface IT2TradeGroupStore
    {
        Task<T2TradeGroupEntity> Create(T2TradeGroupEntity tradeGroup);
        Task<T2TradeGroupEntity> Update(T2TradeGroupEntity tradeGroup);
    }
}