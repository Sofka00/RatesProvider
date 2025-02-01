namespace RatesProvider.Application.Models
{
    public class CurrencyRateResponse
    {
        public Dictionary<string, decimal> Rates { get; set; }
        public string BaseCurrency { get; set; }
        public DateTime Date { get; set; }
    }
}
