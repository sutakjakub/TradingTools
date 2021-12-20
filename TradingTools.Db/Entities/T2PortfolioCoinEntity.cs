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
        /// The identifier synchronization
        /// </summary>
        public long T2SyncId { get; set; }
        /// <summary>
        /// T2Sync entity
        /// </summary>
        public T2SyncEntity T2Sync { get; set; }

        /// <summary>
        /// The identifier for symbol info.
        /// </summary>
        public long T2SymbolInfoId { get; set; }
        /// <summary>
        /// T2SymbolInfo entity
        /// </summary>
        public T2SymbolInfoEntity T2SymbolInfo { get; set; }

        /// <summary>
        /// Free amount
        /// </summary>
        public decimal Free { get; set; }

        /// <summary>
        /// Locked in orders amount
        /// </summary>
        public decimal Locked { get; set; }
    }
}
