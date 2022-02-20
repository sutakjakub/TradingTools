using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingTools.Taxes.Models
{
    public class TaxDataRoot
    {
        public string AssetName { get; private set; }
        public DateTimeOffset WhenRealizeProfit { get; private set; }
        public TaxDataItemBase SellCommision { get; private set; }
        public IList<TaxDataItem> BuyItems { get; private set; }

        protected TaxDataRoot()
        {
            BuyItems = new List<TaxDataItem>(); 
        }
        
        public static TaxDataRoot Create(Element sellElement, IList<Element> buyElement)
        {
            var root = new TaxDataRoot
            {
                AssetName = sellElement.AssetName,
                WhenRealizeProfit = sellElement.TradeDateTime,
                SellCommision = new TaxDataItemBase
                {
                    Amount = sellElement.CommisionAmount,
                    AssetName = sellElement.CommisionAsset
                }
                
            };
            root.BuyItems = new List<TaxDataItem>();
            foreach (var item in buyElement)
            {
                root.BuyItems.Add(new TaxDataItem 
                { 
                    Amount = item.Amount,
                    AssetName = item.AssetName,
                    Price = item.Price,
                    When = item.TradeDateTime,
                    Commision = new TaxDataItemBase 
                    {
                        Amount = item.CommisionAmount,
                        AssetName = item.AssetName
                    },

                });
            }

            return root;
        }
    }

    public class TaxDataItem : TaxDataItemBase
    {
        public decimal Price { get; set; }
        public DateTimeOffset When { get; set; }
        public TaxDataItemBase Commision { get; set; }
    }

    public class TaxDataItemBase
    {
        public string AssetName { get; set; }
        public decimal Amount { get; set; }
    }
}
