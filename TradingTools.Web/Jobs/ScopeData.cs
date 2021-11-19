using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradingTools.Core.Synchronization.Interfaces;
using TradingTools.Persistence.Queries.Interfaces;
using TradingTools.Persistence.Stores.Interfaces;
using TradingTools.Web.Hubs;

namespace TradingTools.Web.Jobs
{
    public class ScopeData
    {
        public ScopeData(
            IT2Synchronizator synchronizator,
            IT2SymbolInfoQuery symbolQuery,
            IHubContext<SynchronizationInfoHub> hub,
            IT2SyncQuery syncQuery,
            IT2SyncStore syncStore)
        {
            Synchronizator = synchronizator;
            SymbolQuery = symbolQuery;
            Hub = hub;
            SyncQuery = syncQuery;
            SyncStore = syncStore;
        }

        public IT2Synchronizator Synchronizator { get; }
        public IT2SymbolInfoQuery SymbolQuery { get; }
        public IHubContext<SynchronizationInfoHub> Hub { get; }
        public IT2SyncQuery SyncQuery { get; }
        public IT2SyncStore SyncStore { get; }
    }
}
