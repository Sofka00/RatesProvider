using RatesProvider.Application.Integrations;


namespace RatesProvider.Application.Services
{
    public class CurrencyProviderService1 
    {
        private readonly CommonHttpClient<CurrencyRate> _httpClient;

        public CurrencyProviderService1(HttpMessageHandler? handler = null)
        {
            _httpClient = new CommonHttpClient<CurrencyRate>("https://api.currencyapi.com/v3/latest/", handler);
        }

        public void SendCurrencyRate(CurrencyRate rate)
        {
            var exchangeRate = _httpClient.SendGetRequest($"rates/{rate.Currency}");
        }
    }
}
