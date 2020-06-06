using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using WeatherSearch.Models;
using WeatherSearch.Views;
using WeatherSearch.ViewModels;
using WeatherSearch.CustomControls;
using Xamarin.Forms.Skeleton;

namespace WeatherSearch.Views
{
    [DesignTimeVisible(false)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemsPage : ContentPage
    {
        ItemsViewModel viewModel;

        public ItemsPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new ItemsViewModel();
        }

        async void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null) return;

            await Task.Delay(500);

            if (sender is ListView lv) lv.SelectedItem = null;
        }

        void OnSearchBarUnfocused(object sender, FocusEventArgs e)
        {
            viewModel.HideSearchBarCommand.Execute(null);
            viewModel.SearchInput = "";
        }

        void OnSearchBarIconTapped(object sender, EventArgs e)
        {
            viewModel.ShowSearchBarCommand.Execute(null);
            SearchBar.Focus();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            MessagingCenter.Unsubscribe<object>(this, "PopulateList");
        }

        
    }
}