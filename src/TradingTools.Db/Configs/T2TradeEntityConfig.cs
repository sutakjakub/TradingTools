using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingTools.Db.Entities;

namespace TradingTools.Db.Configs
{
    public class T2TradeEntityConfig : EntityConfiguration<T2TradeEntity>
    {
        public override void Configure(EntityTypeBuilder<T2TradeEntity> builder)
        {
            //Configure base
            base.Configure(builder);

            builder.Property(p => p.Id)
                .HasComment("PK, Identity")
                .HasColumnName($"{nameof(T2TradeEntity)}_ID");

            builder.Property(p => p.Price)
                 .HasPrecision(18, 8);
            builder.Property(p => p.Quantity)
                .HasPrecision(18, 8);
            builder.Property(p => p.QuoteQuantity)
                .HasPrecision(18, 8);
            builder.Property(p => p.Commission)
                .HasPrecision(18, 8);
            builder.Property(p => p.QuoteUsdValue)
                .HasPrecision(18, 8);
            builder.Property(p => p.CommissionUsdValue)
               .HasPrecision(18, 8);
        }
    }
}
