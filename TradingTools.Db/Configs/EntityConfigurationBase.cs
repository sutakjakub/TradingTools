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
    public abstract class EntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
        where TEntity : Entity
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.Property(p => p.Timestamp)
                .IsRowVersion()
                .IsConcurrencyToken();

            builder.Property(p => p.Created)
                .HasComment("Represents UTC date time of entity creation")
                .HasColumnType("datetimeoffset(2)");

            builder.Property(p => p.Updated)
                .HasComment("Represents UTC date time of latest entity version")
                .HasColumnType("datetimeoffset(2)");
        }
    }
}
