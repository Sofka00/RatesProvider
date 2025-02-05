namespace RatesProvider.Application.Models
{
    public class CurrencyRateResponse
    {
        public Dictionary<string, decimal> Rates { get; set; }
        public Currencies BaseCurrency { get; set; }
        public DateTime Date { get; set; }
    }
}
