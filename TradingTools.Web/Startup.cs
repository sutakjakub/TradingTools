using Binance.Net;
using Binance.Net.Interfaces;
using Binance.Net.Objects;
using CryptoExchange.Net.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradingTools.Core.Business;
using TradingTools.Core.Business.Interfaces;
using TradingTools.Core.Synchronization;
using TradingTools.Core.Synchronization.Interfaces;
using TradingTools.Db;
using TradingTools.ExchangeServices;
using TradingTools.ExchangeServices.Interfaces;
using TradingTools.Persistence;
using TradingTools.Persistence.Queries;
using TradingTools.Persistence.Queries.Interfaces;
using TradingTools.Web.ViewModels;
using TradingTools.Web.ViewModels.Interfaces;

namespace TradingTools.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<TradingToolsDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            PersistenceRegistrator.RegisterEverything(services);

            services.AddScoped<IBinanceClient>((serviceProvider) =>
            {
                var options = new BinanceClientOptions
                {
                    ApiCredentials = new ApiCredentials(Configuration["binance:key"], Configuration["binance:secret"]),
                    AutoTimestamp = true
                };
                return new BinanceClient(options);
            });
            services.AddScoped<IBinanceExchangeService, BinanceExchangeService>();
            services.AddScoped<IT2Synchronizator, T2Synchronizator>();
            services.AddScoped<ITradeGroupStatisticsBusiness, TradeGroupStatisticsBusiness>();
            services.AddScoped<ITradeGroupViewModel, TradeGroupViewModel>();

            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
