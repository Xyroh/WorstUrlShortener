// <copyright file="App.xaml.cs" company="Askaris IT">
// Copyright (c) Askaris IT. All rights reserved.
// </copyright>

using System;
using System.IO;
using com.xyroh.lib;
using WorstUrlShortener.ViewModels;
using WorstUrlShortener.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace WorstUrlShortener
{
    public partial class App : Application
    {
        public static string ImagesStore;
        public static string SharedURL = string.Empty;

        public App(string sharedURL)
        {
            this.InitializeComponent();

            // XyrohLib Crash handler Setup
            XyrohLib.setFileLog(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "debug.txt"), 500000); // 0.5MB
            XyrohLib.setCrashreporter(SettingsViewModel.SentryKey);

            #if DEBUG
            // XyrohLib.setAnalytics(SettingsViewModel.AppCenteriOSKey, SettingsViewModel.AppCenterAndroidKey);
            #else
				XyrohLib.setAnalytics(SettingsViewModel.AppCenteriOSKey, SettingsViewModel.AppCenterAndroidKey);
            #endif

            // Freshdesk
            XyrohLib.SetHelpDesk(SettingsViewModel.FreshDeskURL, SettingsViewModel.FreshDeskKey, "c29tZSByYW5kb20gdW5uZWNlc3Nhcnkga2V5");

            VersionTracking.Track();

            // filesystem prep
            try
            {
                App.ImagesStore = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "screenshots");
                Directory.CreateDirectory(App.ImagesStore);
            }
            catch (Exception ex)
            {
                XyrohLib.LogCrash(ex);
            }

            if (!string.IsNullOrEmpty(sharedURL))
            {
                XyrohLib.Log(sharedURL);
                App.SharedURL = sharedURL;
            }
            // this.MainPage = new MainPage();
            this.MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
