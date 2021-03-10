using ExchangeRateProvider.Database;
using ExchangeRateProvider.Services;
using ExchangeRateProvider.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateProvider
{
    public class Startup
    {
        public IConfiguration config { get; set; }

        public Startup(IConfiguration config)
        {
            this.config = config;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews().AddNewtonsoftJson();

            var azurePassword = Environment.GetEnvironmentVariable("AzurePassword", EnvironmentVariableTarget.Process);
            string azureConnectionString = $"Data Source=tcp:exchangerateproviderdbserver.database.windows.net,1433;Initial Catalog=ExchangeRateProvider_db;User Id=Nicolsburg@exchangerateproviderdbserver;Password={azurePassword}";
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(azureConnectionString));

            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(config.GetConnectionString("Azure")));

            services.AddScoped<ICurrencyService, CurrencyService>();
            services.AddScoped<IMarketService, MarketService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseRouting();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}