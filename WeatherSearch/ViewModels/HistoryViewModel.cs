using WeatherSearch.Models;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Services;
using WeatherSearch.Views;

namespace WeatherSearch.ViewModels
{

    public class HistoryViewModel : BaseViewModel
    {
        #region Fields
       
        private Page MainPage => App.Current.MainPage;
        private bool _isClicked;
        #endregion

        #region Properties
        public double _listViewHeight;
        public double ListViewHeight
        {
            get { return _listViewHeight; }
            set { SetProperty(ref _listViewHeight, value); }
        }

        public ObservableCollection<WeatherApiResonseData> _storedWeatherApiResponseData;
        public ObservableCollection<WeatherApiResonseData> StoredWeatherApiResponseData
        {
            get { return _storedWeatherApiResponseData; }
            set { SetProperty(ref _storedWeatherApiResponseData, value); }
        }

        private bool _searchResultVisibility;
        public bool SearchResultVisibility
        {
            get { return _searchResultVisibility; }
            set { SetProperty(ref _searchResultVisibility, value); }
        }

        private bool _emptyResultVisibility;
        public bool EmptyResultVisibility
        {
            get { return _emptyResultVisibility; }
            set { SetProperty(ref _emptyResultVisibility, value); }
        }
        #endregion

        public HistoryViewModel()
        {
            Title = "History";

            Task.Run(async () =>
            {
                await InitializeHistory();
                await ValidateListVisibility();
            });

        }

        #region Command
        public ICommand GetLocationCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await MainPage.Navigation.PopModalAsync();
                    MessagingCenter.Send(new object(), "LoadLocation");
                });
            }
        }

        public ICommand LoadCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await ExecuteLoadItemsCommand();
                });
            }
        }
        public ICommand DeleteCommand
        {
            get
            {
                return new Command<WeatherApiResonseData>(async (model) =>
                {
                    await Task.Run(() =>
                    {
                        LocalDataRepository.DeleteStoredWeatherData(model);
                        _storedWeatherApiResponseData = LocalDataRepository.GetStoredWeatherData();
                        OnPropertyChanged("StoredWeatherApiResponseData");
                    });

                    await ValidateListVisibility();
                    await CalculateListHeight();
                });
            }
        }
        public ICommand DeleteAllCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await Task.Run(() =>
                    {
                        LocalDataRepository.DeleteAllStoredWeatherData();
                        _storedWeatherApiResponseData = LocalDataRepository.GetStoredWeatherData();

                        OnPropertyChanged("StoredWeatherApiResponseData");
                    });
                    
                    await ValidateListVisibility();
                    await CalculateListHeight();
                });
            }
        }

        public ICommand BackCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await MainPage.Navigation.PopModalAsync();
                });
            }
        }

        public ICommand InfoCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await NavigateToInfo();
                });
            }
        }

        #endregion

        #region Methods
        async Task NavigateToInfo()
        {
            if (_isClicked)
                return;

            _isClicked = true;

            await PopupNavigation.Instance.PushAsync(new MyPopupPage());

            await Task.Delay(1000);
            _isClicked = false;
        }
        async Task InitializeHistory()
        {
            await Task.Run(() =>
            {
                ObservableCollection<WeatherApiResonseData>
                    storedWeatherApiResponseData = LocalDataRepository.GetStoredWeatherData();
                if (storedWeatherApiResponseData.Count > 0)
                    _storedWeatherApiResponseData = new ObservableCollection<WeatherApiResonseData>(storedWeatherApiResponseData.OrderByDescending(a => a.search_time));
                else
                    _storedWeatherApiResponseData = new ObservableCollection<WeatherApiResonseData>();
            });
            
        }
        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                ObservableCollection<WeatherApiResonseData>
                    storedWeatherApiResponseData = LocalDataRepository.GetStoredWeatherData();

                _storedWeatherApiResponseData = new ObservableCollection<WeatherApiResonseData>(storedWeatherApiResponseData.OrderByDescending(a => a.search_time));
                OnPropertyChanged("StoredWeatherApiResponseData");

                await ValidateListVisibility();
                await CalculateListHeight();
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
        async Task ValidateListVisibility()
        {
            await Task.Run(() =>
            {
                _searchResultVisibility = _storedWeatherApiResponseData.Count > 0;
                _emptyResultVisibility = !_searchResultVisibility;

                OnPropertyChanged("SearchResultVisibility");
                OnPropertyChanged("EmptyResultVisibility");
            });
        }
        async Task CalculateListHeight()
        {
            await Task.Run(() =>
            {
                _listViewHeight = _storedWeatherApiResponseData.Count * 44;
                OnPropertyChanged("ListViewHeight");
            });
        }
        #endregion
    }
}
