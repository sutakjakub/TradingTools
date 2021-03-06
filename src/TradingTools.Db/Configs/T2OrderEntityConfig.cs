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
    public class T2OrderEntityConfig : EntityConfiguration<T2OrderEntity>
    {
        public override void Configure(EntityTypeBuilder<T2OrderEntity> builder)
        {
            //Configure base
            base.Configure(builder);

            builder.Property(p => p.Id)
                .HasComment("PK, Identity")
                .HasColumnName($"{nameof(T2OrderEntity)}_ID");

            builder.Property(p => p.StopPrice)
                 .HasPrecision(18, 8);
            builder.Property(p => p.QuoteQuantity)
                .HasPrecision(18, 8);
            builder.Property(p => p.QuoteQuantityFilled)
                .HasPrecision(18, 8);
            builder.Property(p => p.QuantityFilled)
                .HasPrecision(18, 8);
            builder.Property(p => p.Quantity)
               .HasPrecision(18, 8);
            builder.Property(p => p.Price)
               .HasPrecision(18, 8);
            builder.Property(p => p.QuantityRemaining)
               .HasPrecision(18, 8);
            builder.Property(p => p.AverageFillPrice)
               .HasPrecision(18, 8);

            builder
                .HasOne(p => p.T2TradeGroup)
                .WithMany(s => s.Orders)
                .HasForeignKey(p => p.T2TradeGroupId)
                .IsRequired(false);
        }
    }
}
