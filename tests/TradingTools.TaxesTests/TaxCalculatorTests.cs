using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingTools.Db.Entities;
using TradingTools.Taxes;
using TradingTools.Taxes.Strategies;
using Xunit;

namespace TradingTools.TaxesTests
{
    public class TaxCalculatorTests
    {
        [Fact]
        public void Calculate_BasicScenario_Should_Work()
        {
            //arrange
            var strategy = new FifoTaxStrategy(Mock.Of<ILogger<FifoTaxStrategy>>());
            TaxCalculator calculator = new(strategy);
            var entities = new List<T2TradeEntity>
            {
                new T2TradeEntity
                {
                    Id = 1,
                    IsBuyer = true,
                    TradeTime = new DateTime(2021, 01,01),
                    Price = 10000M,
                    Quantity = 0.2M,
                    QuoteQuantity = 2000M,
                    QuoteUsdValue = 1.0M,
                    T2SymbolInfo = GetBtcUsdtSymbolInfo()
                },
                new T2TradeEntity
                {
                    Id = 2,
                    IsBuyer = false,
                    TradeTime = new DateTime(2021, 01,02),
                    Price = 20000M,
                    Quantity = 0.2M,
                    QuoteQuantity = 4000M,
                    QuoteUsdValue = 1.0M,
                    T2SymbolInfo = GetBtcUsdtSymbolInfo()
                },
            };
            //act
            calculator.Calculate(entities);
            var totalGain = calculator.TotalGain();
            //assert
            totalGain.Should().Be(2000M);
        }

        [Fact]
        public void Calculate_BasicScenarioWithCommision_Should_Work()
        {
            //arrange
            var strategy = new FifoTaxStrategy(Mock.Of<ILogger<FifoTaxStrategy>>());
            TaxCalculator calculator = new(strategy);
            var entities = new List<T2TradeEntity>
            {
                new T2TradeEntity
                {
                    Id = 1,
                    IsBuyer = true,
                    TradeTime = new DateTime(2021, 01,01),
                    Price = 10000M,
                    Quantity = 0.2M,
                    QuoteQuantity = 2000M,
                    QuoteUsdValue = 1.0M,
                    Commission = 0.0001M,
                    CommissionUsdValue = 1.0M,
                    T2SymbolInfo = GetBtcUsdtSymbolInfo()
                },
                new T2TradeEntity
                {
                    Id = 2,
                    IsBuyer = false,
                    TradeTime = new DateTime(2021, 01,02),
                    Price = 20000M,
                    Quantity = 0.2M,
                    QuoteQuantity = 4000M,
                    QuoteUsdValue = 1.0M,
                    Commission = 0.0002M,
                    CommissionUsdValue = 1.0M,
                    T2SymbolInfo = GetBtcUsdtSymbolInfo()
                },
            };
            //act
            calculator.Calculate(entities);
            var totalGain = calculator.TotalGain();
            //assert
            totalGain.Should().Be(2000M - 0.0003M);
        }

        [Fact]
        public void Calculate_AdvancedScenarioWithCommision_Should_Work_1()
        {
            //arrange
            var strategy = new FifoTaxStrategy(Mock.Of<ILogger<FifoTaxStrategy>>());
            TaxCalculator calculator = new(strategy);
            var entities = new List<T2TradeEntity>
            {
                new T2TradeEntity
                {
                    Id = 1,
                    IsBuyer = true,
                    TradeTime = new DateTime(2021, 01,01),
                    Price = 10000M,
                    Quantity = 0.2M,
                    QuoteQuantity = 2000M,
                    QuoteUsdValue = 1.0M,
                    Commission = 0.0001M,
                    CommissionUsdValue = 1.0M,
                    T2SymbolInfo = GetBtcUsdtSymbolInfo()
                },
                new T2TradeEntity
                {
                    Id = 2,
                    IsBuyer = false,
                    TradeTime = new DateTime(2021, 01,02),
                    Price = 20000M,
                    Quantity = 0.1M,
                    QuoteQuantity = 2000M,
                    QuoteUsdValue = 1.0M,
                    Commission = 0.0001M,
                    CommissionUsdValue = 1.0M,
                    T2SymbolInfo = GetBtcUsdtSymbolInfo()
                },
                new T2TradeEntity
                {
                    Id = 3,
                    IsBuyer = false,
                    TradeTime = new DateTime(2021, 01,03),
                    Price = 40000M,
                    Quantity = 0.1M,
                    QuoteQuantity = 4000M,
                    QuoteUsdValue = 1.0M,
                    Commission = 0.0001M,
                    CommissionUsdValue = 1.0M,
                    T2SymbolInfo = GetBtcUsdtSymbolInfo()
                }
            };
            //act
            calculator.Calculate(entities);
            var totalGain = calculator.TotalGain();
            //assert
            totalGain.Should().Be(4000M - 0.0003M);
        }

