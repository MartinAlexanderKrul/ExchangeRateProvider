using ExchangeRateProvider.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateProvider.Services.Interfaces
{
    public interface IMarketService
    {
        public Task AddAsync(Market market);

        public Task SaveAsync();

        public Task<List<Market>> GetAllAsync();

        public Task<Market> GetLastAsync();

        public Task<Market> GetByIdAsync(int id);

        public Currency GetMarketsCurrencyByCode(Market market, string code);
    }
}