using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RatesProvider.Application.Models
{
    public static class AvailableCurrencies
    {
        public static List<Currencies> AvailableCurrencyList= Enum.GetValues(typeof(Currencies))
                   .Cast<Currencies>()
                   .ToList();
    };
        
    
}

