﻿using Microsoft.Extensions.DependencyInjection;
using RatesProvider.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RatesProvider.Application.Services
{
    public class FixerApiService: IFixerApiService
    {
        private readonly ICurrencyRateProvider _fixerClient;

        public FixerApiService([FromKeyedServices("Fixer")]  ICurrencyRateProvider fixerClient)
        {
            _fixerClient = fixerClient;
        }

        public async Task Execute()
        {
            await GetCurrencyRateWithTypedClientAsync();
        }

        public async Task GetCurrencyRateWithTypedClientAsync() => await _fixerClient.GetCurrencyRatesAsync();
    }

}
