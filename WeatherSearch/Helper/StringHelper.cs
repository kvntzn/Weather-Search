using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace WeatherSearch.Helper
{
    public static class StringHelper
    {
        private static TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

        public static string ToTitleCase(this string word)
        {
            return textInfo.ToTitleCase(word);
        }
    }
}
