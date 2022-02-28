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
        public void ProcessTest()
        {
            //arrange
            var roots = new List<RootElement>
            {
                new RootElement
                {
                    //AssetName = "BTC",
                    Items = new List<Element>
                    {
                        CreateElement(0.5M, true, new DateTimeOffset(2021,1,1,10,10,0, TimeSpan.Zero)),
                        CreateElement(0.5M, false, new DateTimeOffset(2021,1,2,10,10,0, TimeSpan.Zero)),
                    }
                }
            };
            var strategy = new FifoTaxStrategy(Mock.Of<ILogger<FifoTaxStrategy>>());

            //act
            strategy.Load(roots);
            strategy.Process();

            //assert
            strategy.TaxData.Should().HaveCount(2);

        }

        private static Element CreateElement(decimal amount, bool isBuyer, DateTimeOffset tradeDateTime)
        {
            return new Element
            {
                Amount= amount,
                IsBuyer = isBuyer,
                TradeDateTime = tradeDateTime
            };
        }
    }
}