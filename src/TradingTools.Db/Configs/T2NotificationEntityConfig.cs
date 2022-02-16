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
    public class T2NotificationEntityConfig : EntityConfiguration<T2NotificationEntity>
    {
        public override void Configure(EntityTypeBuilder<T2NotificationEntity> builder)
        {
            //Configure base
            base.Configure(builder);

            builder.Property(p => p.Id)
                .HasComment("PK, Identity")
                .HasColumnName($"{nameof(T2NotificationEntity)}_ID");
        }
    }
}
