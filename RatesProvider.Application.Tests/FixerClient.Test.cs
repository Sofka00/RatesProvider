using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using RatesProvider.Application.Configuration;
using RatesProvider.Application.Integrations;
using RatesProvider.Application.Models.FixerApiModels;
namespace RatesProvider.Tests

{
    public class FixerClientTest
    {
        [Fact]
        public async Task GetCurrencyRatesAsync_ShouldReturnCurrencyRateMessage_WhenResponseIsValid()
        {
            // Arrange
            var mockOptions = new Mock<IOptions<FixerClientSettings>>();
            mockOptions.Setup(opt => opt.Value)
                .Returns(new FixerClientSettings { ApiKey = "valid-api-key" });

            var mockLogger = new Mock<ILogger<FixerClient>>();

            var mockHttpClient = new Mock<ICommonHttpClient>();
            mockHttpClient
                 .Setup(client => client.SendRequestAsync<FixerResponse>(It.IsAny<string>()))
                .ReturnsAsync(new FixerResponse
                {
                    Base = "EUR",
                    Rates = new Dictionary<string, decimal>
                    {
                        { "USD", 1.1m },
                        { "ARS",1109.8m},
                        { "BGN",11.00m}
                    },
                    Date = DateTime.Now
                });

            var fixerClient = new FixerClient(mockOptions.Object, mockHttpClient.Object, mockLogger.Object);

            // Act
            var result = await fixerClient.GetCurrencyRatesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal("EUR", result.BaseCurrency.ToString());
            Assert.Contains("EURUSD", result.Rates.Keys);
            Assert.Equal(1.1m, result.Rates["EURUSD"]);
            Assert.Contains("EURARS", result.Rates.Keys);
            Assert.Equal(1109.8m, result.Rates["EURARS"]);
            Assert.Contains("EURBGN", result.Rates.Keys);
            Assert.Equal(11.00m, result.Rates["EURBGN"]);
        }

        [Fact]
        public async Task GetCurrencyRatesAsync_ShouldReturnEmptyRates_WhenResponseIsInvalid()
        {
            // Arrange
            var mockOptions = new Mock<IOptions<FixerClientSettings>>();
            mockOptions.Setup(opt => opt.Value)
                .Returns(new FixerClientSettings { ApiKey = "valid-api-key" });

            var mockLogger = new Mock<ILogger<FixerClient>>();

            var mockHttpClient = new Mock<ICommonHttpClient>();
            mockHttpClient
                .Setup(client => client.SendRequestAsync<FixerResponse>(It.IsAny<string>()))
                .ReturnsAsync(new FixerResponse
                {
                    Base = "EUR",
                    Rates = new Dictionary<string, decimal>(),
                    Date = DateTime.Now
                });


            var fixerClient = new FixerClient(mockOptions.Object, mockHttpClient.Object, mockLogger.Object);

            // Act
            var result = await fixerClient.GetCurrencyRatesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal("EUR", result.BaseCurrency.ToString());
            Assert.Empty(result.Rates);
        }

        [Fact]
        public async Task GetCurrencyRatesAsync_ShouldThrowException_WhenApiFails()
        {
            // Arrange
            var mockOptions = new Mock<IOptions<FixerClientSettings>>();
            mockOptions.Setup(opt => opt.Value)
                .Returns(new FixerClientSettings { ApiKey = "valid-api-key" });

            var mockLogger = new Mock<ILogger<FixerClient>>();

            var mockHttpClient = new Mock<ICommonHttpClient>();
            mockHttpClient
                .Setup(client => client.SendRequestAsync<FixerResponse>(It.IsAny<string>()))
                .ThrowsAsync(new Exception("API request failed"));


            var fixerClient = new FixerClient(mockOptions.Object, mockHttpClient.Object, mockLogger.Object);

            // Act
            var exception = await Assert.ThrowsAsync<Exception>(() => fixerClient.GetCurrencyRatesAsync());

            // Assert
            Assert.Equal("API request failed", exception.Message);
        }
    }
}