using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TradingTools.Core.Synchronization.Interfaces;
using TradingTools.Db.Enums;
using TradingTools.Persistence.Queries.Interfaces;
using TradingTools.Persistence.Stores.Interfaces;
using TradingTools.Web.Hubs;

namespace TradingTools.Web.Jobs
{
    public class SyncProcessorJob : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public SyncProcessorJob(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope()) // this will use `IServiceScopeFactory` internally
                {
                    var scopeData = scope.ServiceProvider.GetService<ScopeData>();

                    var syncEntity = await scopeData.SyncQuery.FindLast();
                    //if (syncEntity == null || syncEntity.State == Db.Enums.SyncState.Done)
                    //{
                    //    syncEntity = new Db.Entities.T2SyncEntity
                    //    {
                    //        State = Db.Enums.SyncState.NotRunYet
                    //    };
                    //    await _syncStore.Create(syncEntity);
                    //}

                    if (syncEntity != null && syncEntity.State == SyncState.NotRunYet)
                    {
                        syncEntity.State = SyncState.InProgress;
                        syncEntity.StartDate = DateTime.Now;
                        await scopeData.SyncStore.Update(syncEntity);

                        var dbSymbols = await scopeData.SymbolQuery.All();
                        var items = dbSymbols.Where(p => p.QuoteAsset == "BTC" || p.QuoteAsset == "USDT").OrderBy(o => o.BaseAsset).ToList();
                        var i = 1;
                        foreach (var item in items)
                        {
                            try
                            {
                                await Task.Delay(300);
                                _ = await scopeData.Synchronizator.SyncBySymbol(item.Symbol);

                                var percentage = $"{Math.Round((decimal)(100 * i / items.Count), 2)}%";
                                await scopeData.Hub.Clients.All.SendAsync("ReceiveMessage", $"{i}/{items.Count} ({percentage}) syncing", item.Symbol);
                                i++;
                            }
                            catch (Exception ex)
                            {

                            }
                        }

                        await scopeData.Synchronizator.SyncOpenOrdersBySymbol();

                        syncEntity.State = SyncState.Done;
                        syncEntity.EndDate = DateTime.Now;
                        await scopeData.SyncStore.Update(syncEntity);
                    }
                }

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
