using WeatherSearch.Models;
using WeatherSearch.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Rg.Plugins.Popup.Services;

namespace WeatherSearch.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HistoryPage : ContentPage
    {
        private HistoryViewModel viewModel;
        public HistoryPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new HistoryViewModel();
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            WeatherApiResonseData weatherApiResponse = args.SelectedItem as WeatherApiResonseData;
            if (weatherApiResponse == null)
                return;

            await Navigation.PopModalAsync();
            MessagingCenter.Send(weatherApiResponse, "LoadData");

            // Manually deselect item.
            ItemsListView.SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            viewModel.LoadCommand.Execute(null);
        }
    }
}