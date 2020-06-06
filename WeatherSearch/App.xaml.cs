using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using WeatherSearch.Services;
using WeatherSearch.Views;
using System.Collections.Generic;
using WeatherSearch.Models;
using Plugin.Settings.Abstractions;
using Plugin.Settings;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using WeatherSearch.Utilities;
using System.Threading.Tasks;

namespace WeatherSearch
{
    public partial class App : Application
    {
        public static string OpenWeatherMapUrl = "https://api.openweathermap.org";
        private static ISettings AppSettings => CrossSettings.Current;

        public App()
        {
            InitializeComponent();

            DependencyService.Register<OpenWeatherServices>();
            DependencyService.Register<LocalDataRepository>();

            CheckPermissions();

            Task.Run(() =>
            {
                if (StoredWeatherApiResponseData == null)
                    StoredWeatherApiResponseData = new ObservableCollection<WeatherApiResonseData>();

                if (HistoryLocalWeatherData == null)
                    HistoryLocalWeatherData = new ObservableCollection<WeatherApiResonseData>();
                else
                    StoredWeatherApiResponseData = HistoryLocalWeatherData;

                if (HomePageLocalWeatherData == null)
                    HomePageLocalWeatherData = new WeatherApiResonseData();

                if (HomePageLocalWeatherDetails == null)
                    HomePageLocalWeatherDetails = new OneCallWeatherAPIResponseData();
            });

            //MainPage = new AppShell();
            MainPage = new NavigationPage(new ItemsPage());
        }

        public async void CheckPermissions()
        {
            await PermissionUtility.CheckPermissions(Plugin.Permissions.Abstractions.Permission.Location);
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        public static ObservableCollection<WeatherApiResonseData> StoredWeatherApiResponseData;

        public static ObservableCollection<WeatherApiResonseData> HistoryLocalWeatherData
        {
            get
            {
                if (AppSettings.Contains(nameof(HistoryLocalWeatherData)))
                    return JsonConvert.DeserializeObject<ObservableCollection<WeatherApiResonseData>>(AppSettings.GetValueOrDefault(nameof(HistoryLocalWeatherData), null));
                return null;
            }
            set => AppSettings.AddOrUpdateValue(nameof(HistoryLocalWeatherData), JsonConvert.SerializeObject(value));
        }

        public static WeatherApiResonseData HomePageLocalWeatherData
        {
            get
            {
                if (AppSettings.Contains(nameof(HomePageLocalWeatherData)))
                    return JsonConvert.DeserializeObject<WeatherApiResonseData>(AppSettings.GetValueOrDefault(nameof(HomePageLocalWeatherData), null));
                return null;
            }
            set => AppSettings.AddOrUpdateValue(nameof(HomePageLocalWeatherData), JsonConvert.SerializeObject(value));
        }

        public static OneCallWeatherAPIResponseData HomePageLocalWeatherDetails
        {
            get
            {
                if (AppSettings.Contains(nameof(HomePageLocalWeatherDetails)))
                    return JsonConvert.DeserializeObject<OneCallWeatherAPIResponseData>(AppSettings.GetValueOrDefault(nameof(HomePageLocalWeatherDetails), null));
                return null;
            }
            set => AppSettings.AddOrUpdateValue(nameof(HomePageLocalWeatherDetails), JsonConvert.SerializeObject(value));
        }
    }
}
