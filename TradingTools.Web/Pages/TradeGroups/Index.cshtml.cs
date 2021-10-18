using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TradingTools.Db.Entities;
using TradingTools.Persistence.Queries.Interfaces;
using TradingTools.Web.ViewModels;
using TradingTools.Web.ViewModels.Interfaces;

namespace TradingTools.Web.Pages.TradeGroups
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IServiceProvider _provider;
        private readonly IT2TradeGroupQuery _tradeGroupQuery;

        public IndexModel(ILogger<IndexModel> logger, IServiceProvider provider, IT2TradeGroupQuery tradeGroupQuery)
        {
            _provider = provider;
            _logger = logger;
            _tradeGroupQuery = tradeGroupQuery ?? throw new ArgumentNullException(nameof(tradeGroupQuery));
        }

        public IList<ITradeGroupViewModel> TradeGroups { get; private set; }

        public async Task OnGet()
        {
            var all = await _tradeGroupQuery.All();

            List<ITradeGroupViewModel> list = new();
            foreach (var item in all)
            {
                var viewModel = _provider.GetService<ITradeGroupViewModel>();
                viewModel.Init(item);

                list.Add(viewModel);
            }

            TradeGroups = list.OrderBy(o => o.TradeGroup.IsDefault).ThenByDescending(o => o.TradeGroup.Created).ToList();
        }
    }
}
