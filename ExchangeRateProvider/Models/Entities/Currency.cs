using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateProvider.Models.Entities
{
    public class Currency
    {
        public Currency()
        {
        }

        public Currency(string country, string name, float amount, string code, float rate, DateTime date)
        {
            Country = country;
            Name = name;
            Amount = amount;
            Code = code;
            Rate = rate;
            Date = date;
        }

        [Key]
        public int Id { get; set; }

        public string Country { get; private set; }

        public string Name { get; private set; }

        public float Amount { get; set; }

        public string Code { get; private set; }

        public float Rate { get; set; }

        public DateTime Date { get; set; }

        public Market Market { get; set; }

        [ForeignKey("Market")]
        public int MarketId { get; set; }
    }
}