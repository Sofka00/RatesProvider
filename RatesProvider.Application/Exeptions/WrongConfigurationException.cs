namespace RatesProvider.Application.Exeptions;

public class WrongConfigurationException(string message) : Exception(message)
{
}