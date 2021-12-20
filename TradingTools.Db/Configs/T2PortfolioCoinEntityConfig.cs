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
    public class T2PortfolioCoinEntityConfig : EntityConfiguration<T2PortfolioCoinEntity>
    {
        public override void Configure(EntityTypeBuilder<T2PortfolioCoinEntity> builder)
        {
            //Configure base
            base.Configure(builder);

            builder.Property(p => p.Id)
                .HasComment("PK, Identity")
                .HasColumnName($"{nameof(T2PortfolioCoinEntity)}_ID");

            builder.Property(p => p.Free)
                .HasPrecision(18, 8);
            builder.Property(p => p.Locked)
                .HasPrecision(18, 8);
        }
    }
}
