using Binance.Net;
using Binance.Net.Objects;
using CryptoExchange.Net.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Linq;
using System.Threading.Tasks;
using TradingTools.Db;
using TradingTools.MathLib;
using TradingTools.ExchangeServices;

namespace TradingTools.TestConsole
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json", true, true)
                .AddEnvironmentVariables()
                .AddUserSecrets<Program>();

            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("program_Main.log")
                .CreateLogger();

            var config = builder.Build();

            var optionsBuilder = new DbContextOptionsBuilder<TradingToolsDbContext>();
            optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));

            var binanceOptions = new BinanceClientOptions
            {
                ApiCredentials = new ApiCredentials(config["binance:key"], config["binance:secret"]),
                AutoTimestamp = true
            };

            using var db = new TradingToolsDbContext(optionsBuilder.Options);
            using var client = new BinanceClient(binanceOptions);
            var exchange = new BinanceExchangeService(client);

            //var all = db.T2Orders.Where(p => p.Symbol == "AVAXUSDT").ToList();
            //var allSell = all.Where(p => p.Side == Shared.Enums.T2OrderSide.Sell).ToList();
            //var allBuy = all.Where(p => p.Side == Shared.Enums.T2OrderSide.Buy);

            //var ppp = allSell.Select(s => new { s.QuantityFilled, s.AverageFillPrice });

            //var lll = db.T2Orders
            //    .Where(p => p.Symbol == "AVAXUSDT" && p.Side == Shared.Enums.T2OrderSide.Sell)
            //    .Select(c => new { c.QuantityFilled, c.AverageFillPrice })
            //    .AsEnumerable()
            //    .Select(s => (quantity: s.QuantityFilled, price: s.AverageFillPrice.Value))
            //    .ToList();

            //var result1 = AverageCost(db, "AVAXBTC");
            //var result2 = AverageCost(db, "AVAXUSDT");

            //return;

            if (!db.T2SymbolInfos.Any())
            {
                var symbols = await exchange.GetAllSymbolsAsync();
                db.T2SymbolInfos.AddRange(symbols.Select(s => new Db.Entities.T2SymbolInfoEntity
                {
                    BaseAsset = s.BaseAsset,
                    BaseAssetCommissionPrecision = s.BaseAssetCommissionPrecision,
                    BaseAssetPrecision = s.BaseAssetPrecision,
                    ExchangeType = s.ExchangeType,
                    QuoteAsset = s.QuoteAsset,
                    QuoteAssetCommissionPrecision = s.QuoteAssetCommissionPrecision,
                    QuoteAssetPrecision = s.QuoteAssetPrecision,
                    Symbol = s.Symbol
                }));
                db.SaveChanges();
            }


            var dbSymbols = db.T2SymbolInfos
                .Where(p => p.QuoteAsset == "BTC" || p.QuoteAsset == "USDT")
                .OrderBy(o => o.Symbol).ToList();

            //dbSymbols = dbSymbols.SkipWhile(s => s.Symbol != "BTCUSDT").ToList();

            foreach (var symbol in dbSymbols)
            {
                await Task.Delay(300);
                var lastEntity = await db.T2Trades.Where(p => p.Symbol == symbol.Symbol).OrderByDescending(o => o.TradeId).FirstOrDefaultAsync();
                var trades = await exchange.GetTradesAsync(symbol.Symbol, lastEntity?.TradeId);
                Log.Information($"{symbol.Symbol} __ {trades.Count()} ___ {lastEntity?.TradeId}");
                foreach (var item in trades.Select(s => new Db.Entities.T2TradeEntity
                {
                    Commission = s.Commission,
                    CommissionAsset = s.CommissionAsset,
                    ExchangeType = s.ExchangeType,
                    OrderId = s.OrderId,
                    Price = s.Price,
                    Quantity = s.Quantity,
                    QuoteQuantity = s.QuoteQuantity,
                    Symbol = s.Symbol,
                    TradeId = s.TradeId,
                    TradeTime = s.TradeTime,
                    IsBestMatch = s.IsBestMatch,
                    IsBuyer = s.IsBuyer,
                    IsMaker = s.IsMaker
                }))
                {
                    if (!db.T2Trades.Any(f => f.TradeId == item.TradeId))
                    {
                        db.T2Trades.Add(item);
                    }
                }
                db.SaveChanges();
            }


            if (false)
            {
                var tradePairs = db.T2Trades.Select(s => new { s.OrderId, s.Symbol, s.Id }).ToList();
                var tradeOrderIdList = tradePairs.Select(s => s.OrderId);
                var orderIdList = db.T2Orders.Select(s => s.OrderId).ToList();
                var exceptedId = tradeOrderIdList.Except(orderIdList).ToList();
                foreach (var tradePair in tradePairs.Where(p => exceptedId.Contains(p.OrderId)))
                {
                    await Task.Delay(300);
                    var order = await exchange.GetOrderAsync(tradePair.Symbol, tradePair.OrderId);
                    if (order == null)
                    {
                        Log.Information($"Not found order {tradePair.Symbol} : {tradePair.OrderId}");
                    }
                    else
                    {
                        Log.Information($"{tradePair.Symbol} : {tradePair.OrderId}");
                        var newOrder = new Db.Entities.T2OrderEntity
                        {
                            T2SymbolInfoId = db.T2SymbolInfos.First(f => f.Symbol == tradePair.Symbol).Id,
                            AverageFillPrice = order.AverageFillPrice,
                            ClientOrderId = order.ClientOrderId,
                            CreateTime = order.CreateTime,
                            IsWorking = order.IsWorking,
                            OrderId = order.OrderId,
                            OrderListId = order.OrderListId,
                            OriginalClientOrderId = order.OriginalClientOrderId,
                            Price = order.Price,
                            Quantity = order.Quantity,
                            QuantityFilled = order.QuantityFilled,
                            QuoteQuantity = order.QuoteQuantity,
                            QuoteQuantityFilled = order.QuoteQuantityFilled,
                            QuantityRemaining = order.QuantityRemaining,
                            Side = order.Side,
                            Status = order.Status,
                            StopPrice = order.StopPrice,
                            Symbol = order.Symbol,
                            TimeInForce = order.TimeInForce,
                            Type = order.Type,
                            UpdateTime = order.UpdateTime
                        };
                        db.T2Orders.Add(newOrder);

                        foreach (var item in db.T2Trades.Where(f => f.Id == tradePair.Id).ToList())
                        {
                            item.T2Order = newOrder;
                        }

                        db.SaveChanges();
                    }
                }
            }
        }

        private static decimal AverageCost(TradingToolsDbContext db, string v)
        {
            //var symbolEntity = db.T2SymbolInfos.First(f=>f.)
            var buyEntries = db.T2Trades
                .Where(p => p.Symbol == v && p.IsBuyer)
                .Select(c => new { c.Quantity, c.Price })
                .AsEnumerable()
                .Select(s => (quantity: s.Quantity, price: s.Price))
                .ToList();



            //var buy = db.T2Orders
            //    .Where(p => p.Symbol == v && p.Side == Shared.Enums.T2OrderSide.Buy)
            //    .Select(c => new { c.QuantityFilled, c.Price })
            //    .AsEnumerable()
            //    .Select(s => (quantity: s.QuantityFilled, price: s.Price))
            //    .ToList();

            var sellEntries = db.T2Trades
               .Where(p => p.Symbol == v && !p.IsBuyer)
               .Select(c => new { c.Quantity, c.Price })
               .AsEnumerable()
               .Select(s => (quantity: s.Quantity, price: s.Price))
               .ToList();

            //var sell = db.T2Orders
            //    .Where(p => p.Symbol == v && p.Side == Shared.Enums.T2OrderSide.Sell)
            //    .Select(c => new { c.QuantityFilled, c.Price })
            //    .AsEnumerable()
            //    .Select(s => (quantity: s.QuantityFilled, price: s.Price))
            //    .ToList();

            //return BasicCalculator.AverageCost(buyEntries, sellEntries, 8);

            var qBuy = db.T2Trades.Where(p => p.Symbol == "AVAXUSDT" && p.IsBuyer).Sum(s => s.Quantity);
            var qSell = db.T2Trades.Where(p => p.Symbol == "AVAXUSDT" && !p.IsBuyer).Sum(s => s.Quantity);

            return 0;
        }
    }
}
