﻿namespace RatesProvider.Application.Interfaces
{
    public interface ICurrencyApiService
    {
        Task ExecuteAsync();
        Task GetCurrencyRateWithTypedClientAsync();
    }
}