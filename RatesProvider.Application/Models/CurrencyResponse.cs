using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RatesProvider.Application.Models
{
    public class CurrencyResponse
    {
        public Meta Meta { get; set; }
        public Data Data { get; set; }
    }
}
