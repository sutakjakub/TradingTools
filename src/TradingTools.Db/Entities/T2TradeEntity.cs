using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingTools.Db.Enums;
using TradingTools.Shared.Enums;

namespace TradingTools.Db.Entities
{
    /// <summary>
    /// The T2Trade class.
    /// </summary>
    [DebuggerDisplay("Symbol = {Symbol} ; IsBuyer = {IsBuyer} ; Price = {Price} ; Quantity = {Quantity} ; QuoteQuantity = {QuoteQuantity}")]
    public class T2TradeEntity : Entity<long>
    {
        /// <summary>
        /// Unique identification of T2Trade
        /// </summary>
        public long T2TradeId { get; set; }
        /// <summary>
        /// The T2OrderId the trade belongs to
        /// </summary>
        public long? T2OrderId { get; set; }
        /// <summary>
        /// The T2Order entity
        /// </summary>
        public T2OrderEntity T2Order { get; set; }
        /// <summary>
        /// The T2SymbolInfo entity
        /// </summary>
        public T2SymbolInfoEntity T2SymbolInfo { get; set; }
        /// <summary>
        /// The T2SymbolInfo ID
        /// </summary>
        public long? T2SymbolInfoId { get; set; }
        /// <summary>
        /// The T2TradeGroup entity
        /// </summary>
        public T2TradeGroupEntity T2TradeGroup { get; set; }
        /// <summary>
        /// The T2TradeGroup ID
        /// </summary>
        public long? T2TradeGroupId { get; set; }

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
        /// Dollar value of 1 quote asset at trade time
        /// </summary>
        public decimal QuoteUsdValue { get; set; }
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

        /// <summary>
        /// Whether account was the buyer in the trade
        /// </summary>
        public bool IsBuyer { get; set; }
        /// <summary>
        /// Whether account was the maker in the trade
        /// </summary>
        public bool IsMaker { get; set; }
        /// <summary>
        /// Whether trade was made with the best match
        /// </summary>
        public bool IsBestMatch { get; set; }

        /// <summary>
        /// Trade state
        /// </summary>
        public TradeState TradeState { get; set; }
    }
}
