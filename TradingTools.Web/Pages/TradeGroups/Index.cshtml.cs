using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using TradingTools.Db.Entities;
using TradingTools.Persistence.Queries.Interfaces;

namespace TradingTools.Web.Pages.TradeGroups
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IT2TradeGroupQuery _tradeGroupQuery;

        public IndexModel(ILogger<IndexModel> logger, IT2TradeGroupQuery tradeGroupQuery)
        {
            _logger = logger;
            _tradeGroupQuery = tradeGroupQuery ?? throw new ArgumentNullException(nameof(tradeGroupQuery));
        }

        public IEnumerable<T2TradeGroupEntity> TradeGroups { get; private set; }

        public async Task OnGet()
        {
            TradeGroups = await _tradeGroupQuery.All();
        }
    }
}
