using RatesProvider.Application.Models.CurrencyApiModels;
using System.Text.Json;

namespace RatesProvider.Application.Integrations;

public class CurrencyApiClient
{
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _options;
    private readonly string _apiKey = "https://api.currencyapi.com/v3/latest?apikey=cur_live_IiK9hxawHQROFiX3rUYiEMzPzQ6oa12EmOqDrSiN&currencies=EUR%2CUSD%2CCAD";

    public CurrencyApiClient(HttpClient client)
    {
        _client = client;
        _client.BaseAddress = new Uri("https://currencyapi.com/");
        _client.Timeout = new TimeSpan(0, 0, 30);
        _client.DefaultRequestHeaders.Clear();

        _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    public async Task <CurrencyResponse> GetCurrencyRatesAsync()
    {
        using (var response = await _client.GetAsync(_apiKey, HttpCompletionOption.ResponseHeadersRead))
        {
            response.EnsureSuccessStatusCode();
            var stream = await response.Content.ReadAsStreamAsync();
            var currency = await JsonSerializer.DeserializeAsync<CurrencyResponse>(stream, _options);
            return currency;
        }
    }
}
