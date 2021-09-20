using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using TradingTools.Db.Entities;
using TradingTools.Persistence.Queries.Interfaces;

namespace TradingTools.Web.Pages
{
    public class TradeGroupDetailModel : PageModel
    {
        private readonly ILogger<TradeGroupDetailModel> _logger;
        private readonly IT2TradeGroupQuery _tradeGroupQuery;

        public TradeGroupDetailModel(ILogger<TradeGroupDetailModel> logger, IT2TradeGroupQuery tradeGroupQuery)
        {
            _logger = logger;
            _tradeGroupQuery = tradeGroupQuery;
        }

        public T2TradeGroupEntity TradeGroup { get; private set; }

        public async Task OnGet(long id)
        {
            TradeGroup = await _tradeGroupQuery.Find(id);
        }
    }
}
