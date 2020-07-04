using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using WeatherSearch.Models;
using WeatherSearch.Views;
using System.Windows.Input;
using System.Linq;
using System.Text.RegularExpressions;
using Xamarin.Essentials;
using Plugin.Geolocator;
using WeatherSearch.Utilities;
using System.Collections.Generic;
using Plugin.Geolocator.Abstractions;
using Plugin.Permissions.Abstractions;

namespace WeatherSearch.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {

        #region Properties
        private string _searchInput;
        public string SearchInput
        {
            get { return _searchInput; }
            set { SetProperty(ref _searchInput, value); }
        }

        public bool EmptyResultVisibility { get; set; }
        public bool SearchResultVisibility { get; set; }
        public bool IsSearchBarActive { get; set; }
        private WeatherApiResonseData _homepageCurrentWeatherData;
        public WeatherApiResonseData HomepageCurrentWeatherData
        {
            get { return _homepageCurrentWeatherData; }
            set { SetProperty(ref _homepageCurrentWeatherData, value); }
        }

        public OneCallWeatherAPIResponseData _homepageCurrentWeatherDetails;
        public OneCallWeatherAPIResponseData HomepageCurrentWeatherDetails
        {
            get { return _homepageCurrentWeatherDetails; }
            set { SetProperty(ref _homepageCurrentWeatherDetails, value); }
        }
        #endregion
        #region Fields
        private bool _isClicked;
        #endregion
        public ItemsViewModel()
        {
            Title = "OpenWeather";

            _homepageCurrentWeatherData = LocalDataRepository.GetHomePageWeatherData();
            _homepageCurrentWeatherDetails = LocalDataRepository.GetHomePageWeatherDetails();

            Task.Run(async () =>
            {
                if (_homepageCurrentWeatherData == null && _homepageCurrentWeatherDetails == null)
                {
                    await ExecuteSearchCommand("Mabalacat City");
                }
                else
                { 
                    await ExecuteLoadItemsCommand();
                }
            });            

            MessagingCenter.Subscribe<WeatherApiResonseData>(this, "LoadData", async (obj) =>
            {
                WeatherApiResonseData weatherApiResponse = obj as WeatherApiResonseData;
                await ExecuteSearchCommand(weatherApiResponse.name);
            });

            MessagingCenter.Subscribe<object>(this, "LoadLocation", async (obj) =>
            {
                await ExecuteGetLocationCommand();
            });
        }

        bool IsConnected => Connectivity.NetworkAccess == NetworkAccess.Internet;
        private Page MainPage => App.Current.MainPage;

