using RatesProvider.Application.Interfaces;
using RatesProvider.Application.Models;
using System.Text.Json;

namespace RatesProvider.Application.Integrations;

public class CommonHttpClient : ICommonHttpClient
{
    private readonly HttpClient _client;
    private readonly AvailableCurrencies _availableCurrencies;

    public CommonHttpClient(HttpClient client, AvailableCurrencies availableCurrencies)
    {
        _client = client;
        _availableCurrencies = availableCurrencies;
    }

    public async Task<T> SendRequestAsync<T>(string url)
    {
        T result = default(T);
        TimeSpan interval = new TimeSpan(0, 0, 2);
        try
        {
            using var response = await _client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            result = JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        catch (Exception ex)
        {

            throw ex;
        }

        return result;
    }

    public List<Currencies> GetAvailableCurrencies()
    {
        return _availableCurrencies.AvailableCurrencyList;
    }
}