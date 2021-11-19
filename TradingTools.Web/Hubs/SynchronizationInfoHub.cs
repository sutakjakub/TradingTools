using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradingTools.Core.Synchronization.Interfaces;
using TradingTools.Persistence.Queries.Interfaces;
using TradingTools.Persistence.Stores.Interfaces;

namespace TradingTools.Web.Hubs
{
    public class SynchronizationInfoHub : Hub
    {
        private readonly IT2Synchronizator _synchronizator;
        private readonly IT2SymbolInfoQuery _symbolQuery;
        private readonly IT2SyncQuery _syncQuery;
        private readonly IT2SyncStore _syncStore;

        public SynchronizationInfoHub(IT2Synchronizator synchronizator, IT2SymbolInfoQuery symbolQuery, IT2SyncQuery syncQuery, IT2SyncStore syncStore)
        {
            _synchronizator = synchronizator;
            _symbolQuery = symbolQuery;
            _syncQuery = syncQuery;
            _syncStore = syncStore;
        }

        public async Task StartSync()
        {
            var syncEntity = await _syncQuery.FindLast();
            if (syncEntity == null || syncEntity.State == Db.Enums.SyncState.Done)
            {
                syncEntity = new Db.Entities.T2SyncEntity
                {
                    State = Db.Enums.SyncState.NotRunYet
                };

                await _syncStore.Create(syncEntity);
            }
        }
    }
}
