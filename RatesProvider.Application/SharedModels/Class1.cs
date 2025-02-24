using RatesProvider.Application.Models;

namespace SharedModels;

public class Class1
{
    public interface CurrencyRate
    {
        public Currency BaseCurrency { get; set; }
        public Dictionary<string, decimal> Rates { get; set; }
        public DateTime Date { get; set; }
    }

}