        #region Commands
        public ICommand LoadItemsCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await ExecuteLoadItemsCommand();
                });
            }
        }

        public ICommand ShowSearchBarCommand
        {
            get
            {
                return new Command(() =>
                {
                    IsSearchBarActive = true;
                    OnPropertyChanged("IsSearchBarActive");
                });
            }
        }

        public ICommand HideSearchBarCommand
        {
            get
            {
                return new Command(() =>
                {
                    IsSearchBarActive = false;
                    OnPropertyChanged("IsSearchBarActive");
                });
            }
        }
        public ICommand SearchCommand
        {
            get
            {
                return new Command(async (args) =>
                {
                    await ExecuteSearchCommand(_searchInput);
                });
            }
        }
        public ICommand GetLocationCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await ExecuteGetLocationCommand();
                });
            }
        }

        public ICommand NavigateToHistoryCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await NavigateToHistory();
                });
            }
        }
        #endregion


        #region Methods
        private async Task NavigateToHistory()
        {
            if (_isClicked)
                return;

            _isClicked = true;

            await MainPage.Navigation.PushModalAsync(new HistoryPage());

            await Task.Delay(1000);
            _isClicked = false;
        }

        private async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                await Task.Run(() =>
                {
                    _homepageCurrentWeatherData = LocalDataRepository.GetHomePageWeatherData();
                    _homepageCurrentWeatherDetails = LocalDataRepository.GetHomePageWeatherDetails();


                    OnPropertyChanged("HomepageCurrentWeatherDetails");
                    OnPropertyChanged("HomepageCurrentWeatherData");
                });

                await ValidateListVisibility();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
        private async Task ExecuteSearchCommand(string searchInput)
        {
            if (IsBusy)
                return;

            IsBusy = true;

            if (!IsConnected)
            {
                await MainPage.DisplayAlert("Message", "No Internet Access. Please connect.", "Try again");
                IsBusy = false;
                return;
            }

            try
            {
                Tuple<WeatherApiResonseData, OneCallWeatherAPIResponseData> weatherInfo = await SearchLocation(searchInput);
                await NotifyUpdateWeatherUI(weatherInfo.Item1, weatherInfo.Item2);
            }
            catch (Exception ex)
            {
                await MainPage.DisplayAlert("Message", "No results found, Please try again", "Ok");
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
        private async Task ExecuteGetLocationCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            if (!IsConnected)
            {
                await MainPage.DisplayAlert("Message", "No Internet Access. Please connect.", "Try again");
                IsBusy = false;
                return;
            }
            try
            {
                Tuple<WeatherApiResonseData, OneCallWeatherAPIResponseData> weatherInfo = await GetLocation();
                await NotifyUpdateWeatherUI(weatherInfo.Item1, weatherInfo.Item2);
            }
            catch (Exception ex)
            {
                PermissionStatus status = await PermissionUtility.CheckPermissions(Plugin.Permissions.Abstractions.Permission.Location);

                if (status == PermissionStatus.Granted)
                    await MainPage.DisplayAlert("Message", "Please allow location services", "Ok");
                else
                    await MainPage.DisplayAlert("Message", "Please allow location permission", "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }
        private async Task<Tuple<WeatherApiResonseData, OneCallWeatherAPIResponseData>> SearchLocation(string searchInput)
        {
            WeatherApiResonseData weather = new WeatherApiResonseData();
            OneCallWeatherAPIResponseData weatherDetails = new OneCallWeatherAPIResponseData();
            if (searchInput.Any(char.IsDigit))
            {
                try
                {
                    weather = await OpenWeatherService.GetCurrentWeatherByZipCodeAsync(searchInput);
                    weatherDetails = await OpenWeatherService.GetOneCallAPIRequestAsync(weather.coord.lat, weather.coord.lon);
                }
                catch (Exception ex)
                {
                    Debug.Write(ex.Message);
                    weather = await OpenWeatherService.GetCurrentWeatherByCityNameAsync(searchInput);
                    weatherDetails = await OpenWeatherService.GetOneCallAPIRequestAsync(weather.coord.lat, weather.coord.lon);
                }
            }
            else
            {
                weather = await OpenWeatherService.GetCurrentWeatherByCityNameAsync(searchInput);
                weatherDetails = await OpenWeatherService.GetOneCallAPIRequestAsync(weather.coord.lat, weather.coord.lon);
            }

            return Tuple.Create(weather, weatherDetails);
        }
        private async Task<Tuple<WeatherApiResonseData, OneCallWeatherAPIResponseData>> GetLocation()
        {
            IGeolocator locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 50;

            Position positions = await locator.GetPositionAsync(timeout: new TimeSpan(10000));
            IEnumerable<Address> address = await locator.GetAddressesForPositionAsync(positions);

            WeatherApiResonseData weather = await OpenWeatherService.GetCurrentWeatherByCityNameAsync(address.FirstOrDefault().Locality);
            OneCallWeatherAPIResponseData weatherDetails = await OpenWeatherService.GetOneCallAPIRequestAsync(weather.coord.lat, weather.coord.lon);

            return Tuple.Create(weather, weatherDetails);
        }
        private async Task NotifyUpdateWeatherUI(WeatherApiResonseData weatherApiResonse, OneCallWeatherAPIResponseData weatherAPIResponseDetails)
        {
            await UpdateStoredWeatherData(weatherApiResonse, weatherAPIResponseDetails);
            await ValidateListVisibility();
        }
        private async Task UpdateStoredWeatherData(WeatherApiResonseData weatherApiResonse, OneCallWeatherAPIResponseData weatherAPIResponseDetails)
        {
            await Task.Run(() =>
            {
                weatherApiResonse.search_time = DateTime.Now;

                _homepageCurrentWeatherData = weatherApiResonse;
                _homepageCurrentWeatherDetails = weatherAPIResponseDetails;
                OnPropertyChanged("HomepageCurrentWeatherData");
                OnPropertyChanged("HomepageCurrentWeatherDetails");

                LocalDataRepository.InsertHomePageWeatherData(weatherApiResonse);
                LocalDataRepository.InsertHomePageWeatherDetails(weatherAPIResponseDetails);
                LocalDataRepository.UpsertStoredWeatherData(weatherApiResonse);
            });
        }
        private async Task ValidateListVisibility()
        {
            await Task.Run(() =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    EmptyResultVisibility = string.IsNullOrEmpty(_homepageCurrentWeatherData.name);
                    SearchResultVisibility = !EmptyResultVisibility;

                    OnPropertyChanged("EmptyResultVisibility");
                    OnPropertyChanged("SearchResultVisibility");
                });
            });
        }
        #endregion
    }
}