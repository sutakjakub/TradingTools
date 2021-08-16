﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingTools.Shared.Enums
{
    /// <summary>
    /// The time the order will be active for
    /// </summary>
    public enum T2TimeInForce
    {
        /// <summary>
        /// GoodTillCancel orders will stay active until they are filled or canceled
        /// </summary>
        GoodTillCancel,
        /// <summary>
        /// ImmediateOrCancel orders have to be at least partially filled upon placing or will be automatically canceled
        /// </summary>
        ImmediateOrCancel,
        /// <summary>
        /// FillOrKill orders have to be entirely filled upon placing or will be automatically canceled
        /// </summary>
        FillOrKill,
        /// <summary>
        /// GoodTillCrossing orders will post only
        /// </summary>
        GoodTillCrossing,
        /// <summary>
        /// Good til the order expires or is canceled
        /// </summary>
        GoodTillExpiredOrCanceled
    }
}
