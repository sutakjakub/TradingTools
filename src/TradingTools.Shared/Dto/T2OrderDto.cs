﻿using System;
using TradingTools.Shared.Enums;

namespace TradingTools.Shared.Dto
{
    /// <summary>
    /// The Order class.
    /// </summary>
    public class T2OrderDto
    {
        /// <summary>
        /// Unique identification of T2Order
        /// </summary>
        public long T2OrderId { get; set; }

        /// <summary>
        /// Is working
        /// </summary>
        public bool? IsWorking { get; set; }
        /// <summary>
        /// The time the order was last updated
        /// </summary>
        public DateTime? UpdateTime { get; set; }
        /// <summary>
        /// The time the order was submitted
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// The stop price
        /// </summary>
        public decimal? StopPrice { get; set; }
        /// <summary>
        /// The side of the order
        /// </summary>
        public T2OrderSide Side { get; set; }
        /// <summary>
        /// The type of the order
        /// </summary>
        public T2OrderType Type { get; set; }
        /// <summary>
        /// How long the order is active
        /// </summary>
        public T2TimeInForce TimeInForce { get; set; }
        /// <summary>
        /// The status of the order
        /// </summary>
        public T2OrderStatus Status { get; set; }
        /// <summary>
        /// The original quote order quantity
        /// </summary>
        public decimal QuoteQuantity { get; set; }
        /// <summary>
        /// Cummulative amount
        /// </summary>
        public decimal QuoteQuantityFilled { get; set; }
        /// <summary>
        /// The currently executed quantity of the order
        /// </summary>
        public decimal QuantityFilled { get; set; }
        /// <summary>
        /// The original quantity of the order
        /// </summary>
        public decimal Quantity { get; set; }
        /// <summary>
        /// The price of the order
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// The order id as assigned by the client
        /// </summary>
        public string ClientOrderId { get; set; }
        /// <summary>
        /// Original order id
        /// </summary>
        public string OriginalClientOrderId { get; set; }
        /// <summary>
        /// Id of the order list this order belongs to
        /// </summary>
        public long OrderListId { get; set; }
        /// <summary>
        /// The order id generated by Binance
        /// </summary>
        public long OrderId { get; set; }
        /// <summary>
        /// The symbol the order is for
        /// </summary>
        public string Symbol { get; set; }
        /// <summary>
        /// Quantity which is still open to be filled
        /// </summary>
        public decimal QuantityRemaining { get; set; }
        /// <summary>
        /// The average price the order was filled
        /// </summary>
        public decimal? AverageFillPrice { get; set; }
    }
}