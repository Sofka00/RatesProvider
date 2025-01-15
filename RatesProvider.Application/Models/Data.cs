using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RatesProvider.Application.Models
{
    public class Data
    {    
            [JsonProperty("EUR")]
            public CurrencyValue EUR { get; set; }

            [JsonProperty("USD")]
            public CurrencyValue USD { get; set; }

            [JsonProperty("CAD")]
            public CurrencyValue CAD { get; set; }

            [JsonProperty("BND")]
            public CurrencyValue Bnd { get; set; }

            [JsonProperty("BOB")]
            public CurrencyValue Bob { get; set; }
       
    }
}
