using Microsoft.Extensions.Logging;
using RatesProvider.Application.Exeptions;
using System.Net;
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

            if (result == null)
            {
                throw new InvalidOperationException("Deserialized result is null.");
            }
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == HttpStatusCode.NotFound || ex.StatusCode == HttpStatusCode.Unauthorized || ex.StatusCode == HttpStatusCode.Forbidden)
            {
                throw new WrongConfigurationException("Error occurred while getting the base address");
            }
            _logger.LogError(ex, "HTTP error occurred while sending request to {Url}", url);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while sending request to {Url}", url);
            throw;
        }

        return result;
    }
}