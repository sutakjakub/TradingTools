using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingTools.Persistence.Queries;
using TradingTools.Persistence.Queries.Interfaces;
using TradingTools.Persistence.Stores;
using TradingTools.Persistence.Stores.Interfaces;

namespace TradingTools.Persistence
{
    public static class PersistenceRegistrator
    {
        public static void RegisterEverything(IServiceCollection services)
        {
            services.AddTransient<IT2TradeQuery, T2TradeQuery>();
            services.AddTransient<IT2TradeGroupQuery, T2TradeGroupQuery>();
            services.AddTransient<IT2SymbolInfoQuery, T2SymbolInfoQuery>();
            services.AddTransient<IT2SyncQuery, T2SyncQuery>();
            services.AddTransient<IT2OrderQuery, T2OrderQuery>();

            services.AddTransient<IT2TradeGroupStore, T2TradeGroupStore>();
            services.AddTransient<IT2TradeStore, T2TradeStore>();
            services.AddTransient<IT2SyncStore, T2SyncStore>();
            services.AddTransient<IT2OrderStore, T2OrderStore>();

            services.AddTransient<IT2PortfolioCoinStore, T2PortfolioCoinStore>();
            services.AddTransient<IT2NotificationStore, T2NotificationStore>();
        }
    }
}
