using System;
using System.Collections.Generic;
using System.Linq;
using TradingTools.Db.Entities;
using TradingTools.Taxes.Models;
using TradingTools.Taxes.Strategies;

namespace TradingTools.Taxes
{
    public class TaxReport
    {
        private readonly TaxStrategy _strategy;

        public TaxReport(TaxStrategy strategy)
        {
            _strategy = strategy ?? throw new ArgumentNullException(nameof(strategy));
        }

        public IList<TaxReportItem> Items { get; private set; }

        public void Load(IEnumerable<T2TradeEntity> trades)
        {
           
        }
    }
}
