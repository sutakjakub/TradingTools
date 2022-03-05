using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingTools.MathLib;
using TradingTools.Taxes.Models;

namespace TradingTools.Taxes
{
    public class TaxReportItem
    {
        
        [Name("Asset Name")]
        public string AssetName { get; set; }
        [Name("Asset Amount")]
        public decimal AssetAmount { get; set; }

        [Name("Gain USD")]
        public decimal GainUsdValue { get; set; }
        [Name("Received Date")]
        public DateTime ReceivedDate { get; set; }
        [Name("Sold Date")]
        public DateTime DateSold { get; set; }
        [Name("Proceeds")]
        public decimal Proceeds { get; set; }
        [Name("Cost Basis")]
        public decimal CostBasis { get; set; }
        [Name("Commision USD")]
        public decimal CommisionUsdValue { get; set; }
        [Name("Term Type")]
        public string TermType { get; set; }

        public static TaxReportItem CreateBuyItem(
            decimal amount, 
            string assetName,
            DateTime receivedDate, 
            DateTime dateSold,
            decimal buyPrice,
            decimal sellPrice,
            decimal totalCommisionInUsd)
        {
            //buyPrice *= usdValue;
            //sellPrice *= usdValue;

            //https://www.investopedia.com/terms/c/costbasis.asp
            //First in first out: ($19 - $20) x 1,000 shares = - $1,000
            //$19 is sell price
            //var costBasis = (sellPrice - buyPrice) * amount;

            //var proceeds = buyPrice * amount; //minus comission
            //var gain = proceeds - costBasis;

            var gainResult = BasicCalculator.TotalGain(
                new List<(decimal quantity, decimal price)> { (amount, buyPrice) },
                new List<(decimal quantity, decimal price)> { (amount, sellPrice) },
                totalCommisionInUsd,
                8);

            return new TaxReportItem
            {
                AssetAmount = amount,
                AssetName = assetName,
                CostBasis = gainResult.CostBasis,
                DateSold = dateSold,
                ReceivedDate = receivedDate,
                GainUsdValue = gainResult.GainLoss,
                Proceeds = gainResult.NetProceeds,
                CommisionUsdValue = totalCommisionInUsd,
                TermType = "Short Term"
            };
        }

        //public static TaxReportItem CreateSellItem(
        //    decimal amount,
        //    string assetName,
        //    DateTimeOffset dateSold,
        //    decimal sellPrice,
        //    decimal comission,
        //    string commissionAssetName
        //    )
        //{
        //    return new TaxReportItem
        //    {
        //        AssetAmount = amount,
        //        AssetPrice = sellPrice,
        //        AssetName = assetName,
        //        DateSold = dateSold,
        //        TermType = "Short Term",
        //        TransactionType = "Sell",
        //        CommisionAmount = comission,
        //        CommisionAssetName = commissionAssetName
        //    };
        //}

        //public static TaxReportItem Create(TaxDataRoot root)
        //{
        //    buyPrice *= usdValue;
        //    sellPrice *= usdValue;

        //    //https://www.investopedia.com/terms/c/costbasis.asp
        //    //First in first out: ($19 - $20) x 1,000 shares = - $1,000
        //    //$19 is sell price
        //    var costBasis = (sellPrice - buyPrice) * amount;

        //    var proceeds = buyPrice * amount; //minus comission
        //    var gain = proceeds - costBasis;

        //    return new TaxReportItem
        //    {
        //        AssetAmount = amount,
        //        AssetName = assetName,
        //        CostBasis = costBasis,
        //        DateSold = dateSold,
        //        ReceivedDate = receivedDate,
        //        Gain = gain,
        //        Proceeds = proceeds
        //    };
        //}
    }
}
