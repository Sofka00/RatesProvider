using RatesProvider.Application.Exeptions;
using RatesProvider.Application.Interfaces;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly ICurrencyRateManager _currencyRateManager;
    public Worker(ILogger<Worker> logger, ICurrencyRateManager currencyRateManager)
    {
        _logger = logger;
        _currencyRateManager = currencyRateManager;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _logger.LogInformation("RatesProviderService running at: {time}", DateTimeOffset.Now);
                await _currencyRateManager.GetRatesAsync();
            }
            catch (ClientAttemptsExceededException ex) // попытки
            {
                _logger.LogWarning(ex.Message);
                //_currencyRateManager.UseNextClient();
                await _currencyRateManager.GetRatesAsync();
            }
            catch (WrongConfigurationException ex) // конфигурация
            {
                _logger.LogCritical(ex.Message);
                //_currencyRateManager.UserNextClient();
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
