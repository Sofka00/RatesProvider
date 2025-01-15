using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RatesProvider.Application.Models
{
    public class CurrencyRate
    {
        [JsonProperty("code")]
        public string Currency { get; set; }  // валютная пара типо usdRub - 
        [JsonProperty("value")]
        public decimal Value { get; set; } //- значение пары
    }
}


