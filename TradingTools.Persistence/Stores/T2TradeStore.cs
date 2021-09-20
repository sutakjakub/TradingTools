using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingTools.Db;
using TradingTools.Db.Entities;

namespace TradingTools.Persistence.Stores
{
    public class T2TradeStore 
    {
        private readonly TradingToolsDbContext _context;

        public T2TradeStore(TradingToolsDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<T2TradeEntity> Update(T2TradeEntity trade)
        {
            _context.T2Trades.Update(trade);
            await _context.SaveChangesAsync();

            return trade;
        }

        public async Task<T2TradeEntity> Create(T2TradeEntity trade)
        {
            _context.T2Trades.Add(trade);
            await _context.SaveChangesAsync();

            return trade;
        }
    }
}
