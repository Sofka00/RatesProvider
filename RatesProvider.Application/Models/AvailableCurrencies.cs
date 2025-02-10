using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RatesProvider.Application.Models
{
    public class AvailableCurrencies
    {
        public List<Currencies> AvailableCurrencyList { get; set; }
        public AvailableCurrencies()
        {
            AvailableCurrencyList = new List<Currencies>
            {
                Currencies.RUB,
                Currencies.USD,
                Currencies.EUR,
                Currencies.JPY,
                Currencies.CNY,
                Currencies.RSD,
                Currencies.BGN,
                Currencies.ARS

            };
        }
    }
}

