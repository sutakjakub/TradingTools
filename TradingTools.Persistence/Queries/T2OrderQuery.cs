using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingTools.Db;
using TradingTools.Db.Entities;
using TradingTools.Persistence.Queries.Interfaces;

namespace TradingTools.Persistence.Queries
{
    public class T2OrderQuery : IT2OrderQuery
    {
        private readonly TradingToolsDbContext _context;

        public T2OrderQuery(TradingToolsDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<T2OrderEntity>> All()
        {
            return await _context.T2Orders.ToListAsync();
        }

        public async Task<T2OrderEntity> Find(long id)
        {
            return await _context.T2Orders
              .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<T2OrderEntity> FindByOrderId(long orderId)
        {
            return await _context.T2Orders
               .FirstOrDefaultAsync(f => f.OrderId == orderId);
        }

        public async Task<IEnumerable<T2OrderEntity>> FindBySymbol(string symbol)
        {
            return await _context.T2Orders
               .Where(f => f.Symbol == symbol).ToListAsync();
        }
    }
}
