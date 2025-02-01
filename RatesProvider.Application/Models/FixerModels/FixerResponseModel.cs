namespace RatesProvider.Application.Models.FixerModels;

public class FixerResponseModel
{
    public bool Success { get; set; }
    public long Timestamp { get; set; }
    public string Base { get; set; }
    public DateTime Date { get; set; }
    public Dictionary<string, decimal> Rates { get; set; }
}
