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
                        try
                        {
                            //sync portfolio
                            await scopeData.Synchronizator.SyncPortfolio();
                            //get lastr portfolio
                            var portfolio = await scopeData.Portfolio.FindLastPortfolio();
                            //get coins - base assets
                            var portfolioBaseAssets = portfolio.Select(s => s.Coin).ToList();
                            var dbSymbols = await scopeData.SymbolQuery.All();

                            var allBtcUsdtSymbols =
                               dbSymbols.Where(p => p.QuoteAsset == "BTC" || p.QuoteAsset == "USDT").OrderBy(o => o.BaseAsset).Select(s => s.Symbol).ToList();
                            var items = FilterSymbols(portfolioBaseAssets, allBtcUsdtSymbols);
                            
                            int count = items.Count();
                            var i = 1;
                            foreach (var symbol in items)
                            {

                                await Task.Delay(300);
                                _ = await scopeData.Synchronizator.SyncBySymbol(symbol);

                                var percentage = $"{Math.Round((decimal)(100 * i / count), 2)}%";
                                await scopeData.Hub.Clients.All.SendAsync("ReceiveMessage", $"{i}/{count} ({percentage}) syncing", symbol);
                                i++;

                            }
                        }
                        catch (Exception ex)
                        {

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

        private static IEnumerable<string> FilterSymbols(List<string> portfolioBaseAssets, List<string> allBtcUsdtSymbols)
        {
            var collection = new List<string>();
            foreach (var baseAsset in portfolioBaseAssets)
            {
                foreach (var symbol in allBtcUsdtSymbols)
                {
                    if (symbol.StartsWith(baseAsset))
                    {
                        collection.Add(symbol);
                    }
                }
            }

            return collection.Concat(allBtcUsdtSymbols.Except(collection));
        }
    }
}
