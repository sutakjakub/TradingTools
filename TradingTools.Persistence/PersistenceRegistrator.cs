using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingTools.Persistence.Queries;
using TradingTools.Persistence.Queries.Interface;

namespace TradingTools.Persistence
{
    public static class PersistenceRegistrator
    {
        public static void RegisterEverything(IServiceCollection services)
        {
            services.AddTransient<IT2TradeQuery, T2TradeQuery>();
        }
    }
}
