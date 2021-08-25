using System;
using System.Collections.Generic;
using System.Linq;
using TradingTools.MathLib.Models;

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

            var result = Math.Abs(PositionSizeByRisk(equity, riskPerc, -1) / StopLossPercentage(entryPrice, stopLossPrice, -1));
            return SolveRound(result, decimalPlaces);
        }

        /// <summary>
        /// Returns an evenly split array of prices.
        /// When <paramref name="decimalPlaces"/> is equal negative one then rounding will not occur.
        /// </summary>
        /// <param name="startPrice">Start price.</param>
        /// <param name="endPrice">End price.</param>
        /// <param name="divisors">Count of divisors.</param>
        /// <param name="decimalPlaces">How many decimal points should the result have.</param>
        /// <returns></returns>
        public static decimal[] Split(decimal startPrice, decimal endPrice, int divisors, int decimalPlaces = 2)
        {
            if (startPrice <= 0)
            {
                throw new ArgumentException($"Input is {startPrice} that is less or equal zero.", nameof(startPrice));
            }
            if (endPrice <= 0)
            {
                throw new ArgumentException($"Input is {endPrice} that is less or equal zero.", nameof(endPrice));
            }
            if (divisors <= 0)
            {
                throw new ArgumentException($"Input is {divisors} that is less or equal zero.", nameof(divisors));
            }
            if (decimalPlaces < -1)
            {
                throw new ArgumentException($"Decimal places is {decimalPlaces} that is less then negative one.", nameof(decimalPlaces));
            }

            //greater value
            var greater = Math.Max(startPrice, endPrice);
            //lower value
            var lower = Math.Min(startPrice, endPrice);

            var diff = (greater - lower) / (divisors - 1); //  10dilku/4usekama
            var parts = new decimal[divisors];
            parts[0] = lower;
            parts[divisors - 1] = greater;
            for (int i = 1; i <= divisors - 2; i++)
            {
                parts[i] = SolveRound(parts[i - 1] + diff, decimalPlaces);
            }

            return parts;
        }

        /// <summary>
        /// Returns array of splitted amount by ratios.
        /// </summary>
        /// <param name="amount">Amount.</param>
        /// <param name="ratios">Ratios.</param>
        /// <returns></returns>
        public static decimal[] DistributeAmount(decimal amount, decimal[] ratios)
        {
            decimal total = 0;
            decimal[] results = new decimal[ratios.Length];

            // find the total of the ratios
            for (int index = 0; index < ratios.Length; index++)
            {
                total += ratios[index];
            }

            // convert amount to a fixed point value (no mantissa portion)
            amount *= 100;
            decimal remainder = amount;
            for (int index = 0; index < results.Length; index++)
            {
                results[index] = Math.Floor(amount * ratios[index] / total);
                remainder -= results[index];
            }

            // allocate remainder across all amounts
            for (int index = 0; index < remainder; index++)
            {
                results[index]++;
            }

            // convert back to decimal portion
            for (int index = 0; index < results.Length; index++)
            {
                results[index] = results[index] / 100;
            }

            return results;
        }

        /// <summary>
        /// Returns average cost of trades.
        /// When <paramref name="decimalPlaces"/> is equal negative one then rounding will not occur.
        /// </summary>
        /// <param name="buyEntries">Buy entries in trade</param>
        /// <param name="sellEntries">Sell entries in trade</param>
        /// <param name="decimalPlaces">How many decimal points should the result have.</param>
        /// <returns></returns>
        public static decimal AverageCost(IEnumerable<(decimal quantity, decimal price)> buyEntries, IEnumerable<(decimal quantity, decimal price)> sellEntries, int decimalPlaces = 2)
        {
            var denominator = buyEntries.Sum(s => s.quantity) - sellEntries.Sum(s => s.quantity);
            if (denominator == 0)
            {
                return 0;
            }

            decimal numerator = 0.0M;
            foreach (var (quantity, price) in buyEntries)
            {
                numerator += quantity * price;
            }

            foreach (var (quantity, price) in sellEntries)
            {
                numerator -= quantity * price;
            }

            var result = numerator / denominator;
            return SolveRound(result, decimalPlaces);
        }

        public static decimal AverageCost(IEnumerable<(decimal quantity, decimal price)> source)
        {
            if (!source.Any())
            {
                return 0;
            }

            var intermediateResult = 0.0M;
            foreach (var (quantity, price) in source)
            {
                intermediateResult += quantity * price;
            }

            return intermediateResult / source.Sum(s => s.quantity);
        }

        //public static decimal CostBasis(IEnumerable<CostBasisModel> source, int decimalPlaces = 2)
        //{
        //    var result = (source.Sum(s => s.PurchasePrice) + source.Sum(s => s.Fee)) / source.Sum(s => s.Quantity);
        //    return SolveRound(result, decimalPlaces);
        //}

        //public static decimal AverageCostPerShare(decimal totalPurchase, int decimalPlaces = 2)
        //{

        //}
    }
}
