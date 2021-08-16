using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingTools.Shared.Enums;

namespace TradingTools.Db.Entities
{
    /// <summary>
    /// The T2Trade class.
    /// </summary>
    public class T2TradeEntity : Entity<long>
    {
        /// <summary>
        /// Unique identification of T2Trade
        /// </summary>
        public long T2TradeId { get; set; }
        /// <summary>
        /// The T2OrderId the trade belongs to
        /// </summary>
        public long T2OrderId { get; set; }
        /// <summary>
        /// The T2Order entity
        /// </summary>
        public T2OrderEntity T2Order { get; set; }

        /// <summary>
        /// Exchange type
        /// </summary>
        public T2ExchangeType ExchangeType { get; set; }
        /// <summary>
        /// The symbol
        /// </summary>
        public string Symbol { get; set; }
        /// <summary>
        /// The id of the trade
        /// </summary>
        public long TradeId { get; set; }
        /// <summary>
        /// The order id the trade belongs to
        /// </summary>
        public long OrderId { get; set; }
        /// <summary>
        /// The price of the trade
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// The quantity of the trade
        /// </summary>
        public decimal Quantity { get; set; }
        /// <summary>
        /// The quote quantity of the trade
        /// </summary>
        public decimal QuoteQuantity { get; set; }
        /// <summary>
        /// The commission paid for the trade
        /// </summary>
        public decimal Commission { get; set; }
        /// <summary>
        /// The asset the commission is paid in
        /// </summary>
        public string CommissionAsset { get; set; }
        /// <summary>
        /// The time the trade was made
        /// </summary>
        public DateTime TradeTime { get; set; }
    }
}
