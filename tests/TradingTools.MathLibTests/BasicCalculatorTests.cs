using Xunit;
using TradingTools.MathLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingTools.MathLib.Tests
{
    public class BasicCalculatorTests
    {
        [Theory]
        [InlineData(2000, 0.01, 20)]
        [InlineData(4000, 0.1, 400)]
        [InlineData(1000, 0.01, 10)]
        public void PositionSizeByRiskTest_Should_Equals(decimal equity, decimal riskPerc, decimal expectedResult)
        {
            //act
            var price = BasicCalculator.PositionSizeByRisk(equity, riskPerc);
            //assert
            Assert.Equal(price, expectedResult);
        }

        [Theory]
        [InlineData(-1, 0.01)]
        [InlineData(0, 0.01)]
        public void PositionSizeByRiskTest_Equity_Should_ThrowsArgumentException(decimal equity, decimal riskPerc)
        {
            //act
            void act() => BasicCalculator.PositionSizeByRisk(equity, riskPerc);
            //assert
            var exception = Assert.Throws<ArgumentException>(act);
            //The thrown exception can be used for even more detailed assertions.
            Assert.Contains("equity", exception.ParamName, StringComparison.OrdinalIgnoreCase);
        }

        [Theory]
        [InlineData(10, -0.1)]
        [InlineData(10, 1.01)]
        public void PositionSizeByRiskTest_RiskPerc_Should_ThrowsArgumentOutOfRangeException(decimal equity, decimal riskPerc)
        {
            //act
            void act() => BasicCalculator.PositionSizeByRisk(equity, riskPerc);
            //assert
            var exception = Assert.Throws<ArgumentOutOfRangeException>(act);
            //The thrown exception can be used for even more detailed assertions.
            Assert.Contains("riskPerc", exception.ParamName, StringComparison.OrdinalIgnoreCase);
        }

        [Theory]
        [InlineData(0.49880, 0.44860, 0.1006)]
        [InlineData(30.7, 27.74000, 0.0964)]
        [InlineData(4.35700, 3.58000, 0.1783)]
        public void StopLossPercentageTest_ShouldEquals(decimal equity, decimal stopLossPrice, decimal expectedResult)
        {
            //act
            var price = BasicCalculator.StopLossPercentage(equity, stopLossPrice);
            //assert
            Assert.Equal(price, expectedResult);
        }

        [Theory]
        [InlineData(4.357, 3.58, 0.058, 2000, 2, 650.47)]
        [InlineData(75, 72.255, 0.01, 2000, 2, 546.45)]
        [InlineData(0.021, 0.019, 0.01, 2000, 2, 210.00)]
        public void PositionSizeTest_ShouldEquals(decimal entryPrice, decimal stopLossPrice, decimal riskPerc, decimal equity, int decimalPlaces, decimal expectedResult)
        {
            //act
            var price = BasicCalculator.PositionSize(entryPrice, stopLossPrice, riskPerc, equity, decimalPlaces);
            //assert
            Assert.Equal(price, expectedResult);
        }
    }
}