using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingTools.Db;
using TradingTools.Db.Entities;
using TradingTools.Persistence.Stores.Interfaces;

namespace TradingTools.Persistence.Stores
{
    public class T2TradeGroupStore : IT2TradeGroupStore
    {
        private readonly TradingToolsDbContext _context;
        public T2TradeGroupStore(TradingToolsDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<T2TradeGroupEntity> Create(T2TradeGroupEntity tradeGroup)
        {
            _context.T2TradeGroups.Add(tradeGroup);
            await _context.SaveChangesAsync();

            return tradeGroup;
        }
    }
}
