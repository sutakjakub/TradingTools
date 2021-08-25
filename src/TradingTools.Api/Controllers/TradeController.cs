using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradingTools.Db;
using TradingTools.ExchangeServices;
using TradingTools.ExchangeServices.Interfaces;

namespace TradingTools.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TradeController : ControllerBase
    {
        private readonly ILogger<TradeController> _logger;
        private readonly IBinanceExchangeService _binance;

        public TradeController(ILogger<TradeController> logger, IBinanceExchangeService binance)
        {
            _logger = logger;
            this._binance = binance ?? throw new ArgumentNullException(nameof(binance));
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
        public ActionResult<decimal> GetSummaryTrades(string symbol)
        {

            throw new NotImplementedException();

        }
    }
}
