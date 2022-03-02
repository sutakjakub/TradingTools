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
using CoinGecko.Clients;
using TradingTools.Persistence.Queries;
using System.Collections.Generic;
using TradingTools.Db.Entities;
using TradingTools.Persistence.Queries.Interfaces;
using System.IO;
using CsvHelper;
using System.Globalization;
using CsvHelper.Configuration;
using TradingTools.Shared.Enums;
using TradingTools.Taxes.Strategies;
using Moq;
using TradingTools.Taxes;
using Microsoft.Extensions.Logging;
using CoinGecko.Entities.Response.Coins;
using MoreLinq;

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

            //var source = db.T2Trades
            //    .Where(p => p.QuoteUsdValue == 0M)
            //    .Include(i => i.T2SymbolInfo)
            //    .Select(s => new { asset = s.T2SymbolInfo.QuoteAsset, date = s.TradeTime, id = s.Id })
            //    .AsEnumerable()
            //    .Select(s => (asset: s.asset, date: s.date, id: s.id)).ToList();

            //var values = await CoinGeckoData(source);
            //foreach (var (id, usdValue) in values)
            //{
            //    var entity = db.T2Trades.Find(id);
            //    entity.QuoteUsdValue = usdValue;
            //}
            //await db.SaveChangesAsync();


            //var list = await FromCsv();
            //foreach (var item in list)
            //{
            //    await db.T2Trades.AddAsync(item);
            //    await db.SaveChangesAsync();
            //}

            var data = db.T2Trades
                .Include(i => i.T2SymbolInfo)
                .ToList();
            //var roots = TradingTools.Taxes.Converter.Convert(data);

            var strategy = new FifoTaxStrategy(Mock.Of<ILogger<FifoTaxStrategy>>());
            var taxCalculator = new TaxCalculator(strategy);
            taxCalculator.Calculate(data);

            using (var writer = new StreamWriter("crypto_gains.csv"))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(taxCalculator.TaxReportItems);
            }

            //strategy.Load(roots);

            var exchange = new BinanceExchangeService(client);
            var tradeQuery = new T2TradeQuery(db);

            var groups = tradeQuery.GroupByBaseAsset();

            foreach (var group in groups)
            {
                var tradeGroup = new T2TradeGroupEntity
                {
                    BaseAsset = group.baseAsset,
                    Name = $"Default_{group.baseAsset}",
                    Trades = group.trades
                };
                db.T2TradeGroups.Add(tradeGroup);
            }

            await db.SaveChangesAsync();

            foreach (var item in db.T2Trades.Where(w => w.T2SymbolInfoId == null).ToList())
            {
                item.T2SymbolInfo = db.T2SymbolInfos.Single(s => s.Symbol == item.Symbol);
                db.T2Trades.Update(item);
            }
            await db.SaveChangesAsync();

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

            //var dbSymbols = db.T2SymbolInfos
            //    .Where(p => (p.BaseAsset == "VIDT"))
            //    .OrderBy(o => o.Symbol).ToList();

            //dbSymbols = dbSymbols.SkipWhile(s => s.Symbol != "BTCUSDT").ToList();

            foreach (var symbol in dbSymbols)
            {
                await Task.Delay(200);
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
                    IsMaker = s.IsMaker,
                    T2SymbolInfoId = db.T2SymbolInfos.First(f => f.Symbol == s.Symbol).Id
                }))
                {
                    if (!db.T2Trades.Any(f => f.TradeId == item.TradeId))
                    {
                        db.T2Trades.Add(item);
                    }
                }
                db.SaveChanges();
            }

            var geckoClient = CoinGeckoClient.Instance;
            var pricesGecko = await geckoClient.CoinsClient.GetMarketChartRangeByCoinId("bitcoin", "usd",
                            ((DateTimeOffset)new DateTime(2020, 3, 1)).ToUnixTimeSeconds().ToString(), ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds().ToString());

            var prices = pricesGecko.Prices
                .Select(s => new { Date = UnixTimeStampToDateTime(double.Parse(s[0].ToString())), Price = decimal.Parse(s[1].ToString()) }).ToList();
            foreach (var item in db.T2Trades.Include(p => p.T2SymbolInfo).ToList())
            {
                if (item.QuoteUsdValue == 0) //not initialized yet
                {
                    if (item.T2SymbolInfo.QuoteAsset == "BTC")
                    {
                        item.QuoteUsdValue = prices.Single(s => s.Date.Date == item.TradeTime.Date).Price;
                        db.T2Trades.Update(item);
                    }
                    else if (item.T2SymbolInfo.QuoteAsset == "USDT")
                    {
                        item.QuoteUsdValue = 1.0M;
                        db.T2Trades.Update(item);
                    }
                }
            }
            await db.SaveChangesAsync();


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

        private static IReadOnlyList<CoinList> coinList;
        private static async Task<IEnumerable<(long id, decimal usdValue)>> CoinGeckoData(IEnumerable<(string asset, DateTime date, long id)> source)
        {
            var result = new List<(long id, decimal usdValue)>();

            var geckoClient = CoinGeckoClient.Instance;
            if (coinList == null)
            {
                Console.WriteLine("downloading coinlist");
                coinList = await geckoClient.CoinsClient.GetCoinList();
            }

            var minDate = source.Min(m => m.date);
            var daysAgo = (DateTime.Now - minDate).TotalDays.ToString();
            var distinctedAssets = source.Select(s => s.asset).Distinct();
            var assetValues = new List<(string asset, DateTime date, decimal price)>();
            foreach (var asset in distinctedAssets)
            {
                if (!assetValues.Any(a => a.asset == asset))
                {
                    var coin = coinList.FirstOrDefault(f => f.Symbol == asset.ToLower());
                    if (coin == null)
                    {
                        Console.WriteLine($"{asset} is not in coinlist");
                        continue;
                    }
                    var chartData = await geckoClient.CoinsClient.GetMarketChartsByCoinId(coin.Id, "usd", daysAgo);

                    assetValues.AddRange(
                        chartData.Prices
                        .Select(s => new { asset, date = UnixTimeStampToDateTime(double.Parse(s[0].ToString())), price = decimal.Parse(s[1].ToString()) })
                        .Select(s => (s.asset, s.date, s.price)).ToList());

                    Console.WriteLine($"waiting 1500ms");
                    await Task.Delay(1500);
                }
            }

            foreach (var (asset, date, id) in source)
            {
                if (assetValues.Any(a => a.asset == asset))
                {
                    var closestTime = assetValues
                        .Where(p => p.asset == asset)
                        .MinBy(t => Math.Abs((t.date - date).Ticks)).ElementAt(0);
                    result.Add((id, closestTime.price));
                }
                else
                {
                    Console.WriteLine($"not found price for {asset}");
                }
            }

            Console.WriteLine($"returns {result.Count} items");
            return result;
        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddMilliseconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }
        private static async Task<List<T2TradeEntity>> FromCsv()
        {
            var path = @".\crypto_transactions_2020_FIFO_Universal.csv";
            using var reader = new StreamReader(path);
            using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture) { Delimiter = "," });
            csv.Context.RegisterClassMap<ModelClassMap>();
            var records = csv.GetRecords<Model>().ToList();

            List<T2TradeEntity> entities = new();
            foreach (var s in records.Where(p => p.ReceivedWallet.StartsWith("Imported Wallet") && (p.Type == "Buy" || p.Type == "Sell")))
            {
                try
                {
                    var entity = new T2TradeEntity
                    {
                        Commission = decimal.Parse(s.FeeAmount),
                        CommissionAsset = s.FeeCurrency,
                        ExchangeType = ConvertToExchangeType(s.ReceivedAddress),
                        TradeTime = DateTime.Parse(s.Date)
                    };

                    var isBuyer = s.Type == "Buy";
                    if (isBuyer)
                    {
                        entity.Symbol = s.ReceivedCurrency + s.SentCurrency;
                        entity.Quantity = decimal.Parse(s.ReceivedQuantity);
                        entity.QuoteQuantity = decimal.Parse(s.SentQuantity);
                    }
                    else
                    {
                        entity.Symbol = s.SentCurrency + s.ReceivedCurrency;
                        entity.Quantity = decimal.Parse(s.SentQuantity);
                        entity.QuoteQuantity = decimal.Parse(s.ReceivedQuantity);
                    }
                    entity.Price = entity.QuoteQuantity / entity.Quantity;
                    entity.IsBuyer = isBuyer;
                    entities.Add(entity);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }

            return entities;
        }

        private static T2ExchangeType ConvertToExchangeType(string receivedAddress)
        {
            foreach (T2ExchangeType item in Enum.GetValues(typeof(T2ExchangeType)))
            {
                if (receivedAddress.Contains(item.ToString()))
                {
                    return item;
                }
            }

            return T2ExchangeType.None;
        }

    }

    public class Model
    {
        public string Date { get; set; }
        public string Type { get; set; }
        public string TransactionID { get; set; }
        public string ReceivedQuantity { get; set; }
        public string ReceivedCurrency { get; set; }
        public string ReceivedCurrencyBalance { get; set; }
        public string ReceivedCostBasisCZK { get; set; }
        public string ReceivedWallet { get; set; }
        public string ReceivedAddress { get; set; }
        public string ReceivedTag { get; set; }
        public string ReceivedComment { get; set; }
        public string SentQuantity { get; set; }
        public string SentCurrency { get; set; }
        public string SentCurrencyBalance { get; set; }
        public string SentCostBasisCZK { get; set; }
        public string SentWallet { get; set; }
        public string SentAddress { get; set; }
        public string SentTag { get; set; }
        public string SentComment { get; set; }
        public string FeeAmount { get; set; }
        public string FeeCurrency { get; set; }
        public string RealizedReturnCZK { get; set; }
        public string Disabled { get; set; }
    }

    public class ModelClassMap : ClassMap<Model>
    {
        public ModelClassMap()
        {
            Map(m => m.Date).Name("Date");
            Map(m => m.Type).Name("Type");
            Map(m => m.TransactionID).Name("Transaction ID");
            Map(m => m.ReceivedQuantity).Name("Received Quantity");
            Map(m => m.ReceivedCurrency).Name("Received Currency");
            Map(m => m.ReceivedCurrencyBalance).Name("Received Currency Balance");
            Map(m => m.ReceivedCostBasisCZK).Name("Received Cost Basis (CZK)");
            Map(m => m.ReceivedWallet).Name("Received Wallet");
            Map(m => m.ReceivedAddress).Name("Received Address");
            Map(m => m.ReceivedTag).Name("Received Tag");
            Map(m => m.ReceivedComment).Name("Received Comment");
            Map(m => m.SentQuantity).Name("Sent Quantity");
            Map(m => m.SentCurrency).Name("Sent Currency");
            Map(m => m.SentCurrencyBalance).Name("Sent Currency Balance");
            Map(m => m.SentCostBasisCZK).Name("Sent Cost Basis (CZK)");
            Map(m => m.SentWallet).Name("Sent Wallet");
            Map(m => m.SentAddress).Name("Sent Address");
            Map(m => m.SentTag).Name("Sent Tag");
            Map(m => m.SentComment).Name("Sent Comment");
            Map(m => m.FeeAmount).Name("Fee Amount");
            Map(m => m.FeeCurrency).Name("Fee Currency");
            Map(m => m.RealizedReturnCZK).Name("Realized Return (CZK)");
            Map(m => m.Disabled).Name("Disabled");
        }
    }
}

