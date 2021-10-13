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
            services.AddScoped<IT2TradeQuery, T2TradeQuery>();
            services.AddScoped<IT2TradeGroupQuery, T2TradeGroupQuery>();
            services.AddScoped<IT2SymbolInfoQuery, T2SymbolInfoQuery>();

            services.AddScoped<IT2TradeGroupStore, T2TradeGroupStore>();
            services.AddScoped<IT2TradeStore, T2TradeStore>();
        }
    }
}
