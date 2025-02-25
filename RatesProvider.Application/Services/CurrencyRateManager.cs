using AutoMapper;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MYPBackendMicroserviceIntegrations.Messages;
using RatesProvider.Application.Interfaces;
using RatesProvider.Application.Models;
using System.Diagnostics.CodeAnalysis;

namespace RatesProvider.Application.Services;

public class CurrencyRateManager : ICurrencyRateManager
{
    private readonly ICurrencyRateProvider _providerFixer;
    private readonly ICurrencyRateProvider _providerCurrencyApi;
    private readonly ICurrencyRateProvider _providerOpenExchangeRates;
    private IRatesProviderContext _context;
    private readonly IBus _bus;
    private readonly ILogger _logger;
    private readonly IMapper _mapper;

    public CurrencyRateManager(IRatesProviderContext context,
        [FromKeyedServices("Fixer")] ICurrencyRateProvider providerFixer,
        [FromKeyedServices("CurrencyApi")] ICurrencyRateProvider providerCurrencyApi,
        [FromKeyedServices("OpenExchangeRates")] ICurrencyRateProvider providerOpenExchangeRates,
         ILogger<CurrencyRateManager> logger,
         IMapper mapper,
        IBus bus)
    {

        _providerOpenExchangeRates = providerOpenExchangeRates;
        _providerCurrencyApi = providerCurrencyApi;
        _providerFixer = providerFixer;
        _context = context;

        _logger = logger;
        _bus = bus;
        _mapper = mapper;
    }
    
    public async Task<CurrencyRateResponse> GetRatesAsync()
    {
        CurrencyRateResponse? result = default;

        var providers = new List<ICurrencyRateProvider>
        {
            _providerFixer,
            _providerCurrencyApi,
            _providerOpenExchangeRates
        };

        foreach (var provider in providers)
        {
            try
            {
                _logger.LogInformation("Trying to fetch currency rates from {ProviderType}", provider.GetType().Name);

                _context.SetCurrencyRatesProvider(provider);
                result = await _context.GetRatesAsync();

                if (result != null && result.Rates.Any())
                {
                    _logger.LogInformation("Successfully retrieved currency rates from {ProviderType}", provider.GetType().Name);
                    break;
                }
                else
                {
                    _logger.LogWarning("No data returned from {ProviderType}. Trying next provider...", provider.GetType().Name);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching rates from {ProviderType}. Trying next provider...", provider.GetType().Name);
            }
        }
        if(result.Rates.Count==0)
        {
            _logger.LogCritical("Failed to retrieve currency rates from all providers.");
            return result;
        }
        var mappedResult = _mapper.Map<CurrencyRateMessage>(result);
        await _bus.Publish(mappedResult);
        return result; 
    }
}





    




