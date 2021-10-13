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

namespace TradingTools.Web.Pages.TradeGroupDetail
{
    public class DetailModel : PageModel
    {
        private readonly ILogger<DetailModel> _logger;
        private readonly IServiceProvider _provider;
        private readonly IT2TradeGroupQuery _tradeGroupQuery;
        private readonly IT2TradeQuery _tradeQuery;
        private readonly IT2TradeStore _tradeStore;
        private readonly IT2Synchronizator _synchronizator;
        private readonly IBinanceExchangeService _binance;

        public DetailModel(
            ILogger<DetailModel> logger,
            IServiceProvider provider,
            IT2TradeGroupQuery tradeGroupQuery,
            IT2TradeQuery tradeQuery,
            IT2TradeStore tradeStore,
            IT2Synchronizator synchronizator,
            IBinanceExchangeService binance)
        {
            _logger = logger;
            _provider = provider;
            _tradeGroupQuery = tradeGroupQuery;
            _tradeQuery = tradeQuery;
            _tradeStore = tradeStore;
            _synchronizator = synchronizator;
            _binance = binance;
        }

        public ITradeGroupViewModel ViewModel { get; private set; }

        public IEnumerable<T2TradeEntity> OutsideTrades { get; private set; }

        [BindProperty]
        public List<long> AreChecked { get; set; } = new List<long>();


        public TradeSummaryModel TradeSummary { get; set; }

        public async Task OnGetAsync(long id)
        {
            var tradeGroup = await _tradeGroupQuery.Find(id);
            ViewModel = _provider.GetService<ITradeGroupViewModel>();
            await ViewModel.Init(tradeGroup);
            if (tradeGroup.SymbolInfo != null)
            {
                OutsideTrades = await _tradeQuery.FindBySymbolOutsideTradeGroup(tradeGroup.SymbolInfo.Symbol, id);
                var prices = await _binance.GetPricesAsync();
                TradeSummary = new TradeSummaryModel
                {
                    Symbol = tradeGroup.SymbolInfo.Symbol
                };
                TradeSummary.AveragePrice = AverageCost(tradeGroup.Trades);
                TradeSummary.Quantity = DiffQuantity(tradeGroup.Trades);
                TradeSummary.CurrentSymbolPrice = prices.Single(s => s.Symbol == TradeSummary.Symbol).Price;
                TradeSummary.GainPercentage = CurrentGain(TradeSummary.CurrentSymbolPrice, TradeSummary.AveragePrice);
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

        private static decimal AverageCost(IEnumerable<T2TradeEntity> source)
        {
            var buyEntries = source
                .Where(p => p.IsBuyer)
                .Select(c => new { c.Quantity, c.Price })
                .AsEnumerable()
                .Select(s => (quantity: s.Quantity, price: s.Price));

            var sellEntries = source
                 .Where(p => !p.IsBuyer)
                 .Select(c => new { c.Quantity, c.Price })
                 .AsEnumerable()
                 .Select(s => (quantity: s.Quantity, price: s.Price));

            return BasicCalculator.AverageCost(buyEntries, sellEntries, 8);
        }
        private static decimal CurrentGain(decimal currentPrice, decimal averagePrice)
        {
            if (averagePrice == 0)
            {
                return 0;
            }

            if (averagePrice <= currentPrice)
            {
                var increase = currentPrice - averagePrice;
                return increase / averagePrice;
            }
            else
            {
                var decrease = averagePrice - currentPrice;
                return (decrease / averagePrice) * -1;
            }
        }
        private static decimal DiffQuantity(IEnumerable<T2TradeEntity> items)
        {
            return items.Where(p => p.IsBuyer).Sum(s => s.Quantity) - items.Where(p => !p.IsBuyer).Sum(s => s.Quantity);
        }
        public class TradeSummaryModel
        {
            public string Symbol { get; set; }
            public decimal Quantity { get; set; }
            public decimal AveragePrice { get; set; }
            public decimal GainPercentage { get; set; }
            public decimal CurrentSymbolPrice { get; set; }
            public decimal CurrentValue
            {
                get
                {
                    return Quantity * CurrentSymbolPrice;
                }
            }
            public decimal DollarPrice { get; set; }

            //public IEnumerable<T2TradeDto> Trades { get; set; }
        }
    }
}
