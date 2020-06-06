using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using WeatherSearch.Models;

namespace WeatherSearch.Services
{
    public interface ILocalDataRepository
    {
        void InsertHomePageWeatherData(WeatherApiResonseData weatherApiResonseData);
        void InsertHomePageWeatherDetails(OneCallWeatherAPIResponseData callWeatherAPIResponseData);
        void UpsertStoredWeatherData(WeatherApiResonseData weatherApiResonseData);
        void DeleteStoredWeatherData(WeatherApiResonseData weatherApiResonseData);
        void DeleteAllStoredWeatherData();
        WeatherApiResonseData GetHomePageWeatherData();
        OneCallWeatherAPIResponseData GetHomePageWeatherDetails();
        ObservableCollection<WeatherApiResonseData> GetStoredWeatherData();

    }
}
