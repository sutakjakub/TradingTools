using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingTools.Db.Entities;
using TradingTools.Taxes.Models;

namespace TradingTools.Taxes
{
    public static class Converter
    {
        public static Element Convert(T2TradeEntity entity)
        {
            return new Element
            {
                Amount = entity.Quantity,
                TradeDateTime = entity.TradeTime,
                IsBuyer = entity.IsBuyer,
                Price = entity.Price,
                T2TradeEntityId = entity.Id,
                AssetName = entity.T2SymbolInfo.BaseAsset,
                QuoteAssetName = entity.T2SymbolInfo.QuoteAsset,
                QuoteAssetUsdValue = entity.QuoteUsdValue,
                CommisionAmount = entity.Commission,
                CommisionAsset = entity.CommissionAsset,
                CommisionUsdValue = entity.CommissionUsdValue
            };
        }

        public static IEnumerable<Element> Convert(IEnumerable<T2TradeEntity> entities)
        {
            foreach (var item in entities)
            {
                yield return Convert(item);
            }
        }
    }
}
