using WeatherSearch.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WeatherSearch.Services
{
    public interface IOpenWeatherServices
    {
        Task<WeatherApiResonseData> GetCurrentWeatherByCityNameAsync(string cityName);
        Task<WeatherApiResonseData> GetCurrentWeatherByGeoCoordinatesAsync(double lat, double lon);
        Task<WeatherApiResonseData> GetCurrentWeatherByZipCodeAsync(string zipCode);
        Task<WeatherApiResonseData> GetForecastByCityNameAsync(string cityName);
        Task<OneCallWeatherAPIResponseData> GetOneCallAPIRequestAsync(double lat, double lon);
    }
}
