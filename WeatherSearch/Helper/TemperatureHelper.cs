using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherSearch.Helper
{
    public static class TemperatureHelper
    {
        public static double ConvertToCelsius(this double temp) 
        {
            return Math.Round(temp - 273.15, 0, MidpointRounding.AwayFromZero);
        }

    }
}
