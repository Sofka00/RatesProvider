using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RatesProvider.Application.Models
{
    public static class AvailableCurrencies
    {
        public static List<Currency> AvailableCurrencyList= Enum.GetValues(typeof(Currency))
                   .Cast<Currency>()
                   .ToList();
    };
        
    
}

