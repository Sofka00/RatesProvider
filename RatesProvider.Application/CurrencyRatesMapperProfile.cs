using AutoMapper;
using MYPBackendMicroserviceIntegrations.Messages;
using RatesProvider.Application.Models;

namespace RatesProvider.Application;

public class CurrencyRatesMapperProfile: Profile
{
    public CurrencyRatesMapperProfile()
    {
        CreateMap<CurrencyRateResponse, CurrencyRateMessage>();
    }
}
