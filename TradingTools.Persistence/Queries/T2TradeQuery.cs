using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingTools.Db;
using TradingTools.Db.Entities;
using TradingTools.Persistence.Queries.Interface;

namespace TradingTools.Persistence.Queries
{
    public class T2TradeQuery : IT2TradeQuery
    {
        private readonly TradingToolsDbContext _context;

        public T2TradeQuery(TradingToolsDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<T2TradeEntity>> FindWithoutSymbolInfo()
        {
            return await _context.T2Trades
                .Where(w => w.T2SymbolInfoId == null)
                .ToListAsync();
        }

        public async Task<IEnumerable<T2TradeEntity>> FindBySymbol(string symbol)
        {
            return await _context.T2Trades
                .Where(w => w.Symbol == symbol)
                .Include(trade => trade.T2SymbolInfo)
                .ToListAsync();
        }

        public async Task<T2TradeEntity> FindLastTradeBySymbol(string symbol)
        {
            return await _context.T2Trades
                .Where(w => w.Symbol == symbol)
                .Include(trade => trade.T2SymbolInfo)
                .OrderByDescending(o => o.TradeId)
                .FirstOrDefaultAsync();
        }

        public async Task<T2TradeEntity> Find(long id)
        {
            return await _context.T2Trades
                .Include(trade => trade.T2SymbolInfo)
                .FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task<IEnumerable<T2TradeEntity>> All()
        {
            return await _context.T2Trades
                .Include(trade => trade.T2SymbolInfo)
                .ToListAsync();
        }

        public IEnumerable<(string symbol, List<T2TradeEntity> trades)> GroupBySymbol()
        {
            var pp =  _context.T2Trades.AsEnumerable().GroupBy(
                p => p.Symbol,
                p => p,
                (key, g) => (symbol: key, trades: g.ToList())).ToList();

            return pp;
        }
    }
}
