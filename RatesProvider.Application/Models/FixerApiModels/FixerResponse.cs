using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RatesProvider.Application.Models.FixerApiModels
{
    public class FixerResponse
    {
        public bool Success { get; set; }             
        public long Timestamp { get; set; }            
        public string Base { get; set; }              
        public DateTime Date { get; set; }            
        public Dictionary<string, decimal> Rates { get; set; } 

    }
}
