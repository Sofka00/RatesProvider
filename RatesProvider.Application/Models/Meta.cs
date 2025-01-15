using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RatesProvider.Application.Models
{
    public class Meta
    {
        [JsonProperty("last_updated_at")]
        public DateTime LastUpdatedAt { get; set; }
    }

}
