using Microsoft.Extensions.Options;
using Moq;
using RatesProvider.Application.Configuration;
using RatesProvider.Application.Integrations;
using RatesProvider.Application.Interfaces;
using RatesProvider.Application.Models;
using RatesProvider.Application.Models.CurrencyApiModels;

namespace RatesProvider.Application.Tests
{
    public class CurrencyApiClientTests
    {
        private Mock<ICommonHttpClient> _commonHttpClient;
        private Mock<IOptions<CurrencyApiClientSettings>> _currencyApiSettings;
        private CurrencyApiClient _sut;

        public CurrencyApiClientTests()
        {
            _currencyApiSettings = new Mock<IOptions<CurrencyApiClientSettings>>();
            _currencyApiSettings.Setup(o => o.Value).Returns(new CurrencyApiClientSettings()
            { ApiKey = "cur_live_IiK9hxawHQROFiX3rUYiEMzPzQ6oa12EmOqDrSiN", BaseUrl = "https://openexchangerates.org/api", QueryOption = "/latest?apikey=" });
            _commonHttpClient = new Mock<ICommonHttpClient>();
            _sut = new(_currencyApiSettings.Object, _commonHttpClient.Object);
        }

        [Fact]
        public async Task GetCurrencyRatesAsync_SuccessfulResponse_ReturnsCurrencyRateResponse()
        {
            var currencyResponse = new CurrencyResponse
            {
                Meta = new Meta { LastUpdatedAt = DateTime.Now.AddDays(-1) },
                Data = new Data
                {
                    RUB = new CurrencyValue { Code = "RUB", Value = 82.00M },
                    EUR = new CurrencyValue { Code = "EUR", Value = 0.45M }
                }
            };
            _commonHttpClient.Setup(x => x.SendRequestAsync<CurrencyResponse>(It.IsAny<string>()))
                .ReturnsAsync(currencyResponse);

            var currencyApiClient = new CurrencyApiClient(_currencyApiSettings.Object, _commonHttpClient.Object);

            // Act
            var result = await currencyApiClient.GetCurrencyRatesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(Currency.USD, result.BaseCurrency);
            Assert.Equal(DateTime.Now.AddDays(-1).Date, result.Date.Date);
            Assert.Equal(1, result.Rates["USDUSD"]);
            Assert.Equal(82.00M, result.Rates["USDRUB"]);
            Assert.Equal(0.45M, result.Rates["USDEUR"]);
            _commonHttpClient.Verify(x => x.SendRequestAsync<CurrencyResponse>(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task GetCurrencyRatesAsync_NoCNYKey_ReturnsCurrencyRateResponseWithoutCNY()
        {
            // Arrange
            var currencyResponse = new CurrencyResponse
            {
                Meta = new Meta { LastUpdatedAt = DateTime.Now.AddDays(-1) },
                Data = new Data
                {
                    EUR = new CurrencyValue { Code = "EUR", Value = 0.32M }
                }
            };
            _commonHttpClient.Setup(x => x.SendRequestAsync<CurrencyResponse>(It.IsAny<string>()))
                .ReturnsAsync(currencyResponse);

            var currencyApiClient = new CurrencyApiClient(_currencyApiSettings.Object, _commonHttpClient.Object);

            // Act
            var result = await currencyApiClient.GetCurrencyRatesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(Currency.USD, result.BaseCurrency);
            Assert.Equal(DateTime.Now.AddDays(-1).Date, result.Date.Date);
            Assert.Equal(1, result.Rates["USDUSD"]);
            Assert.Equal(0.32M, result.Rates["USDEUR"]);
            Assert.False(result.Rates.ContainsKey("USDCNY"));
            _commonHttpClient.Verify(x => x.SendRequestAsync<CurrencyResponse>(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task GetCurrencyRatesAsync_MissingCurrencyData_ReturnsCurrencyRateResponseWithMissingCurrencies()
        {
            // Arrange
            _commonHttpClient.Setup(x => x.SendRequestAsync<CurrencyResponse>(It.IsAny<string>()))
                .ReturnsAsync(new CurrencyResponse
                {
                    Meta = new Meta
                    {
                        LastUpdatedAt = DateTime.Now
                    },
                    Data = new Data
                    {
                        RUB = null, // RUB отсутствует
                        EUR = new CurrencyValue { Code = "EUR", Value = 101.1m }, // EUR присутствует
                        JPY = null, // JPY отсутствует
                        CNY = new CurrencyValue { Code = "CNY", Value = 125.5m }, // CNY присутствует
                        RSD = null, // RSD отсутствует
                        BGN = new CurrencyValue { Code = "BGN", Value = 145.5m }, // BGN присутствует
                        ARS = null // ARS отсутствует
                    }
                });

            var currencyApiClient = new CurrencyApiClient(_currencyApiSettings.Object, _commonHttpClient.Object);

            // Act
            var response = await currencyApiClient.GetCurrencyRatesAsync();

            // Assert
            Assert.NotNull(response);
            Assert.NotNull(response.Rates);
            Assert.True(response.Rates.ContainsKey("USDUSD")); // Базовая валюта всегда должна быть
            Assert.True(response.Rates.ContainsKey("USDEUR")); // EUR присутствует
            Assert.True(response.Rates.ContainsKey("USDCNY")); // CNY присутствует
            Assert.True(response.Rates.ContainsKey("USDBGN")); // BGN присутствует
            Assert.False(response.Rates.ContainsKey("USDRUB")); // RUB отсутствует
            Assert.False(response.Rates.ContainsKey("USDJPY")); // JPY отсутствует
            Assert.False(response.Rates.ContainsKey("USDRSD")); // RSD отсутствует
            Assert.False(response.Rates.ContainsKey("USDARS")); // ARS отсутствует
        }
    }
}