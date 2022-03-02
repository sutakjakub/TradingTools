using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingTools.MathLib.Models
{
    public class TotalGainResult
    {
        public decimal TotalPurchase { get; set; }
        public decimal NumberOfShares { get; set; }
        public decimal AverageCost { get; set; }
        public decimal NetProceeds { get; set; }
        public decimal CostBasis { get; set; }
        public decimal GainLoss { get; set; }
    }
}
