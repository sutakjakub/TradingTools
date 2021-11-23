using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TradingTools.Db.Entities;
using TradingTools.ExchangeServices.Interfaces;
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
        private readonly IBinanceExchangeService _binance;

        public IndexModel(ILogger<IndexModel> logger, IServiceProvider provider, IT2TradeGroupQuery tradeGroupQuery, IBinanceExchangeService binance)
        {
            _provider = provider;
            _logger = logger;
            _tradeGroupQuery = tradeGroupQuery ?? throw new ArgumentNullException(nameof(tradeGroupQuery));
            _binance = binance;
        }

        public IList<ITradeGroupViewModel> TradeGroups { get; private set; }

        public async Task OnGet()
        {
            var all = await _tradeGroupQuery.All();

            List<ITradeGroupViewModel> list = new();
            var prices = await _binance.GetPricesAsync();

            foreach (var item in all)
            {
                var viewModel = _provider.GetService<ITradeGroupViewModel>();
                viewModel.Init(item);

                if (item.SymbolInfo != null)
                {
                    var priceDto = prices.FirstOrDefault(f => f.Symbol == item.SymbolInfo.Symbol);
                    if (priceDto != null)
                    {
                        viewModel.CurrentPrice = priceDto.Price;
                    }
                }
                list.Add(viewModel);
            }

            TradeGroups = list
                .OrderBy(o => o.TradeGroup.IsDefault)
                .ThenByDescending(o => o.TradeGroup.TradeGroupState != Db.Enums.TradeGroupState.Done)
                .ThenByDescending(o => o.TradeGroup.Created).ToList();
        }

        public async Task<IActionResult> OnGetDownloadTradingViewList()
        {
            var groups = await _tradeGroupQuery.All();
            var filtered = groups.Where(p => !p.IsDefault && p.TradeGroupState == Db.Enums.TradeGroupState.InProgress).ToList();
            StringBuilder sb = new();
            var count = filtered.Count;
            for (int i = 0; i < count; i++)
            {
                sb.Append($"BINANCE:{filtered[i].SymbolInfo.Symbol}");
                if (i < count - 1)
                {
                    sb.Append(',');
                }
            }

            // convert string to stream
            byte[] byteArray = Encoding.ASCII.GetBytes(sb.ToString());
            MemoryStream stream = new(byteArray);

            return File(stream, "application/octet-stream"); // returns a FileStreamResult
        }
    }
}
