using MassTransit;
using Microsoft.Extensions.Options;
using RatesProvider.Application.Configuration;
using RatesProvider.Application.Exeptions;
using RatesProvider.Application.Interfaces;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly ICurrencyRateManager _currencyRateManager;
    private readonly CommonHttpClientSettings _settings;

    public Worker(ILogger<Worker> logger, ICurrencyRateManager currencyRateManager, IOptions<CommonHttpClientSettings> commonHttpClientSettings)
    {
        _logger = logger;
        _currencyRateManager = currencyRateManager;
        _settings = commonHttpClientSettings.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("RatesProviderService running at: {time}", DateTimeOffset.Now);
            try
            {
                await _currencyRateManager.GetRatesAsync();
            }
            catch (ClientAttemptsExceededException ex)
            {
                _logger.LogError(ex.Message);
                _currencyRateManager.SetNextProvider();
                await _currencyRateManager.GetRatesAsync();
            }
            catch (WrongConfigurationException ex)
            {
                _logger.LogCritical(ex.Message);
                _currencyRateManager.SetNextProvider();
                await _currencyRateManager.GetRatesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            finally
            {
                await Task.Delay(_settings.Timeout, stoppingToken);
            }
        }
    }
}

