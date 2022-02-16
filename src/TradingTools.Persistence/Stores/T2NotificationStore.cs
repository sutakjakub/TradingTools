using Microsoft.EntityFrameworkCore;
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
    public class T2NotificationStore : IT2NotificationStore
    {
        private readonly TradingToolsDbContext _context;

        public T2NotificationStore(TradingToolsDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<T2NotificationEntity>> All()
        {
            return await _context.T2Notifications.ToListAsync();
        }

        public async Task<T2NotificationEntity> Create(T2NotificationEntity source)
        {
            _context.T2Notifications.Add(source);
            await _context.SaveChangesAsync();

            return source;
        }
    }
}
