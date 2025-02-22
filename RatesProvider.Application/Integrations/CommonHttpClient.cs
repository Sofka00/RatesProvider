using Microsoft.Extensions.Logging;
using RatesProvider.Application.Interfaces;
using System.Text.Json;

namespace RatesProvider.Application.Integrations;

public class CommonHttpClient : ICommonHttpClient
{
    private readonly HttpClient _client;
    private readonly ILogger<CommonHttpClient> _logger;

    public CommonHttpClient(HttpClient client, ILogger<CommonHttpClient> logger)
    {
        _client = client;
        _logger = logger;

    }

    public async Task<T> SendRequestAsync<T>(string url)
    {
        T result = default(T);
        TimeSpan interval = new TimeSpan(0, 0, 2);

        try
        {

            _logger.LogInformation("Sending GET request to URL: {Url}", url);
            using var response = await _client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);

            _logger.LogDebug("Received response with status code {StatusCode} from URL: {Url}", response.StatusCode, url);
            response.EnsureSuccessStatusCode();

            _logger.LogInformation("Received successful response from {Url} with status code {StatusCode}", url, response.StatusCode);
            var json = await response.Content.ReadAsStringAsync();

            _logger.LogDebug("Response content: {JsonContent}", json);
            result = JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while sending request to {Url}", url);
        }

        return result;
    }

}

