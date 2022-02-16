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
    public class T2OrderStore : IT2OrderStore
    {
        private readonly TradingToolsDbContext _context;

        public T2OrderStore(TradingToolsDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<T2OrderEntity> Create(T2OrderEntity source)
        {
            _context.T2Orders.Add(source);
            await _context.SaveChangesAsync();

            return source;
        }

        public async Task<bool> Delete(params long[] list)
        {
            var allByList = _context.T2Orders.Where(p => list.Contains(p.Id));
            _context.T2Orders.RemoveRange(allByList);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<T2OrderEntity> Update(T2OrderEntity source)
        {
            _context.T2Orders.Update(source);
            await _context.SaveChangesAsync();

            return source;
        }
    }
}
