namespace RatesProvider.Application.Exeptions;

public class ClientAttemptsExceededException(string message) : Exception(message)
{
}
