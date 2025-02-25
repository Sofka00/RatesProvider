using MYPBackendMicroserviceIntegrations.Enums;

namespace RatesProvider.Application
{
    public static class AvailableCurrencies
    {
        public static List<Currency> AvailableCurrencyList = Enum.GetValues(typeof(Currency))
                   .Cast<Currency>()
                   .ToList();
    };
}
