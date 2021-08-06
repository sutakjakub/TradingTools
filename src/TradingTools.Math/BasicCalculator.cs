using System;

namespace TradingTools.MathLib
{
    public static class BasicCalculator
    {
        /// <summary>
        /// Returns position size for trade by risk.
        /// When <paramref name="decimalPlaces"/> is equal negative one then rounding will not occur.
        /// </summary>
        /// <param name="equity">The total capital size.</param>
        /// <param name="riskPerc">Percentage risk per trade.</param>
        /// <param name="decimalPlaces">How many decimal points should the result have.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static decimal PositionSizeByRisk(decimal equity, decimal riskPerc, int decimalPlaces = 8)
        {
            if (equity <= 0)
            {
                throw new ArgumentException($"Equity from input is {equity} that is less or equal zero.", nameof(equity));
            }
            if (riskPerc is < 0 or > 1)
            {
                throw new ArgumentOutOfRangeException(nameof(riskPerc), $"Risk per trade is in percentage. Acceptable value is between zero " +
                    $"and one but but value in input is {riskPerc}.");
            }
            if (decimalPlaces < -1)
            {
                throw new ArgumentException($"Decimal places is {decimalPlaces} that is less then negative one.", nameof(decimalPlaces));
            }

            return SolveRound(equity * riskPerc, decimalPlaces);
        }

        private static decimal SolveRound(decimal result, int decimalPlaces)
        {
            if (decimalPlaces > -1)
            {
                result = decimal.Round(result, decimalPlaces);
            }

            return result;
        }

        /// <summary>
        /// Returns calculated stoploss of trade in percentage.
        /// When <paramref name="decimalPlaces"/> is equal negative one then rounding will not occur.
        /// </summary>
        /// <param name="entryPrice">Entry price of trade.</param>
        /// <param name="stopLossPrice">Stoploss price of trade.</param>
        /// <param name="decimalPlaces">How many decimal points should the result have.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static decimal StopLossPercentage(decimal entryPrice, decimal stopLossPrice, int decimalPlaces = 4)
        {
            if (entryPrice <= 0)
            {
                throw new ArgumentException($"Entry price is {entryPrice} that is less or equal zero.", nameof(entryPrice));
            }
            if (stopLossPrice < 0 || stopLossPrice == entryPrice)
            {
                throw new ArgumentOutOfRangeException(nameof(stopLossPrice), $"Stoploss price is except greater than zero and must not be equal to the entry price.");
            }
            if (decimalPlaces < -1)
            {
                throw new ArgumentException($"Decimal places is {decimalPlaces} that is less then negative one.", nameof(decimalPlaces));
            }

            var result = Math.Abs((100 - (stopLossPrice * 100) / entryPrice) / 100);
            return SolveRound(result, decimalPlaces);
        }

        /// <summary>
        /// Returns calculated postition size.
        /// When <paramref name="decimalPlaces"/> is equal negative one then rounding will not occur.
        /// </summary>
        /// <param name="entryPrice">Entry price of trade.</param>
        /// <param name="stopLossPrice">Stoploss price of trade.</param>
        /// <param name="riskPerc">Percentage risk per trade.</param>
        /// <param name="equity">The total capital size.</param>
        /// <param name="decimalPlaces">How many decimal points should the result have.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static decimal PositionSize(decimal entryPrice, decimal stopLossPrice, decimal riskPerc, decimal equity, int decimalPlaces = 8)
        {
            if (equity <= 0)
            {
                throw new ArgumentException($"Equity from input is {equity} that is less or equal zero.", nameof(equity));
            }
            if (riskPerc < 0 || riskPerc > 1)
            {
                throw new ArgumentOutOfRangeException(nameof(riskPerc), $"Risk per trade is in percentage. Acceptable value is between zero " +
                    $"and one but but value in input is {riskPerc}.");
            }
            if (entryPrice <= 0)
            {
                throw new ArgumentException($"Entry price is {entryPrice} that is less or equal zero.", nameof(entryPrice));
            }
            if (stopLossPrice < 0 || stopLossPrice == entryPrice)
            {
                throw new ArgumentOutOfRangeException(nameof(stopLossPrice), $"Stoploss price is except greater than zero and must not be equal to the entry price.");
            }
            if (decimalPlaces < -1)
            {
                throw new ArgumentException($"Decimal places is {decimalPlaces} that is less then negative one.", nameof(decimalPlaces));
            }

            var result = Math.Abs(PositionSizeByRisk(equity *5 , riskPerc, -1) / StopLossPercentage(entryPrice, stopLossPrice, -1));
            return SolveRound(result, decimalPlaces);
        }
    }
}
