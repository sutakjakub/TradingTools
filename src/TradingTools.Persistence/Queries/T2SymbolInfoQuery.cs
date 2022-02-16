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
    public class T2SymbolInfoQuery : IT2SymbolInfoQuery
    {
        private readonly TradingToolsDbContext _context;

        public T2SymbolInfoQuery(TradingToolsDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<T2SymbolInfoEntity>> All()
        {
            return await _context.T2SymbolInfos.ToListAsync();
        }

        public async Task<T2SymbolInfoEntity> Find(long id)
        {
            return await _context.T2SymbolInfos
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<T2SymbolInfoEntity> FindByName(string symbol)
        {
            return await _context.T2SymbolInfos
                .FirstOrDefaultAsync(f => f.Symbol == symbol);
        }
    }
}
