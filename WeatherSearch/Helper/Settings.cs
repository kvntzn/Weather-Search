using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherSearch.Helper
{
	public static class Settings
	{
		private static ISettings AppSettings
		{
			get
			{
				return CrossSettings.Current;
			}
		}

		#region Setting Constants

		private const string HelpSettingsKey = "helpsettings_key";
		private const string NotificationSettingsKey = "notificationsettings_key";
		private const string SettingsKey = "settings_key";
		// private static readonly string SettingsDefault = string.Empty;
		#endregion

		public static string GeneralSettings
		{
			get => AppSettings.GetValueOrDefault(nameof(GeneralSettings), string.Empty);

			set => AppSettings.AddOrUpdateValue(nameof(GeneralSettings), value);

		}
	}
}
