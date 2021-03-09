using ExchangeRateProvider.Database;
using ExchangeRateProvider.Models.Entities;
using ExchangeRateProvider.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ExchangeRateProvider.Services
{
    public class MarketService : IMarketService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly DbSet<Market> table;

        public MarketService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
            table = dbContext.Set<Market>();
        }

        public async Task AddAsync(Market market)
        {
            await table.AddAsync(market);
        }

        public async Task SaveAsync()
        {
            await dbContext.SaveChangesAsync();
        }

        public async Task<List<Market>> GetAllAsync()
        {
            return await table.Include(m => m.Currencies).ToListAsync();
        }

        public async Task<Market> GetLastAsync()
        {
            return await table.Include(m => m.Currencies).OrderByDescending(m => m.Date).LastOrDefaultAsync();
        }

        public async Task<Market> GetByIdAsync(int id)
        {
            return await table.Include(m => m.Currencies).FirstOrDefaultAsync();
        }

        public Currency GetMarketsCurrencyByCode(Market market, string code)
        {
            return market.Currencies.Where(c => c.Code == code).FirstOrDefault();
        }
    }
}