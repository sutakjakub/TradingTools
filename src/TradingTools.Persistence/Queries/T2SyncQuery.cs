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
    public class T2SyncQuery : IT2SyncQuery
    {
        private readonly TradingToolsDbContext _context;
        public T2SyncQuery(TradingToolsDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<T2SyncEntity>> All()
        {
            return await _context.T2Syncs.ToListAsync();
        }

        public async Task<T2SyncEntity> Find(long id)
        {
            return await _context.T2Syncs
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<T2SyncEntity> FindLast()
        {
            return await _context.T2Syncs.OrderByDescending(o => o.Created)
                .FirstOrDefaultAsync();
        }
    }
}
