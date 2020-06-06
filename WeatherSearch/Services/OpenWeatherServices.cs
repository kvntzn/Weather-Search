using WeatherSearch.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace WeatherSearch.Services
{
    public class OpenWeatherServices : IOpenWeatherServices
    {
        HttpClient client;
        const string appid = "";

        public OpenWeatherServices()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri($"{App.OpenWeatherMapUrl}/");
            client.Timeout = TimeSpan.FromMinutes(1);
        }

        bool IsConnected => Connectivity.NetworkAccess == NetworkAccess.Internet;
        public async Task<WeatherApiResonseData> GetCurrentWeatherByCityNameAsync(string cityName)
        {
            if (cityName != null && IsConnected)
            {
                var json = await client.GetStringAsync($"data/2.5/weather?q={cityName}&appid={appid}");
                return await Task.Run(() => JsonConvert.DeserializeObject<WeatherApiResonseData>(json));
            }

            return null;
        }

        public async Task<WeatherApiResonseData> GetCurrentWeatherByGeoCoordinatesAsync(double lat, double lon)
        {
            if (IsConnected)
            {
                var json = await client.GetStringAsync($"data/2.5/weather?lat={lat}&lon={lon}&appid={appid}");
                return await Task.Run(() => JsonConvert.DeserializeObject<WeatherApiResonseData>(json));
            }

            return null;
        }

        public async Task<WeatherApiResonseData> GetCurrentWeatherByZipCodeAsync(string zipCode)
        {
            if (IsConnected)
            {
                var json = await client.GetStringAsync($"data/2.5/weather?zip={zipCode}&appid={appid}");
                return await Task.Run(() => JsonConvert.DeserializeObject<WeatherApiResonseData>(json));
            }

            return null;
        }

        public async Task<WeatherApiResonseData> GetForecastByCityNameAsync(string cityName)
        {
            if (cityName != null && IsConnected)
            {
                var json = await client.GetStringAsync($"data/2.5/forecast?q={cityName}&appid={appid}");
                return await Task.Run(() => JsonConvert.DeserializeObject<WeatherApiResonseData>(json));
            }

            return null;
        }

        public async Task<OneCallWeatherAPIResponseData> GetOneCallAPIRequestAsync(double lat, double lon)
        {
            if (IsConnected)
            {
                var json = await client.GetStringAsync($"data/2.5/onecall?lat={lat}&lon={lon}&appid={appid}");
                return await Task.Run(() => JsonConvert.DeserializeObject<OneCallWeatherAPIResponseData>(json));
            }

            return null;
        }
    }
}
