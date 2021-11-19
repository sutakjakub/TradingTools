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
    public class T2SyncStore : IT2SyncStore
    {
        private readonly TradingToolsDbContext _context;

        public T2SyncStore(TradingToolsDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<T2SyncEntity> Create(T2SyncEntity entity)
        {
            _context.T2Syncs.Add(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<T2SyncEntity> Update(T2SyncEntity entity)
        {
            _context.T2Syncs.Update(entity);
            await _context.SaveChangesAsync();

            return entity;
        }
    }
}
