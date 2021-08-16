using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TradingTools.Db.Entities;

namespace TradingTools.Db
{
    /// <summary>
    /// DB context.
    /// </summary>
    public class TradingToolsDbContext : DbContext
    {
        public TradingToolsDbContext(DbContextOptions<TradingToolsDbContext> options)
                    : base(options)
        {
        }

        public DbSet<T2OrderEntity> T2Orders { get; set; }
        public DbSet<T2SymbolInfoEntity> T2SymbolInfos { get; set; }
        public DbSet<T2TradeEntity> T2Trades { get; set; }

        public override int SaveChanges()
        {
            SetCreatedUpdated();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            SetCreatedUpdated();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }

        private void SetCreatedUpdated()
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is Entity
                            && (e.State == EntityState.Added
                                || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                var timeOffset = DateTimeOffset.UtcNow;
                ((Entity)entityEntry.Entity).Updated = timeOffset;

                if (entityEntry.State == EntityState.Added)
                {
                    ((Entity)entityEntry.Entity).Created = timeOffset;
                }
            }
        }
    }
}
