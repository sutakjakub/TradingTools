using Xunit;
using TradingTools.Taxes.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingTools.Taxes.Models;
using Microsoft.Extensions.Logging;
using Moq;
using FluentAssertions;

namespace TradingTools.Taxes.Strategies.Tests
{
    public class FifoTaxStrategyTests
    {
        [Fact]
        public void Process_Simple_Should_Work()
        {
            //arrange
            var roots = new List<RootElement>
            {
                new RootElement
                {
                    //AssetName = "BTC",
                    Items = new List<Element>
                    {
                        new Element
                        {
                            Amount = 0.5M,
                            AssetName = "BTC",
                            CommisionAmount = 0.1M,
                            CommisionAsset = "USDT",
                            IsBuyer = true,
                            Price = 12500M,
                            TradeDateTime = new DateTimeOffset(2021,1,1,10,10,0, TimeSpan.Zero)
                        },
                        new Element
                        {
                            Amount = 0.5M,
                            AssetName = "BTC",
                            CommisionAmount = 0.1M,
                            CommisionAsset = "USDT",
                            IsBuyer = false,
                            Price = 15000M,
                            TradeDateTime = new DateTimeOffset(2021,1,2,10,10,0, TimeSpan.Zero)
                        },
                    }
                }
            };
            var strategy = new FifoTaxStrategy(Mock.Of<ILogger<FifoTaxStrategy>>());

            //act
            strategy.Load(roots);
            strategy.Process();

            //assert
            strategy.TaxData.Should().HaveCount(1);
            strategy.TaxData[0].BuyItems.Should().HaveCount(1);

        }

        [Fact]
        public void Process_SellWithRemainder_Should_Work()
        {
            //arrange
            var roots = new List<RootElement>
            {
                new RootElement
                {
                    //AssetName = "BTC",
                    Items = new List<Element>
                    {
                        new Element
                        {
                            Amount = 0.2M,
                            AssetName = "BTC",
                            CommisionAmount = 0.1M,
                            CommisionAsset = "USDT",
                            IsBuyer = true,
                            Price = 12500M,
                            TradeDateTime = new DateTimeOffset(2021,1,1,10,10,0, TimeSpan.Zero)
                        },
                        new Element
                        {
                            Amount = 0.3M,
                            AssetName = "BTC",
                            CommisionAmount = 0.1M,
                            CommisionAsset = "USDT",
                            IsBuyer = true,
                            Price = 12500M,
                            TradeDateTime = new DateTimeOffset(2021,1,2,10,10,0, TimeSpan.Zero)
                        },
                        new Element
                        {   
                            Amount = 0.5M,
                            AssetName = "BTC",
                            CommisionAmount = 0.1M,
                            CommisionAsset = "USDT",
                            IsBuyer = false,
                            Price = 15000M,
                            TradeDateTime = new DateTimeOffset(2021,1,3,10,10,0, TimeSpan.Zero)
                        },
                    }
                }
            };
            var strategy = new FifoTaxStrategy(Mock.Of<ILogger<FifoTaxStrategy>>());

            //act
            strategy.Load(roots);
            strategy.Process();

            //assert
            strategy.TaxData.Should().HaveCount(1);
            strategy.TaxData[0].BuyItems.Should().HaveCount(2);
        }

        [Fact]
        public void Process_SellWithRemainder_Should_Work_2()
        {
            //arrange
            var roots = new List<RootElement>
            {
                new RootElement
                {
                    //AssetName = "BTC",
                    Items = new List<Element>
                    {
                        new Element
                        {
                            Amount = 0.4M,
                            AssetName = "BTC",
                            CommisionAmount = 0.1M,
                            CommisionAsset = "USDT",
                            IsBuyer = true,
                            Price = 12500M,
                            TradeDateTime = new DateTimeOffset(2021,1,1,10,10,0, TimeSpan.Zero)
                        },
                        new Element
                        {
                            Amount = 0.8M,
                            AssetName = "BTC",
                            CommisionAmount = 0.1M,
                            CommisionAsset = "USDT",
                            IsBuyer = true,
                            Price = 12500M,
                            TradeDateTime = new DateTimeOffset(2021,1,2,10,10,0, TimeSpan.Zero)
                        },
                        new Element
                        {
                            Amount = 0.5M,
                            AssetName = "BTC",
                            CommisionAmount = 0.1M,
                            CommisionAsset = "USDT",
                            IsBuyer = false,
                            Price = 15000M,
                            TradeDateTime = new DateTimeOffset(2021,1,3,10,10,0, TimeSpan.Zero)
                        },
                    }
                }
            };
            var strategy = new FifoTaxStrategy(Mock.Of<ILogger<FifoTaxStrategy>>());

            //act
            strategy.Load(roots);
            strategy.Process();

            //assert
            strategy.TaxData.Should().HaveCount(1);
            strategy.TaxData[0].BuyItems.Should().HaveCount(2);
            strategy.TaxData[0].BuyItems[0].Amount.Should().Be(0.4M);
            strategy.TaxData[0].BuyItems[1].Amount.Should().Be(0.1M);
        }
    }
}