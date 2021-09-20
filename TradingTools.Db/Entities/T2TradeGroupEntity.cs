using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingTools.Db.Entities
{
    /// <summary>
    /// Trade group entity.
    /// </summary>
    public class T2TradeGroupEntity : Entity<long>
    {
        /// <summary>
        /// Name of the group
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Trades belongs to group
        /// </summary>
        public IList<T2TradeEntity> Trades { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// T2SymbolInfo
        /// </summary>
        public T2SymbolInfoEntity SymbolInfo { get; set; }

        /// <summary>
        /// Identification of SymbolInfo
        /// </summary>
        public long? SymbolInfoId { get; set; }

        /// <summary>
        /// Base asset
        /// </summary>
        public string BaseAsset { get; set; }
    }
}
