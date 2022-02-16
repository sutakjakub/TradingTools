using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using TradingTools.Core.Synchronization.Interfaces;
using TradingTools.Db.Entities;
using TradingTools.ExchangeServices.Interfaces;
using TradingTools.Persistence.Queries.Interfaces;
using TradingTools.Persistence.Stores.Interfaces;
using TradingTools.Shared.Dto;
using TradingTools.Web.ViewModels;

namespace TradingTools.Web.Pages.Portfolio
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IBinanceExchangeService _binance;
        private readonly IT2SymbolInfoQuery _symbolInfoQuery;
        private readonly IT2Synchronizator _sync;
        private readonly IT2PortfolioCoinStore _coinStore;

        public IndexModel(
            ILogger<IndexModel> logger,
            IBinanceExchangeService binance,
            IT2SymbolInfoQuery symbolInfoQuery,
            IT2Synchronizator sync,
            IT2PortfolioCoinStore coinStore)
        {
            _logger = logger;
            _binance = binance;
            _symbolInfoQuery = symbolInfoQuery;
            _sync = sync;
            _coinStore = coinStore;
        }

        public IList<BinanceUserCoinViewModel> Coins { get; private set; }

        public async Task OnGet()
        {
            var list = await _coinStore.FindLastPortfolio();

            List<BinanceUserCoinViewModel> temp = new();
            foreach (var item in list)
            {
                var vm = new BinanceUserCoinViewModel(item);
                temp.Add(vm);
            }

            Coins = temp.OrderByDescending(o => o.Coin.TotalDollarValue).ToList();

            int initCount = Coins.Count - 1;

            for (int i = initCount; i >= 0; i--)
            {
                if (i + 1 <= initCount)
                {
                    Coins[i].CurrentTimeLineDollarValue += Coins[i + 1].CurrentTimeLineDollarValue + Coins[i].Coin.TotalDollarValue;
                }
            }
        }
    }
}
