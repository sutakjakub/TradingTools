using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingTools.Taxes.Models
{
    public class TaxDataRoot
    {
        public string AssetName => SellItem?.AssetName;
        public DateTimeOffset WhenRealizeProfit => SellItem.When;
        public IList<TaxDataItem> BuyItems { get; private set; }
        public TaxDataItem SellItem { get; private set; }

        protected TaxDataRoot()
        {
            BuyItems = new List<TaxDataItem>();
        }

        public static TaxDataRoot Create(Element sellElement, IList<Element> buyElement)
        {
            var root = new TaxDataRoot
            {
                SellItem = new TaxDataItem
                {
                    Id = sellElement.T2TradeEntityId,
                    Amount = sellElement.Amount,
                    AssetName = sellElement.AssetName,
                    AssetUsdValue = sellElement.QuoteAssetUsdValue,
                    Price = sellElement.Price,
                    When = sellElement.TradeDateTime,
                    Commision = new TaxDataItemBase
                    {
                        Amount = sellElement.CommisionAmount,
                        AssetName = sellElement.CommisionAsset,
                        AssetUsdValue = sellElement.CommisionUsdValue
                    }
                }
            };
            root.BuyItems = new List<TaxDataItem>();
            foreach (var item in buyElement)
            {
                root.BuyItems.Add(new TaxDataItem
                {
                    Id = item.T2TradeEntityId,
                    Amount = item.Amount,
                    AssetName = item.AssetName,
                    QuoteAssetName = item.QuoteAssetName,
                    AssetUsdValue = item.QuoteAssetUsdValue,
                    Price = item.Price,
                    When = item.TradeDateTime,
                    Commision = new TaxDataItemBase
                    {
                        Amount = item.CommisionAmount,
                        AssetName = item.CommisionAsset,
                        AssetUsdValue = item.CommisionUsdValue
                    }
                });
            }

            return root;
        }
    }

    public class TaxDataItem : TaxDataItemBase
    {
        public long Id { get; set; }
        public decimal Price { get; set; }
        public string QuoteAssetName { get; set; }
        public DateTimeOffset When { get; set; }
        public TaxDataItemBase Commision { get; set; }
    }

    public class TaxDataItemBase
    {
        public string AssetName { get; set; }
        public decimal Amount { get; set; }
        public decimal AssetUsdValue { get; set; }
    }
}
