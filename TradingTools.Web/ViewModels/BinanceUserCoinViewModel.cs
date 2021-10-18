using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradingTools.Db.Entities;
using TradingTools.Shared.Dto;

namespace TradingTools.Web.ViewModels
{
    public class BinanceUserCoinViewModel
    {
        public BinanceUserCoinViewModel()
        {

        }

        public BinanceUserCoinDto Coin { get; private set; }

        public decimal TotalDollarValue { get; private set; }
        public string TotalDollarAssetName { get; private set; }
        public decimal TotalQuoteValue { get; private set; }
        public string TotalQuoteAssetName { get; private set; }

        public void Init(BinanceUserCoinDto coinDto, ExchangePriceDto priceDto, T2SymbolInfoEntity symbolInfo, decimal priceBtcUsdt)
        {
            Coin = coinDto;
            TotalQuoteValue = (coinDto.Free + coinDto.Locked) * priceDto?.Price ?? 0;
            TotalQuoteAssetName = symbolInfo?.QuoteAsset;
            TotalDollarAssetName = "USDT";

            if (TotalQuoteAssetName == "USDT")
            {
                TotalQuoteValue = Math.Round(TotalQuoteValue, 2);
                TotalDollarValue = TotalQuoteValue;
            }
            else if (TotalQuoteAssetName == "BTC")
            {
                TotalQuoteValue = Math.Round(TotalQuoteValue, 8);
                TotalDollarValue = Math.Round(TotalQuoteValue * priceBtcUsdt, 2);
            }
            else
            {
                TotalDollarValue = 0;
            }
        }
    }
}
