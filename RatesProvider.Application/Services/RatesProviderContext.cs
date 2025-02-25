using Microsoft.Extensions.Logging;
using MYPBackendMicroserviceIntegrations.Messages;
using RatesProvider.Application.Interfaces;
using RatesProvider.Application.Models;

namespace RatesProvider.Application.Services
{
    public class RatesProviderContext : IRatesProviderContext
    {
        private ICurrencyRateProvider _currencyRateProvider;
        private readonly ILogger<RatesProviderContext> _logger;


        public RatesProviderContext(ILogger<RatesProviderContext> logger)
        {
            _currencyRateProvider = null;
            _logger = logger;   
        }

        public void SetCurrencyRatesProvider(ICurrencyRateProvider currencyRateProvider)
        {
            _currencyRateProvider = currencyRateProvider;
            _logger.LogInformation("CurrencyRateProvider has been set to: {ProviderType}", _currencyRateProvider.GetType().Name);   
        }

        public async Task<CurrencyRateMessage> GetRatesAsync()
        {
            CurrencyRateMessage response = default;
            TimeSpan interval = new TimeSpan(0, 0, 2);

            for (int i = 0; i < 3; i++)
            {
                try
                {
                    _logger.LogInformation("Attempting to fetch currency rates, attempt {AttemptNumber}/3", i + 1);
                    response = await _currencyRateProvider.GetCurrencyRatesAsync();
                    return response;

                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to fetch currency rates on attempt {AttemptNumber}/3. Retrying in {Delay} seconds.", i + 1, interval.TotalSeconds);
                    await Task.Delay(interval * i);

                }
            }
            throw new Exception("All attempts to fetch currency rates failed.");
        }


    }
}
