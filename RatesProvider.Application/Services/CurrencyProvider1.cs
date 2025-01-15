using currencyapi;
using Newtonsoft.Json;
using RatesProvider.Application;
using RatesProvider.Application.Models;
using RatesProvider.Starter;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

public class CurrencyProvider1 : ICurrencyProvider1
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
 

    public CurrencyProvider1(string apiKey)
    {
        _apiKey = apiKey;
        _httpClient = new HttpClient();

    }


    public async Task<Quota[]> GetRatesAsync(string baseCurrency)
    {
        try
        {
            var response = await _httpClient.GetAsync("https://api.currencyapi.com/v3/latest?apikey=cur_live_IiK9hxawHQROFiX3rUYiEMzPzQ6oa12EmOqDrSiN&currencies=EUR%2CUSD%2CCAD");
            var content = await response.Content.ReadAsStringAsync();
            var currencyResponse = JsonConvert.DeserializeObject<CurrencyResponse>(content);

            return new Quota[0]; 
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при получении курсов валют: {ex.Message}");
            throw;
        }
    }
}
