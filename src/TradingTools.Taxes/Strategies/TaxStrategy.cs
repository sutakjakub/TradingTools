using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingTools.Db.Entities;
using TradingTools.Taxes.Models;

namespace TradingTools.Taxes.Strategies
{
    public abstract class TaxStrategy
    {
        public IList<TaxDataRoot> TaxData { get; protected set; }
        protected List<RootElement> Roots { get; private set; }

        public abstract void Process();
        public void Load(IEnumerable<RootElement> roots)
        {
            Roots = new List<RootElement>();
            foreach (var root in roots)
            {
                Roots.Add((RootElement)root.Clone());
            }
        }
        public abstract void Generate(GenerateType type);
    }
}
