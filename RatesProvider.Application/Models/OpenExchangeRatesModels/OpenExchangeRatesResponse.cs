namespace RatesProvider.Application.Models.OpenExchangeRatesModels;

public class OpenExchangeRatesResponse
{
    public long Timestamp { get; set; }
    public string Base { get; set; }
    public DateTime Date { get; set; }
    public Dictionary<string, decimal> Rates { get; set; }
}
