using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using TradingTools.Db.Entities;
using TradingTools.ExchangeServices.Interfaces;
using TradingTools.Persistence.Queries.Interfaces;
using TradingTools.Shared.Dto;
using TradingTools.Web.ViewModels;

namespace TradingTools.Web.Pages.Portfolio
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IBinanceExchangeService _binance;
        private readonly IT2SymbolInfoQuery _symbolInfoQuery;

        public IndexModel(
            ILogger<IndexModel> logger,
            IBinanceExchangeService binance,
            IT2SymbolInfoQuery symbolInfoQuery)
        {
            _logger = logger;
            _binance = binance;
            _symbolInfoQuery = symbolInfoQuery;
        }

        public IList<BinanceUserCoinViewModel> Coins { get; private set; }

        public async Task OnGet()
        {
            var list = await _binance.GetUserCoinsAsync();
            var prices = await _binance.GetPricesAsync();
            var btcUsdtPrice = prices.First(f => f.Symbol == "BTCUSDT").Price;
            List<BinanceUserCoinViewModel> temp = new();
            foreach (var item in list)
            {
                var vm = new BinanceUserCoinViewModel();
                var price = prices.FirstOrDefault(f => f.Symbol == item.Coin + "USDT");
                if (price == null)
                {
                    price = prices.FirstOrDefault(f => f.Symbol == item.Coin + "BTC");
                }

                T2SymbolInfoEntity symbolInfo = null;
                if (price != null)
                {
                    symbolInfo = await _symbolInfoQuery.FindByName(price.Symbol);
                }
                vm.Init(item, price, symbolInfo, btcUsdtPrice);

                temp.Add(vm);
            }

            Coins = temp.OrderByDescending(o => o.TotalDollarValue).ToList();
        }
    }
}
