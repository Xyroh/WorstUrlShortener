using Xamarin.Essentials;

namespace WorstUrlShortener.ViewModels
{
    public class SettingsViewModel
    {
        /*
         *
         * ALL THESE VALUES ARE APP SPECIFIC, IE FOR MY VERSION,
         * CHANGE THEM ALL UNLESS YOU WANT YOUR APP CRASH REPORTS
         * AND SUPPORT TICKETS COMING TO ME!!
         *
         */

        // analytics

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

        // freshdesk
        public static string FreshDeskURL
        {
            get => Preferences.Get("FreshDeskURL", "https://xyroh.freshdesk.com");
            set => Preferences.Set("FreshDeskURL", value);
        }

        public static string FreshDeskKey
        {
            get => Preferences.Get("FreshDeskKey", "bH2xmn4atsRUVHxFI9x");
            set => Preferences.Set("FreshDeskKey", value);
        }
    }
}
