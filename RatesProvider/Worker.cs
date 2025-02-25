using MassTransit;
using RatesProvider.Application.Exeptions;
using RatesProvider.Application.Interfaces;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly ICurrencyRateManager _currencyRateManager;
    private readonly IBus _bus;

    public Worker(ILogger<Worker> logger, ICurrencyRateManager currencyRateManager, IBus bus)
    {
        _logger = logger;
        _currencyRateManager = currencyRateManager;
        _bus = bus;
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
                await Task.Delay(TimeSpan.FromMinutes(1));
            }
        }
    }
}

