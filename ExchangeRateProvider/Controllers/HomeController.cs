using ExchangeRateProvider.Models.Entities;
using ExchangeRateProvider.Models.ViewModels;
using ExchangeRateProvider.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateProvider.Controllers
{
    [Route("")]
    public class HomeController : Controller
    {
        private readonly ICurrencyService currencyService;
        private readonly IMarketService marketService;

        public HomeController(ICurrencyService currencyService, IMarketService marketService)
        {
            this.currencyService = currencyService;
            this.marketService = marketService;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            Market market;

            if (await currencyService.AreCurrenciesUpToDate())
            {
                market = await marketService.GetLastAsync();
            }
            else
            {
                var currencies = currencyService.CreateCurrencies();
                market = new Market(currencies.FirstOrDefault().Date, currencies);

                await marketService.AddAsync(market);
                await marketService.SaveAsync();
            }

            var model = new IndexViewModel(market);

            return View("Index", model);
        }

        [HttpGet("toCZK")]
        public async Task<IActionResult> ExchangeToCzk(float amount, string code)
        {
            var market = await marketService.GetLastAsync();
            var currency = marketService.GetMarketsCurrencyByCode(market, code);
            var exchanged = currencyService.ExchangeToCzk(currency, amount);

            var model = new IndexViewModel(market, amount, exchanged, true);

            return View("Index", model);
        }

        [HttpGet("fromCZK")]
        public async Task<IActionResult> ExchangeFromCzk(float amount, string code)
        {
            var market = await marketService.GetLastAsync();
            var currency = marketService.GetMarketsCurrencyByCode(market, code);
            var exchanged = currencyService.ExchangeFromCzk(currency, amount);

            var model = new IndexViewModel(market, amount, exchanged, false);

            return View("Index", model);
        }
    }
}