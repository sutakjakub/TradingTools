using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using TradingTools.Db.Entities;
using TradingTools.ExchangeServices.Interfaces;
using TradingTools.MathLib;
using TradingTools.Persistence.Queries.Interfaces;
using TradingTools.Persistence.Stores.Interfaces;
using TradingTools.Web.Business;

namespace TradingTools.Web.Pages.TradeGroupDetail
{
    public class DetailModel : PageModel
    {
        private readonly ILogger<DetailModel> _logger;
        private readonly IT2TradeGroupQuery _tradeGroupQuery;
        private readonly IT2TradeQuery _tradeQuery;
        private readonly IT2TradeStore _tradeStore;
        private readonly IBinanceExchangeService _binance;

        public DetailModel(ILogger<DetailModel> logger, IT2TradeGroupQuery tradeGroupQuery, IT2TradeQuery tradeQuery, IT2TradeStore tradeStore, IBinanceExchangeService binance)
        {
            _logger = logger;
            _tradeGroupQuery = tradeGroupQuery;
            _tradeQuery = tradeQuery;
            _tradeStore = tradeStore;
            _binance = binance;
        }

        public T2TradeGroupEntity TradeGroup { get; private set; }
        public TradeGroupBusinessModel Business { get; private set; }

        public IEnumerable<T2TradeEntity> Trades { get; private set; }

        [BindProperty]
        public List<long> AreChecked { get; set; } = new List<long>();


        public TradeSummaryModel TradeSummary { get; set; }

        public async Task OnGetAsync(long id)
        {
            TradeGroup = await _tradeGroupQuery.Find(id);
            Trades = await _tradeQuery.FindBySymbolOutsideTradeGroup(TradeGroup.SymbolInfo.Symbol, id);
            Business = new TradeGroupBusinessModel();
            
            var prices = await _binance.GetPricesAsync();

            TradeSummary = new TradeSummaryModel
            {
                Symbol = TradeGroup.SymbolInfo.Symbol
            };
            TradeSummary.AveragePrice = AverageCost(TradeGroup.Trades);
            TradeSummary.Quantity = DiffQuantity(TradeGroup.Trades);
            TradeSummary.CurrentSymbolPrice = prices.Single(s => s.Symbol == TradeSummary.Symbol).Price;
            TradeSummary.GainPercentage = CurrentGain(TradeSummary.CurrentSymbolPrice, TradeSummary.AveragePrice);
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
