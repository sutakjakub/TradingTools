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
                .Include(group => group.Trades)
                .Include(group => group.SymbolInfo)
                .ToListAsync();
        }

        public async Task<T2TradeGroupEntity> Find(long id)
        {
            return await _context.T2TradeGroups
                .Include(i => i.SymbolInfo)
                .Include(i => i.Trades.OrderBy(o => o.TradeTime))
                .ThenInclude(trade => trade.T2SymbolInfo)
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<IEnumerable<T2TradeGroupEntity>> FindByBaseAsset(string baseAsset)
        {
            return await _context.T2TradeGroups
                .Where(p => p.BaseAsset == baseAsset)
                .Include(group => group.Trades)
                .Include(group => group.SymbolInfo)
                .ToListAsync();
        }

        public async Task<IEnumerable<T2TradeGroupEntity>> FindBySymbol(string symbol)
        {
            return await _context.T2TradeGroups
                .Include(group => group.Trades)
                .Include(group => group.SymbolInfo)
                .Where(p => p.SymbolInfo.Symbol == symbol)
                .ToListAsync();
        }
    }
}