        [Fact]
        public void Calculate_AdvancedScenarioWithCommision_Should_Work_2()
        {
            //arrange
            var strategy = new FifoTaxStrategy(Mock.Of<ILogger<FifoTaxStrategy>>());
            TaxCalculator calculator = new(strategy);
            var entities = new List<T2TradeEntity>
            {
                new T2TradeEntity
                {
                    Id = 1,
                    IsBuyer = true,
                    TradeTime = new DateTime(2021, 01,01),
                    Price = 10000M,
                    Quantity = 0.2M,
                    QuoteQuantity = 2000M,
                    QuoteUsdValue = 1.0M,
                    Commission = 0.0001M,
                    CommissionUsdValue = 1.0M,
                    T2SymbolInfo = GetBtcUsdtSymbolInfo()
                },
                new T2TradeEntity
                {
                    Id = 2,
                    IsBuyer = false,
                    TradeTime = new DateTime(2021, 01,02),
                    Price = 20000M,
                    Quantity = 0.1M,
                    QuoteQuantity = 2000M,
                    QuoteUsdValue = 1.0M,
                    Commission = 0.0001M,
                    CommissionUsdValue = 1.0M,
                    T2SymbolInfo = GetBtcUsdtSymbolInfo()
                },
                new T2TradeEntity
                {
                    Id = 3,
                    IsBuyer = false,
                    TradeTime = new DateTime(2021, 01,03),
                    Price = 20000M,
                    Quantity = 0.1M,
                    QuoteQuantity = 2000M,
                    QuoteUsdValue = 1.0M,
                    Commission = 0.0001M,
                    CommissionUsdValue = 1.0M,
                    T2SymbolInfo = GetBtcUsdtSymbolInfo()
                }
            };
            //act
            calculator.Calculate(entities);
            var totalGain = calculator.TotalGain();
            //assert
            totalGain.Should().Be(2000M - 0.0003M);
        }

        [Fact]
        public void Calculate_AdvancedScenarioOverMoreCoins_Should_Work_1()
        {
            //arrange
            var strategy = new FifoTaxStrategy(Mock.Of<ILogger<FifoTaxStrategy>>());
            TaxCalculator calculator = new(strategy);
            var entities = new List<T2TradeEntity>
            {
                new T2TradeEntity
                {
                    Id = 1,
                    IsBuyer = true,
                    TradeTime = new DateTime(2021, 01,01),
                    Price = 10000M,
                    Quantity = 0.2M,
                    QuoteQuantity = 2000M,
                    QuoteUsdValue = 1.0M,
                    Commission = 0.0001M,
                    CommissionUsdValue = 1.0M,
                    T2SymbolInfo = GetBtcUsdtSymbolInfo()
                },
                new T2TradeEntity
                {
                    Id = 2,
                    IsBuyer = true,
                    TradeTime = new DateTime(2021, 01,02),
                    Price = 0.005M,
                    Quantity = 2.0M,
                    QuoteQuantity = 0.01M,
                    QuoteUsdValue = 15000M,
                    Commission = 0.0001M,
                    CommissionUsdValue = 1.0M,
                    T2SymbolInfo = GetLtcBtcSymbolInfo()
                },
                new T2TradeEntity
                {
                    Id = 3,
                    IsBuyer = false,
                    TradeTime = new DateTime(2021, 01,03),
                    Price = 200M,
                    Quantity = 0.1M,
                    QuoteQuantity = 2000M,
                    QuoteUsdValue = 1.0M,
                    Commission = 0.0001M,
                    CommissionUsdValue = 1.0M,
                    T2SymbolInfo = GetLtcUsdtSymbolInfo()
                }
            };
            //act
            calculator.Calculate(entities);
            var totalGain = calculator.TotalGain();
            //assert
            //12.49980
            totalGain.Should().Be(2000M - 0.0003M);
        }

        private static T2SymbolInfoEntity GetBtcUsdtSymbolInfo()
        {
            return new T2SymbolInfoEntity
            {
                Symbol = "BTCUSDT",
                BaseAsset = "BTC",
                QuoteAsset = "USDT"
            };
        }

        private static T2SymbolInfoEntity GetLtcBtcSymbolInfo()
        {
            return new T2SymbolInfoEntity
            {
                Symbol = "LTCBTC",
                BaseAsset = "LTC",
                QuoteAsset = "BTC"
            };
        }

        private static T2SymbolInfoEntity GetLtcUsdtSymbolInfo()
        {
            return new T2SymbolInfoEntity
            {
                Symbol = "LTCUSDT",
                BaseAsset = "LTC",
                QuoteAsset = "USDT"
            };
        }
    }
}
