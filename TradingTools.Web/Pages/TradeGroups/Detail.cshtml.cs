using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using TradingTools.Core.Synchronization.Interfaces;
using TradingTools.Db.Entities;
using TradingTools.ExchangeServices.Interfaces;
using TradingTools.MathLib;
using TradingTools.Persistence.Queries.Interfaces;
using TradingTools.Persistence.Stores.Interfaces;
using TradingTools.Web.ViewModels.Interfaces;
using TradingTools.Db.Enums;

namespace TradingTools.Web.Pages.TradeGroupDetail
{
    public class DetailModel : PageModel
    {
        private readonly ILogger<DetailModel> _logger;
        private readonly IServiceProvider _provider;
        private readonly IT2TradeGroupQuery _tradeGroupQuery;
        private readonly IT2TradeGroupStore _tradeGroupStore;
        private readonly IT2TradeQuery _tradeQuery;
        private readonly IT2TradeStore _tradeStore;
        private readonly IT2Synchronizator _synchronizator;
        private readonly IBinanceExchangeService _binance;

        public DetailModel(
            ILogger<DetailModel> logger,
            IServiceProvider provider,
            IT2TradeGroupQuery tradeGroupQuery,
            IT2TradeGroupStore tradeGroupStore,
            IT2TradeQuery tradeQuery,
            IT2TradeStore tradeStore,
            IT2Synchronizator synchronizator,
            IBinanceExchangeService binance)
        {
            _logger = logger;
            _provider = provider;
            _tradeGroupQuery = tradeGroupQuery;
            this._tradeGroupStore = tradeGroupStore;
            _tradeQuery = tradeQuery;
            _tradeStore = tradeStore;
            _synchronizator = synchronizator;
            _binance = binance;
        }

        public ITradeGroupViewModel ViewModel { get; private set; }

        public IEnumerable<T2TradeEntity> OutsideTrades { get; private set; }

        [BindProperty]
        public List<long> AreChecked { get; set; } = new List<long>();

        [BindProperty]
        public List<long> AreCheckedForRemove { get; set; } = new List<long>();

        public async Task OnGetAsync(long id)
        {
            var tradeGroup = await _tradeGroupQuery.Find(id);
            ViewModel = _provider.GetService<ITradeGroupViewModel>();
            ViewModel.Init(tradeGroup);

            if (tradeGroup.SymbolInfo != null)
            {
                var outsideTrades = await _tradeQuery.FindBySymbolOutsideTradeGroup(tradeGroup.SymbolInfo.Symbol, id);
                OutsideTrades = outsideTrades.OrderBy(o => o.TradeTime);
                var price = await _binance.GetPriceAsync(tradeGroup.SymbolInfo.Symbol);
                ViewModel.CurrentPrice = price;
            }
        }

        public async Task<IActionResult> OnPostAssign(long id)
        {
            foreach (var item in AreChecked)
            {
                var trade = await _tradeQuery.Find(item);
                trade.T2TradeGroupId = id;
                await _tradeStore.Update(trade);
            }
            return RedirectToPage("Detail", new { id });
        }

        public async Task<IActionResult> OnPostRemove(long groupId)
        {
            var tradeGroup = await _tradeGroupQuery.Find(groupId);
            var defaultTradeGroup = await _tradeGroupQuery.FindDefaultByBaseAsset(tradeGroup.SymbolInfo.BaseAsset);
            await _tradeStore.MoveTo(AreCheckedForRemove, defaultTradeGroup.Id);

            return RedirectToPage("Detail", new { groupId });
        }

        public async Task<IActionResult> OnPostSetTradeState(long groupId, TradeState state)
        {
            await _tradeStore.SetState(AreCheckedForRemove, state);
            return RedirectToPage("Detail", new { groupId });
        }

        public async Task<IActionResult> OnPostSetTradeGroupState(long groupId, TradeGroupState state)
        {
            var tradeGroup = await _tradeGroupQuery.Find(groupId);
            tradeGroup.TradeGroupState = state;
            await _tradeGroupStore.Update(tradeGroup);

            TradeState tradeState = state == TradeGroupState.InProgress ? TradeState.Pending : TradeState.Resolved;
            await _tradeStore.SetState(tradeGroup.Trades.Select(s => s.Id), tradeState);

            return RedirectToPage("Detail", new { groupId });
        }

        public async Task<IActionResult> OnPostSync(long groupId, string symbol, string baseAsset)
        {
            if (!string.IsNullOrWhiteSpace(symbol))
            {
                _ = await _synchronizator.SyncBySymbol(symbol);
            }
            else
            {
                _ = await _synchronizator.SyncByBaseAsset(baseAsset);
            }

            return RedirectToPage("Detail", new { id = groupId });
        }
    }
}
