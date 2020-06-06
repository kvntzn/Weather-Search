using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace WeatherSearch
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Shell.SetTabBarIsVisible(this, false);
        }
    }
}
