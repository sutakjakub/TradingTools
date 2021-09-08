using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradingTools.Db;
using TradingTools.Db.Entities;
using TradingTools.ExchangeServices;
using TradingTools.ExchangeServices.Interfaces;
using TradingTools.MathLib;
using TradingTools.Shared.Dto;

namespace TradingTools.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TradeController : ControllerBase
    {
        private readonly ILogger<TradeController> _logger;
        private readonly IBinanceExchangeService _binance;
        private readonly TradingToolsDbContext _db;

        public TradeController(ILogger<TradeController> logger, IBinanceExchangeService binance, TradingToolsDbContext db)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this._binance = binance ?? throw new ArgumentNullException(nameof(binance));
            this._db = db ?? throw new ArgumentNullException(nameof(db));
        }

        /// <summary>
        /// Returns position size for trade.
        /// </summary>
        /// <param name="equity">The total capital size.</param>
        /// <param name="riskPerc">Percentage risk per trade.</param>
        /// <returns></returns>
        [HttpGet("/createTrade")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<decimal> GetSimplePositionSize()
        {
            throw new NotImplementedException();
        }


        [HttpGet("/GetSummaryTrades")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<TradeSummaryModel>>> GetSummaryTrades(string symbol)
        {
            var tradeGroups = _db.T2Trades.AsEnumerable().GroupBy(
                p => p.Symbol,
                p => p,
                (key, g) => new { Symbol = key, Items = g.ToList() }).ToList();
            var result = new List<TradeSummaryModel>();
            var prices = await _binance.GetPricesAsync();
            foreach (var group in tradeGroups)
            {
                var trade = new TradeSummaryModel
                {
                    Symbol = group.Symbol,
                };
                trade.AveragePrice = AverageCost(group.Items);
                trade.Quantity = DiffQuantity(group.Items);
                trade.CurrentSymbolPrice = prices.Single(s => s.Symbol == group.Symbol).Price;
                trade.GainPercentage = CurrentGain(trade.CurrentSymbolPrice, trade.AveragePrice);
                result.Add(trade);
            }

            foreach (var item in result)
            {

            }

            return Ok(result);
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

            public IEnumerable<T2TradeDto> Trades { get; set; }
        }

        public enum TradeSummaryState
        {
            /// <summary>
            /// Balance quantity in pair is negative. 
            /// Some inconsistent occured.
            /// </summary>
            InconsistTradePair,
            Ok
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
    }
}
