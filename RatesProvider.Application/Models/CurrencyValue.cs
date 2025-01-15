using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RatesProvider.Application.Models
{
    public class CurrencyValue
    {

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("value")]
        public double Value { get; set; }

    }
}
