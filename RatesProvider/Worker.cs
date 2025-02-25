using MassTransit;
using RatesProvider.Application.Interfaces;
using RatesProvider.Application.Models;


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
            catch (Exception ex)
            {

                _logger.LogError(ex, "Error fetching rates, switching provider...");
                _currencyRateManager.SetNextProvider();
                continue;
            }
            await _currencyRateManager.GetRatesAsync();
            await Task.Delay(TimeSpan.FromSeconds(10));
        }
    }
}

