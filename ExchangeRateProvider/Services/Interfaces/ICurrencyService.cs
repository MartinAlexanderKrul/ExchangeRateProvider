using ExchangeRateProvider.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateProvider.Services.Interfaces
{
    public interface ICurrencyService
    {
        public Task<bool> AreCurrenciesUpToDate();

        public List<Currency> CreateCurrencies();

        public Task<List<Currency>> GetAllCurrenciesAsync();

        public Task<Currency> GetLastAsync();

        public float ExchangeToCzk(Currency currency, float amount);

        public float ExchangeFromCzk(Currency currency, float amount);
    }
}