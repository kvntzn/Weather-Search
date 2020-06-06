using WeatherSearch.Helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeatherSearch.Models
{
    public class OneCallWeatherAPIResponseData
    {
        public double lat { get; set; }
        public double lon { get; set; }
        public string timezone { get; set; }
        public int timezone_offset { get; set; }
        public Current current { get; set; }
        public List<Hourly> hourly { get; set; }
        public List<Daily> daily { get; set; }
    }

    public class Weather
    {
        public int id { get; set; }
        public string main { get; set; }
        public string description { get; set; }
        public string description_totitle { get { return description.ToTitleCase(); } }
        public string icon { get; set; }
        public string iconurl { get { return $"i{icon}"; } }
    }

    public class Current
    {
        public int dt { get; set; }
        public int sunrise { get; set; }
        public int sunset { get; set; }
        public double temp { get; set; }
        public double feels_like { get; set; }
        public int pressure { get; set; }
        public int humidity { get; set; }
        public double dew_point { get; set; }
        public double uvi { get; set; }
        public int clouds { get; set; }
        public int visibility { get; set; }
        public double wind_speed { get; set; }
        public int wind_deg { get; set; }
        public List<Weather> weather { get; set; }
    }

    public class Rain
    {
        [JsonProperty("1h")]
        public double h { get; set; }
    }

    public class Hourly
    {
        public int dt { get; set; }
        public double temp { get; set; }
        private double temp_celsius { get { return Math.Round(temp - 273.15, 0, MidpointRounding.AwayFromZero); } }
        public string temp_string { get { return temp_celsius + "°C"; } }
        public DateTime dt_parsed { get { return dt.UnixTimeStampToDateTime(); } }
        public string dt_formatted { get; set; }
        public double feels_like { get; set; }
        public int pressure { get; set; }
        public int humidity { get; set; }
        public double dew_point { get; set; }
        public int clouds { get; set; }
        public double wind_speed { get; set; }
        public int wind_deg { get; set; }
        public List<Weather> weather { get; set; }
        public Weather firstweather { get { return weather.First(); } }
        public Rain rain { get; set; }
    }

    public class Temp
    {
        public double day { get; set; }
        public double min { get; set; }
        public double min_celsius { get { return min.ConvertToCelsius(); } }
        public double max { get; set; }
        public double max_celsius{ get { return max.ConvertToCelsius(); } }
        public double night { get; set; }
        public double eve { get; set; }
        public double morn { get; set; }
    }

    public class FeelsLike
    {
        public double day { get; set; }
        public double night { get; set; }
        public double eve { get; set; }
        public double morn { get; set; }
    }

    public class Daily
    {
        public int dt { get; set; }
        public int sunrise { get; set; }
        public int sunset { get; set; }
        public DateTime dt_parsed { get { return dt.UnixTimeStampToDateTime(); } }
        public Temp temp { get; set; }
        public FeelsLike feels_like { get; set; }
        public int pressure { get; set; }
        public int humidity { get; set; }
        public double dew_point { get; set; }
        public double wind_speed { get; set; }
        public int wind_deg { get; set; }
        public List<Weather> weather { get; set; }
        public Weather firstweather { get { return weather.First(); } }
        public int clouds { get; set; }
        public double rain { get; set; }
        public double uvi { get; set; }
    }
}
