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
            decimal sellPrice)
        {
            return new TaxReportItem();
        }
    }
}
