using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingTools.Taxes
{
    public class TaxReportItem
    {
        public decimal AssetAmount { get; set; }
        public string AssetName { get; set; }
        public DateTimeOffset ReceivedDate { get; set; }
        public DateTimeOffset DateSold { get; set; }
        public decimal Proceeds { get; set; }
        public decimal CostBasis { get; set; }
        public decimal Gain { get; set; }

        public static TaxReportItem Create(
            decimal amount, 
            string assetName, 
            DateTimeOffset receivedDate, 
            DateTimeOffset dateSold,
            decimal buyPrice,
            decimal sellPrice,
            decimal comission,
            decimal usdValue)
        {
            buyPrice *= usdValue;
            sellPrice *= usdValue;

            //https://www.investopedia.com/terms/c/costbasis.asp
            //First in first out: ($19 - $20) x 1,000 shares = - $1,000
            //$19 is sell price
            var costBasis = (sellPrice - buyPrice) * amount;

            var proceeds = buyPrice * amount; //minus comission
            var gain = proceeds - costBasis;

            return new TaxReportItem
            {
                AssetAmount = amount,
                AssetName = assetName,
                CostBasis = costBasis,
                DateSold = dateSold,
                ReceivedDate = receivedDate,
                Gain = gain,
                Proceeds = proceeds
            };
        }
    }
}
