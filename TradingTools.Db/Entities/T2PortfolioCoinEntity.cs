using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingTools.Db.Entities
{
    public class T2PortfolioCoinEntity : Entity<long>
    {
        /// <summary>
        /// Free amount
        /// </summary>
        public decimal Free { get; set; }
        /// <summary>
        /// Locked in orders amount
        /// </summary>
        public decimal Locked { get; set; }
        /// <summary>
        /// Portfolio version
        /// </summary>
        public int Version { get; set; }
        /// <summary>
        /// Coin
        /// </summary>
        public string Coin { get; set; }
        /// <summary>
        /// Name of the coin
        /// </summary>
        public string CoinName { get; set; }
        /// <summary>
        /// Total Dollar Value
        /// </summary>
        public decimal TotalDollarValue { get; set; }
        /// <summary>
        /// Total Dollar asset name
        /// </summary>
        public string TotalDollarAssetName { get; set; }
        /// <summary>
        /// TotalQuoteValue
        /// </summary>
        public decimal TotalQuoteValue { get; set; }
        /// <summary>
        /// TotalQuoteAssetName
        /// </summary>
        public string TotalQuoteAssetName { get; set; }
    }
}
