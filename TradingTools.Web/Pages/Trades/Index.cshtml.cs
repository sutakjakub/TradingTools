using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using TradingTools.Db.Entities;
using TradingTools.Persistence.Queries.Interfaces;

namespace TradingTools.Web.Pages.Trades
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IT2TradeQuery _tradeQuery;

        public IndexModel(ILogger<IndexModel> logger, IT2TradeQuery tradeQuery)
        {
            _logger = logger;
            _tradeQuery = tradeQuery ?? throw new ArgumentNullException(nameof(tradeQuery));
        }

        public IEnumerable<T2TradeEntity> Trades { get; private set; }
        public IEnumerable<T2TradeGroupEntity> TradeGroups { get; private set; }

        public async Task OnGet()
        {
            Trades = await _tradeQuery.All();
        }
    }
}
