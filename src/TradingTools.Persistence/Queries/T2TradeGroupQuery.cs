using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradingTools.Db;
using TradingTools.Db.Entities;
using TradingTools.Persistence.Queries.Interfaces;

namespace TradingTools.Persistence.Queries
{
    public class T2TradeGroupQuery : IT2TradeGroupQuery
    {
        private readonly TradingToolsDbContext _context;

        public T2TradeGroupQuery(TradingToolsDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<T2TradeGroupEntity>> All()
        {
            return await _context.T2TradeGroups
                .Include(group => group.SymbolInfo)
                .Include(group => group.Trades)
                .Include(group => group.Orders)
                .ThenInclude(trade => trade.T2SymbolInfo)
                .ToListAsync();
        }

        public async Task<T2TradeGroupEntity> Find(long id)
        {
            return await _context.T2TradeGroups
                .Include(i => i.SymbolInfo)
                .Include(i => i.Trades)
                .Include(i => i.Trades.OrderBy(o => o.TradeTime))
                .Include(group => group.Orders)
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<IEnumerable<T2TradeGroupEntity>> FindByBaseAsset(string baseAsset)
        {
            return await _context.T2TradeGroups
                .Where(p => p.BaseAsset == baseAsset)
                .Include(group => group.Trades)
                .Include(group => group.SymbolInfo)
                .Include(group => group.Orders)
                .ToListAsync();
        }

        public async Task<IEnumerable<T2TradeGroupEntity>> FindBySymbol(string symbol)
        {
            return await _context.T2TradeGroups
                .Include(group => group.Trades)
                .Include(group => group.SymbolInfo)
                .Include(group => group.Orders)
                .Where(p => p.SymbolInfo.Symbol == symbol)
                .ToListAsync();
        }

        public async Task<T2TradeGroupEntity> FindDefaultByBaseAsset(string baseAsset)
        {
            return await _context.T2TradeGroups
                .Include(group => group.Trades)
                .Include(group => group.SymbolInfo)
                .Include(group => group.Orders)
                .Where(p => p.BaseAsset == baseAsset && p.Name == $"Default_{baseAsset}")
                .FirstOrDefaultAsync();
        }

        public async Task<T2TradeGroupEntity> FindLastGroupBySymbol(string symbol)
        {
            return await _context.T2TradeGroups
                .Include(group => group.Trades)
                .Include(group => group.SymbolInfo)
                .Include(group => group.Orders)
                .OrderByDescending(o => o.Created)
                .FirstOrDefaultAsync(p => p.SymbolInfo.Symbol == symbol);
        }
    }
}
