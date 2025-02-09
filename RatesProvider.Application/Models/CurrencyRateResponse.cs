namespace RatesProvider.Application.Models;

public class CurrencyRateResponse
{
    public Currences BaseCurrency { get; set; }
    public Dictionary<string, decimal> Rates { get; set; }
    public DateTime Date { get; set; }
}
