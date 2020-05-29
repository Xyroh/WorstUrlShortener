using Xamarin.Essentials;

namespace WorstUrlShortener.ViewModels
{
    public class SettingsViewModel
    {
        #region analytics

        public static string SentryKey
        {
            get => Preferences.Get("SentryKey", "https://b97c74f92fce40cab00418ed0ef0f8cc@sentry.io/5257872");
            set => Preferences.Set("SentryKey", value);
        }

        public static string AppCenteriOSKey
        {
            get => Preferences.Get("AppCenteriOSKey", "83cb9aab-0fff-4c4b-9fd0-918311e6b942");
            set => Preferences.Set("AppCenteriOSKey", value);
        }

        public static string AppCenterAndroidKey
        {
            get => Preferences.Get("AppCenterAndroidKey", "f774074e-ad47-4d24-bf9f-6c7cb9e18b6d");
            set => Preferences.Set("AppCenterAndroidKey", value);
        }

        #endregion
    }
}

