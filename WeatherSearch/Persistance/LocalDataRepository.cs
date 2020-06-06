using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using WeatherSearch.Models;

namespace WeatherSearch.Services
{
    public class LocalDataRepository : ILocalDataRepository
    {
        // Can't assign directly stored values won't update
        public void DeleteAllStoredWeatherData()
        {
            var storedWeatherApiResponseData = App.StoredWeatherApiResponseData;
            storedWeatherApiResponseData.Clear();
            
            App.HistoryLocalWeatherData = App.StoredWeatherApiResponseData = storedWeatherApiResponseData;
        }

        public void DeleteStoredWeatherData(WeatherApiResonseData weatherApiResonseData)
        {
            ObservableCollection<WeatherApiResonseData> storedWeatherApiResponseData = App.StoredWeatherApiResponseData;
            storedWeatherApiResponseData.Remove(weatherApiResonseData);
            
            App.HistoryLocalWeatherData = App.StoredWeatherApiResponseData = storedWeatherApiResponseData;
        }

        public WeatherApiResonseData GetHomePageWeatherData()
        {
            return App.HomePageLocalWeatherData;
        }

        public OneCallWeatherAPIResponseData GetHomePageWeatherDetails()
        {
            return App.HomePageLocalWeatherDetails;
        }

        public ObservableCollection<WeatherApiResonseData> GetStoredWeatherData()
        {
            return App.StoredWeatherApiResponseData;
        }

        public void InsertHomePageWeatherData(WeatherApiResonseData weatherApiResonseData)
        {
            WeatherApiResonseData _homepageLocalData = App.HomePageLocalWeatherData;
            _homepageLocalData = weatherApiResonseData;
            App.HomePageLocalWeatherData = _homepageLocalData;
        }

        public void InsertHomePageWeatherDetails(OneCallWeatherAPIResponseData callWeatherAPIResponseData)
        {
            OneCallWeatherAPIResponseData _homePageLocalWeatherDetails = App.HomePageLocalWeatherDetails;
            _homePageLocalWeatherDetails = callWeatherAPIResponseData;
            App.HomePageLocalWeatherDetails = _homePageLocalWeatherDetails;
        }

        public void UpsertStoredWeatherData(WeatherApiResonseData weatherApiResonseData)
        {
            ObservableCollection<WeatherApiResonseData> storedWeatherData = App.StoredWeatherApiResponseData;
            ObservableCollection<WeatherApiResonseData> localstoredWeatherData = App.HistoryLocalWeatherData;

            WeatherApiResonseData existingLocalWeatherData = localstoredWeatherData.FirstOrDefault(s => s.name == weatherApiResonseData.name);
            WeatherApiResonseData existingStoredWeatherData = storedWeatherData.FirstOrDefault(s => s.name == weatherApiResonseData.name);
            if (existingLocalWeatherData != null && existingStoredWeatherData != null)
            {
                storedWeatherData.Remove(existingStoredWeatherData);
                localstoredWeatherData.Remove(existingLocalWeatherData);
            }

            storedWeatherData.Add(weatherApiResonseData);
            localstoredWeatherData.Add(weatherApiResonseData);

            App.StoredWeatherApiResponseData = storedWeatherData;
            App.HistoryLocalWeatherData = localstoredWeatherData;
        }
    }
}
