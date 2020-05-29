// <copyright file="App.xaml.cs" company="Askaris IT">
// Copyright (c) Askaris IT. All rights reserved.
// </copyright>

using System;
using System.IO;
using com.xyroh.lib;
using WorstUrlShortener.Views;
using WorstUrlShortener.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WorstUrlShortener
{
    public partial class App : Application
    {
        public App()
        {
            this.InitializeComponent();

            // XyrohLib Crash handler Setup
            XyrohLib.setFileLog(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "debug.txt"), 500000); //0.5MB
            XyrohLib.setCrashreporter(SettingsViewModel.SentryKey);

            #if DEBUG
            XyrohLib.setAnalytics(SettingsViewModel.AppCenteriOSKey, SettingsViewModel.AppCenterAndroidKey);
            #else
				XyrohLib.setAnalytics(SettingsViewModel.AppCenteriOSKey, SettingsViewModel.AppCenterAndroidKey);
            #endif

            VersionTracking.Track();

            this.MainPage = new MainPage();
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
