using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using TradingTools.Db.Entities;
using TradingTools.Taxes.Models;
using TradingTools.Taxes.Strategies;

namespace TradingTools.Taxes
{
    public class TaxReport
    {
        private readonly TaxCalculator _taxCalculator;

        public TaxReport(TaxCalculator taxCalculator)
        {
            _taxCalculator = taxCalculator ?? throw new ArgumentNullException(nameof(taxCalculator));
        }

        public void Load(IEnumerable<T2TradeEntity> trades)
        {
            _taxCalculator.Calculate(trades);
        }

        public void Generate(GenerateType type)
        {
            if (type == GenerateType.Csv)
            {
                using (var writer = new StreamWriter($"{DateTime.Now.Year}_{_taxCalculator.Strategy.UserFriendlyName}_crypto_gains.csv"))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(_taxCalculator.TaxReportItems);
                }

                using (var writer = new StreamWriter($"{DateTime.Now.Year}_{_taxCalculator.Strategy.UserFriendlyName}_crypto_transactions.csv"))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(_taxCalculator.Source);
                }
            }
        }
    }
}
