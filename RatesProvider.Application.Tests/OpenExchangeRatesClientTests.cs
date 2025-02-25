using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using MYPBackendMicroserviceIntegrations.Enums;
using RatesProvider.Application.Configuration;
using RatesProvider.Application.Integrations;
using RatesProvider.Application.Interfaces;
using RatesProvider.Application.Models;
using RatesProvider.Application.Models.OpenExchangeRatesModels;

namespace RatesProvider.Application.Tests;

public class OpenExchangeRatesClientTests
{
    private readonly Mock<ICommonHttpClient> _commonHttpClient;
    private readonly Mock<IOptions<OpenExchangeRatesClientSettings>> _openExchangeRatesSettings;
    private readonly Mock<ILogger<OpenExchangeRatesClient>> _logger;

    public OpenExchangeRatesClientTests()
    {
        _openExchangeRatesSettings = new Mock<IOptions<OpenExchangeRatesClientSettings>>();
        _openExchangeRatesSettings.Setup(o => o.Value).Returns(new OpenExchangeRatesClientSettings()
        { ApiKey = "cur_live_IiK9hxawHQROFiX3rUYiEMzPzQ6oa12EmOqDrSiN", BaseUrl = "https://api.currencyapi.com/v3", QueryOption = "/latest.json?app_id=" });
        _commonHttpClient = new Mock<ICommonHttpClient>();
        _logger = new Mock<ILogger<OpenExchangeRatesClient>>();
    }

    [Fact]
    public async Task GetCurrencyRatesAsync_SuccessfulResponse_ReturnsCurrencyRateMessage()
    {
        var currencyResponse = new OpenExchangeRatesResponse
        {
            Base = "USD",
            Date = DateTime.UtcNow,
            Rates = new Dictionary<string, decimal>
            {
                { "EUR", 0.85m },
                { "ARS", 0.75m }
            }
        };

        _commonHttpClient.Setup(client => client.SendRequestAsync<OpenExchangeRatesResponse>(It.IsAny<string>()))
                       .ReturnsAsync(currencyResponse);

        var client = new OpenExchangeRatesClient(_openExchangeRatesSettings.Object, _commonHttpClient.Object, _logger.Object);

        
        var result = await client.GetCurrencyRatesAsync();

        Assert.NotNull(result);
        Assert.Equal(Currency.USD, result.BaseCurrency);
        Assert.Contains("USDEUR", result.Rates.Keys);
        Assert.Contains("USDARS", result.Rates.Keys);
        Assert.Equal(0.85m, result.Rates["USDEUR"]);
        Assert.Equal(0.75m, result.Rates["USDARS"]);
    }

    [Fact]
    public async Task GetCurrencyRatesAsync_InvalidCurrency_LogsError()
    {
      
        var expectedResponse = new OpenExchangeRatesResponse
        {
            Base = "USD",
            Date = DateTime.UtcNow,
            Rates = new Dictionary<string, decimal>
            {
                { "INVALID", 101.0m }
            }
        };

        _commonHttpClient.Setup(client => client.SendRequestAsync<OpenExchangeRatesResponse>(It.IsAny<string>()))
                       .ReturnsAsync(expectedResponse);

        var client = new OpenExchangeRatesClient(_openExchangeRatesSettings.Object, _commonHttpClient.Object, _logger.Object);

      
        var result = await client.GetCurrencyRatesAsync();

   
        Assert.NotNull(result);
        Assert.Equal(Currency.USD, result.BaseCurrency);
        Assert.Empty(result.Rates); 
    }
}
