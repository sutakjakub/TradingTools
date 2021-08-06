using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradingTools.MathLib;

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
        /// <param name="riskPerc">Percentage risk per trade.</param>
        /// <returns></returns>
        [HttpGet("/calcSimplePositionSize")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<decimal> GetSimplePositionSize(decimal equity, decimal riskPerc)
        {
            return Ok(BasicCalculator.PositionSizeByRisk(equity, riskPerc));
        }

        /// <summary>
        /// Returns calculated stoploss of trade in percentage.
        /// </summary>
        /// <param name="entryPrice">Entry price of trade.</param>
        /// <param name="stopLossPrice">Stoploss price of trade.</param>
        /// <returns></returns>
        [HttpGet("/calcPositionSizePerc")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<decimal> CalculateStopLossPercentage(decimal entryPrice, decimal stopLossPrice)
        {
            return Ok(BasicCalculator.StopLossPercentage(entryPrice, stopLossPrice));
        }

        /// <summary>
        /// Returns calculated postition size.
        /// </summary>
        /// <param name="entryPrice">Entry price of trade.</param>
        /// <param name="stopLossPrice">Stoploss price of trade.</param>
        /// <param name="riskPerc">Percentage risk per trade.</param>
        /// <param name="equity">The total capital size.</param>
        /// <returns></returns>
        [HttpGet("/calcPositionSize")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<decimal> CalculatePositionSize(decimal entryPrice, decimal stopLossPrice, decimal riskPerc, decimal equity)
        {
            return Ok(BasicCalculator.PositionSize(entryPrice, stopLossPrice, riskPerc, equity));
        }

        [HttpGet("/calcLadderingTradingPos")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<decimal> CalculateLadderingTradingPosition(decimal positionSize)
        {
            throw new NotImplementedException();
        }
    }
}
