using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateProvider.Models.Entities
{
    public class Market
    {
        public Market()
        {
        }

        public Market(DateTime date, ICollection<Currency> currencies)
        {
            Date = date;
            Currencies = currencies;
        }

        [Key]
        public int Id { get; set; }

        public DateTime Date { get; private set; }
        public ICollection<Currency> Currencies { get; private set; }
    }
}