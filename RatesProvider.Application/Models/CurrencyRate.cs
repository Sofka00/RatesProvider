using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RatesProvider.Application.Models;

public class CurrencyRate
{
    public Dictionary<string, decimal> Rates { get; set; }
    public string BaseCurrency { get; set; }
    public DateTime Date { get; set; }
    Currencies Currencies { get; set; }
}
