using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeatherSearch.Helper;

namespace WeatherSearch.Models
{
    public class WeatherApiResonseData
    {
        public class Coord
        {
            public double lon { get; set; }
            public double lat { get; set; }
        }

        public class Weather
        {
            public int id { get; set; }
            public string main { get; set; }
            public string description { get; set; }
            public string icon { get; set; }
        }

        public class Main
        {
            public double temp { get; set; }
            public int pressure { get; set; }
            public int humidity { get; set; }
            public double temp_min { get; set; }
            public double temp_max { get; set; }
            public double temp_celsius { get { return temp.ConvertToCelsius(); } }
            public double temp_min_celsius { get { return temp_min.ConvertToCelsius(); } }
            public double temp_max_celsius { get { return temp_max.ConvertToCelsius(); } }
            public string humidity_string { get { return humidity + " %"; } }
            public string pressure_string { get { return pressure + " hpa"; } }

        }

        public class Wind
        {
            public double speed { get; set; }
            public int deg { get; set; }
            public double gust { get; set; }
        }

        public class Clouds
        {
            public int all { get; set; }
        }

        public class Sys
        {
            public int type { get; set; }
            public int id { get; set; }
            public double message { get; set; }
            public string country { get; set; }
            public int sunrise { get; set; }
            public int sunset { get; set; }
        }
        public class Rain
        {
            [JsonProperty("3h")]
            public double h { get; set; }
        }
        public class City
        {
            public int id { get; set; }
            public string name { get; set; }
            public Coord coord { get; set; }
            public string country { get; set; }
            public int population { get; set; }
            public int timezone { get; set; }
            public int sunrise { get; set; }
            public int sunset { get; set; }
        }

        public class WeatherList
        {
            public int dt { get; set; }
            public Main main { get; set; }
            public List<Weather> weather { get; set; }
            public Clouds clouds { get; set; }
            public Wind wind { get; set; }
            public Sys sys { get; set; }
            public string dt_txt { get; set; }
            public Rain rain { get; set; }
            public string temp_string { get { return main != null ? main.temp_celsius + "°C" : ""; } }
            public string iconurl { get { return $"i{weather.First().icon}"; } }
            public DateTime dt_parsed { get { return DateTime.Parse(dt_txt); } }
            public string dt_formatted { get; set; }
        }

        public int message { get; set; }
        public int cnt { get; set; }
        public List<WeatherList> list { get; set; }
        public City city { get; set; }
        public Coord coord { get; set; }
        public List<Weather> weather { get; set; }
        public string @base { get; set; }
        public Main main { get; set; }
        public int visibility { get; set; }
        public Wind wind { get; set; }
        public Clouds clouds { get; set; }
        public int dt { get; set; }
        public DateTime time { get { return dt.UnixTimeStampToDateTime(); } }
        public Sys sys { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public string temp_string { get { return main != null ? main.temp_celsius + "°C" : ""; } }
        public string temp_min_string { get { return main != null ? main.temp_min_celsius + "°C" : ""; } }
        public string temp_max_string { get { return main != null ? main.temp_max_celsius + "°C" : ""; } }
        public string fullname { get { return (name != null && sys != null) ? name + ", " + sys.country : ""; } }
        public string iconurl { get { return $"i{weather?.FirstOrDefault().icon}"; } }
        public string weather_description { get { return weather != null ? $"{weather.First().description.ToTitleCase()}" : ""; } }
        public int cod { get; set; }
        public DateTime search_time { get; set; }

    }
}
