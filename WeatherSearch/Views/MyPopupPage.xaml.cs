using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeatherSearch.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyPopupPage : Rg.Plugins.Popup.Pages.PopupPage
    {
        public MyPopupPage()
        {
            InitializeComponent();
        }

        public void OnWebsiteTapped(object sender, EventArgs e)
        {
            Launcher.OpenAsync(new Uri("https://kvntzn.github.io/"));
        }

        public void OnIconCreditsTapped(object sender, EventArgs e)
        {
            Launcher.OpenAsync(new Uri("https://www.iconfinder.com/andhikairfani"));
        }

        protected override bool OnBackgroundClicked()
        {
            PopupNavigation.Instance.PopAsync();

            return base.OnBackgroundClicked();
        }
    }
}