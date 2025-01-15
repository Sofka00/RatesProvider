namespace RatesProvider.Application.Exeptions;

public class ServiceUnavailableException(string message) : Exception(message)
{
}
