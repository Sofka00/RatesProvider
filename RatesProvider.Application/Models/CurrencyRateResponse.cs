using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RatesProvider.Application.Models
{
    public class CurrencyRateResponse
    {
        public Currences BaseCurrency { get; set; } // базовая валюта из енама 
        public Dictionary<string, decimal> Rates { get; set; }
        public DateTime Date { get; set; }
     
    }
}
