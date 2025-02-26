namespace RatesProvider.Application.Configuration;

public abstract class CurrencyClientSettings
{
    public string ApiKey { get; set; }
    public string BaseUrl { get; set; }
    public string QueryOption { get; set; }
    public TimeSpan Timeout { get; set; }
}
