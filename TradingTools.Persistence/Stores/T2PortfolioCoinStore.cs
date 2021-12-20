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
    public class T2PortfolioCoinStore : IT2PortfolioCoinStore
    {
        private readonly TradingToolsDbContext _context;

        public T2PortfolioCoinStore(TradingToolsDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<T2PortfolioCoinEntity>> All()
        {
            return await _context.T2PortfolioCoins.ToListAsync();
        }

        public async Task<T2PortfolioCoinEntity> Create(T2PortfolioCoinEntity entity)
        {
            _context.T2PortfolioCoins.Add(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<IEnumerable<T2PortfolioCoinEntity>> FindBySyncVersion(long syncId)
        {
            return await _context.T2PortfolioCoins.Where(p => p.T2SyncId == syncId).ToListAsync();
        }

        public async Task<IEnumerable<T2PortfolioCoinEntity>> FindLastPortfolio()
        {
            var lastSyncId = await _context.T2PortfolioCoins.MaxAsync(m => m.T2SyncId);
            return await _context.T2PortfolioCoins.Where(p => p.T2SyncId == lastSyncId).ToListAsync();
        }

        public async Task<T2PortfolioCoinEntity> Update(T2PortfolioCoinEntity entity)
        {
            _context.T2PortfolioCoins.Update(entity);
            await _context.SaveChangesAsync();

            return entity;
        }
    }
}
