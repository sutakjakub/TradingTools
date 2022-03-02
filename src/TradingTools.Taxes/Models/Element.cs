using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingTools.Db.Entities;

namespace TradingTools.Taxes.Models
{
    public class Element : ICloneable
    {
        public long T2TradeEntityId { get; set; }
        public string AssetName { get; set; }
        public string QuoteAssetName { get; set; }
        public decimal QuoteAssetUsdValue { get; set; }
        public decimal Price { get; set; }
        public bool IsBuyer { get; set; }
        public decimal Amount { get; set; }
        public DateTimeOffset TradeDateTime { get; set; }
        public string CommisionAsset { get; set; }
        public decimal CommisionAmount { get; set; }
        public decimal CommisionUsdValue { get; set; }

        public object Clone()
        {
            return (Element)MemberwiseClone();
        }
    }

    public class RootElement : ICloneable
    {
        public IList<Element> Items { get; set; }
        public string Symbol => Items?.FirstOrDefault()?.AssetName;

        public RootElement()
        {
            Items = new List<Element>();
        }

        public object Clone()
        {
            var root = (RootElement)MemberwiseClone();
            root.Items = new List<Element>();
            foreach (var item in Items)
            {
                root.Items.Add((Element)item.Clone());
            }

            return root;
        }
    }
}
