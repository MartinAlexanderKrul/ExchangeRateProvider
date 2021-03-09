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
    public class CurrencyService : ICurrencyService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly DbSet<Currency> table;

        public CurrencyService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
            table = dbContext.Set<Currency>();
        }

        public async Task<bool> AreCurrenciesUpToDate()
        {
            var lastDbCurrency = await GetLastAsync();
            var allCurrencies = await GetAllCurrenciesAsync();
            var lastUpToDateCurrencies = allCurrencies.Where(c => c.Date == lastDbCurrency.Date).ToList();

            return lastUpToDateCurrencies.Count > 0 && lastUpToDateCurrencies.Any(c => (DateTime.Now - c.Date).TotalHours < 24);
        }

        public List<Currency> CreateCurrencies()
        {
            var filePath = "https://www.cnb.cz/cs/financni-trhy/devizovy-trh/kurzy-devizoveho-trhu/kurzy-devizoveho-trhu/denni_kurz.txt";

            var currencies = new List<Currency>();
            var cnbActual = new WebClient().DownloadString(filePath).Split("\n", StringSplitOptions.RemoveEmptyEntries);
            var date = Convert.ToDateTime(cnbActual[0].Split(" ")[0]).AddHours(14.5);

            foreach (var line in cnbActual.Skip(2))
            {
                var data = line.Split("|");
                var currency = new Currency(data[0], data[1], float.Parse(data[2]), data[3], float.Parse(data[4]), date);
                currencies.Add(currency);
            }

            for (int i = 0; i < currencies.Count; i++)
            {
                if (currencies[i].Amount != 1)
                {
                    currencies[i].Rate = currencies[i].Rate / currencies[i].Amount;
                    currencies[i].Amount = 1;
                }
            }

            return currencies;
        }

        public async Task<List<Currency>> GetAllCurrenciesAsync()
        {
            return await table.ToListAsync();
        }

        public async Task SaveAsync()
        {
            await dbContext.SaveChangesAsync();
        }

        public async Task<Currency> GetLastAsync()
        {
            return await table.OrderBy(c => c.Date).LastOrDefaultAsync();
        }

        public float ExchangeToCzk(Currency currency, float amount)
        {
            return currency.Rate * amount;
        }

        public float ExchangeFromCzk(Currency currency, float amount)
        {
            return amount / currency.Rate;
        }
    }
}