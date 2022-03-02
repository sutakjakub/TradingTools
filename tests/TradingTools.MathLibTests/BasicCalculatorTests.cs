using Xunit;
using TradingTools.MathLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using FluentAssertions;

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
            Assert.Equal(expectedResult, price);
        }

        [Fact]
        public void SplitTest_ShouldEquals()
        {
            //arrange
            var startPrice = 5M;
            var endPrice = 15M;
            var numberDivisors = 5;
            //act
            var result = BasicCalculator.Split(startPrice, endPrice, numberDivisors);
            //assert
            Assert.Equal(numberDivisors, result.Length);
            Assert.Equal(5M, result[0]);
            Assert.Equal(7.5M, result[1]);
            Assert.Equal(10M, result[2]);
            Assert.Equal(12.5M, result[3]);
            Assert.Equal(15M, result[4]);
        }

        [Fact]
        public void DistributeAmount_ShouldEquals()
        {
            //arrange
            var amount = 100M;
            var ratios = new decimal[] { 0.2M, 0.2M, 0.3M, 0.2M, 0.1M };
            //act
            var result = BasicCalculator.DistributeAmount(amount, ratios);
            //assert
            Assert.Equal(ratios.Length, result.Length);
            Assert.Equal(20M, result[0]);
            Assert.Equal(20M, result[1]);
            Assert.Equal(30M, result[2]);
            Assert.Equal(20M, result[3]);
            Assert.Equal(10M, result[4]);
        }

        [Fact()]
        public void AveragePriceTest_Without_SellEntries_ShouldEqual()
        {
            //arrange
            var buyEntries = new List<(decimal quantity, decimal price)> 
            {
                (50M, 50M),
                (2.86M, 35M),
                (2.5M, 40M),
                (1.25M, 40M)
            };

            var sellEntries = new List<(decimal quantity, decimal price)>();

            //act
            var result = BasicCalculator.AverageCost(buyEntries, sellEntries);

            //assert
            Assert.Equal(48.58M, result);
        }

        [Fact()]
        public void AveragePriceTest_With_SellEntries_ShouldEqual()
        {
            //arrange
            var buyEntries = new List<(decimal quantity, decimal price)>
            {
                (1M, 40000M)
            };

            var sellEntries = new List<(decimal quantity, decimal price)>
            { 
                (0.5M, 40000M),
                (0.5M, 40000M)
            };

            //act
            var result = BasicCalculator.AverageCost(buyEntries, sellEntries);

            //assert
            Assert.Equal(0, result);
        }

        [Fact()]
        public void AveragePriceTest_With_SellEntries2_ShouldEqual()
        {
            //arrange
            var buyEntries = new List<(decimal quantity, decimal price)>
            {
                (1M, 10000M)
            };

            var sellEntries = new List<(decimal quantity, decimal price)>
            {
                (0.25M, 15000M),
                (0.25M, 20000M)
            };

            //act
            var result = BasicCalculator.AverageCost(buyEntries, sellEntries);

            //assert
            Assert.Equal(2500, result);
        }

        [Fact()]
        public void AveragePriceTest_With_SellEntries3_ShouldEqual()
        {
            //arrange
            var buyEntries = new List<(decimal quantity, decimal price)>
            {
                (10M, 1.14M),
                (10M, 1.13M)
            };

            var sellEntries = new List<(decimal quantity, decimal price)>
            {
                (10M, 1.145M)
            };

            //act
            var result = BasicCalculator.AverageCost(buyEntries, sellEntries, 3);

            //assert
            Assert.Equal(1.125M, result);
        }

        [Fact]
        public void TotalGain_ShouldEquals_1()
        {

            //arrange
            var buyEntries = new List<(decimal quantity, decimal price)>
            {
                (100M, 10M)
            };

            var sellEntries = new List<(decimal quantity, decimal price)>
            {
                (100M, 17M)
            };

            //act
            var result = BasicCalculator.TotalGain(buyEntries, sellEntries, 8);

            //assert
            Assert.Equal(700M, result.GainLoss);
        }

        /// <summary>
        /// https://www.sofi.com/learn/content/how-to-calculate-stock-profit/
        /// </summary>
        [Fact]
        public void TotalGain_ShouldEquals_2()
        {

            //arrange
            var buyEntries = new List<(decimal quantity, decimal price)>
            {
                (100M, 20M)
            };

            var sellEntries = new List<(decimal quantity, decimal price)>
            {
                (100M, 23M)
            };

            //act
            var result = BasicCalculator.TotalGain(buyEntries, sellEntries, 2);

            //assert
            result.GainLoss.Should().Be(300M);
        }

        /// <summary>
        /// https://www.janushenderson.com/en-us/investor/planning/calculate-average-cost/
        /// </summary>
        [Fact]
        public void TotalGain_ShouldEquals_3()
        {
            //arrange
            var buyEntries = new List<(decimal quantity, decimal price)>
            {
                (50M, 50M),
                (2.86M, 35M),
                (2.5M, 40M),
                (1.25M, 40M),
            };

            var sellEntries = new List<(decimal quantity, decimal price)>
            {
                (5M, 70M)
            };

            //act
            var result = BasicCalculator.TotalGain(buyEntries, sellEntries, 2);

            //assert
            result.GainLoss.Should().Be(107.10M);
        }

        [Fact]
        public void TotalGain_ShouldEquals_4()
        {
            //arrange
            var buyEntries = new List<(decimal quantity, decimal price)>
            {
                (0.03087M, 161968.92010786M),
            };

            var sellEntries = new List<(decimal quantity, decimal price)>
            {
                (0.03087012M, 168383.86115765M)
            };

            //act
            var result = BasicCalculator.TotalGain(buyEntries, sellEntries, 2);

            //assert
            result.GainLoss.Should().Be(198.03M);
        }
    }
}