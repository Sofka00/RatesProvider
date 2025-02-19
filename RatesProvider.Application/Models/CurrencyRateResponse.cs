namespace RatesProvider.Application.Models;

public class CurrencyRateResponse
{
    public Currency BaseCurrency { get; set; }
    public Dictionary<string, decimal> Rates { get; set; }
    public DateTime Date { get; set; }
}
