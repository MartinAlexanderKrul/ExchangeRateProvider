using ExchangeRateProvider.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateProvider.Models.ViewModels
{
    public class IndexViewModel
    {
        public IndexViewModel(Market market)
        {
            Market = market;
        }

        public IndexViewModel(Market market, float originalAmount, float exchangedAmount, bool toCzk)
        {
            Market = market;
            OriginalAmount = originalAmount;
            ExchangedAmount = exchangedAmount;
            ToCzk = toCzk;
        }

        public Market Market { get; set; }
        public float? OriginalAmount { get; set; }
        public float? ExchangedAmount { get; set; }
        public bool ToCzk { get; set; }
    }
}