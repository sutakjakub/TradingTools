using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TradingTools.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToolsController : ControllerBase
    {
        private readonly ILogger<ToolsController> _logger;

        public ToolsController(ILogger<ToolsController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Returns position size for trade.
        /// </summary>
        /// <param name="equity">The total capital size.</param>
        /// <param name="maxRiskPerTrade">Percentage risk per trade.</param>
        /// <returns></returns>
        [HttpGet("/calcSimplePositionSize")]
        public ActionResult<decimal> GetSimplePositionSize(decimal equity, decimal riskPerTrade)
        {
            if (equity <= 0)
            {
                _logger.LogWarning($"Equity is less or equal zero.");
                return BadRequest("Equity is less or equal zero.");
            }
            if (riskPerTrade < 0 || riskPerTrade > 1)
            {
                _logger.LogWarning($"Risk per trade is in percentage. Acceptable value is between zero and one.");
                return BadRequest("Risk per trade is in percentage. Acceptable value is between zero and one.");
            }

            return Ok(decimal.Round(equity * riskPerTrade, 8));
        }

        /// <summary>
        /// Returns calculated stoploss of trade in percentage.
        /// </summary>
        /// <param name="entryPrice">Entry price of trade.</param>
        /// <param name="stopLossPrice">Stoploss price of trade.</param>
        /// <returns></returns>
        [HttpGet("/calcPositionSizePerc")]
        public ActionResult<decimal> CalculateStopLossPercentage(decimal entryPrice, decimal stopLossPrice)
        {
            if (entryPrice <= 0)
            {
                _logger.LogWarning($"Entry price is less or equal zero.");
                return BadRequest("Entry price is less or equal zero.");
            }
            if (stopLossPrice < 0 || stopLossPrice == entryPrice)
            {
                _logger.LogWarning($"Stoploss price is except greater than zero and must not be equal to the entry price.");
                return BadRequest("Stoploss price is except greater than zero and must not be equal to the entry price.");
            }

            var result = Math.Abs((100 - (stopLossPrice * 100) / entryPrice) / 100);
            result = decimal.Round(result, 4);

            return Ok(result);
        }

        /// <summary>
        /// Returns calculated risk of trade in percentage.
        /// </summary>
        /// <param name="equity">The total capital size.</param>
        /// <param name="riskPerTrade">Percentage risk per trade.</param>
        /// <returns></returns>
        [HttpGet("/calcRiskPerc")]
        public ActionResult<decimal> CalculateRiskPercentage(decimal equity, decimal riskPerTrade)
        {
            if (equity <= 0)
            {
                _logger.LogWarning($"Equity is less or equal zero.");
                return BadRequest("Equity is less or equal zero.");
            }
            if (riskPerTrade < 0 || riskPerTrade > 1)
            {
                _logger.LogWarning($"Risk per trade is in percentage. Acceptable value is between zero and one.");
                return BadRequest("Risk per trade is in percentage. Acceptable value is between zero and one.");
            }

            var result = decimal.Round(equity * riskPerTrade, 4);
            return Ok(result);
        }

        /// <summary>
        /// Returns calculated postition size.
        /// </summary>
        /// <param name="entryPrice">Entry price of trade.</param>
        /// <param name="stopLossPrice">Stoploss price of trade.</param>
        /// <param name="riskPerTrade">Percentage risk per trade.</param>
        /// <param name="equity">The total capital size.</param>
        /// <returns></returns>
        [HttpGet("/calcPositionSize")]
        public ActionResult<decimal> CalculatePositionSize(decimal entryPrice, decimal stopLossPrice, decimal riskPerTrade, decimal equity)
        {
            if (equity <= 0)
            {
                _logger.LogWarning($"Equity is less or equal zero.");
                return BadRequest("Equity is less or equal zero.");
            }
            if (riskPerTrade < 0 || riskPerTrade > 1)
            {
                _logger.LogWarning($"Risk per trade is in percentage. Acceptable value is between zero and one.");
                return BadRequest("Risk per trade is in percentage. Acceptable value is between zero and one.");
            }
            if (equity <= 0)
            {
                _logger.LogWarning($"Equity is less or equal zero.");
                return BadRequest("Equity is less or equal zero.");
            }
            if (riskPerTrade < 0 || riskPerTrade > 1)
            {
                _logger.LogWarning($"Risk per trade is in percentage. Acceptable value is between zero and one.");
                return BadRequest("Risk per trade is in percentage. Acceptable value is between zero and one.");
            }

            var riskPerc = decimal.Round(equity * riskPerTrade);
            var stoplossPerc = Math.Abs((100 - (stopLossPrice * 100) / entryPrice) / 100);

            var result = Math.Abs(riskPerc / stoplossPerc);
            result = decimal.Round(result, 8);
            return Ok(result);
        }
    }
}
