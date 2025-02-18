using RatesProvider.Application.Interfaces;
using System.Text.Json;

namespace RatesProvider.Application.Integrations;

public class CommonHttpClient : ICommonHttpClient
{
    private readonly HttpClient _client;

    public CommonHttpClient(HttpClient client)
    {
        _client = client;
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
}