using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingTools.Shared.Enums;

namespace TradingTools.Db.Entities
{
    /// <summary>
    /// The T2SymbolInfo class.
    /// </summary>
    public class T2SymbolInfoEntity : Entity<long>
    {
        /// <summary>
        /// The exchange type
        /// </summary>
        public T2ExchangeType ExchangeType { get; set; }
        /// <summary>
        /// The symbol
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// Base asset
        /// </summary>
        public string BaseAsset { get; set; }
        /// <summary>
        /// The precission of base asset
        /// </summary>
        public int BaseAssetPrecision { get; set; }
        /// <summary>
        /// The preccission of commision base asset
        /// </summary>
        public int BaseAssetCommissionPrecision { get; set; }

        /// <summary>
        /// The quote asset
        /// </summary>
        public string QuoteAsset { get; set; }
        /// <summary>
        /// The preccisssion of quote asset
        /// </summary>
        public int QuoteAssetPrecision { get; set; }
        /// <summary>
        /// The preccission of commision quote asset
        /// </summary>
        public int QuoteAssetCommissionPrecision { get; set; }
    }
}
