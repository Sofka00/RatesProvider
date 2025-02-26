using MYPBackendMicroserviceIntegrations.Enums;

namespace RatesProvider.Application.Models
{
    public static class AvailableCurrencies
    {
        public static List<Currency> AvailableCurrencyList = Enum.GetValues(typeof(Currency))
                   .Cast<Currency>()
                   .ToList();
    };
}