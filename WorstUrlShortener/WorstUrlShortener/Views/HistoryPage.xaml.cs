using System;
using System.Collections.Generic;
using com.xyroh.lib;
using WorstUrlShortener.Interfaces;
using WorstUrlShortener.Models;
using WorstUrlShortener.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WorstUrlShortener.Views
{
    public partial class HistoryPage : ContentPage
    {
        private ShortenViewModel viewModel = new ShortenViewModel();

        public HistoryPage()
        {
            XyrohLib.LogEvent("Page : History ");

            this.BindingContext = this.viewModel;
            this.InitializeComponent();


            /*foreach(var hist in this.viewModel.History)
            {
                XyrohLib.Log("HIST: " + hist.ShortUrl + " : " + hist.CreatedDate);
            }*/
        }

        private async void onShareButtonClicked(object sender, EventArgs e)
        {
            XyrohLib.LogEvent("Button : Share URL");

            var button = sender as ImageButton;
            var selectedItem = button.BindingContext as ShortenedUrl;

            if (!string.IsNullOrEmpty(selectedItem.ShortUrl))
            {
                await Clipboard.SetTextAsync(selectedItem.ShortUrl);
                this.viewModel.ShortURL = selectedItem.ShortUrl;

                SnackBar.IsOpen = !SnackBar.IsOpen;

                /*if (this.viewModel.ShareCommand.CanExecute(null))
                {
                    this.viewModel.ShareCommand.Execute(null);
                }*/
            }
        }


        protected override void OnAppearing()
        {
            try
            {
                Accelerometer.ShakeDetected += this.OnShaked;
                Accelerometer.Start(SensorSpeed.UI);
            }
            catch (FeatureNotSupportedException featEx)
            {
                // for the emulator as not supported
            }

            if(this.viewModel.RefreshCommand.CanExecute(null))
            {
                this.viewModel.RefreshCommand.Execute(null);
            }
            

            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            try
            {
                Accelerometer.Stop();
                Accelerometer.ShakeDetected -= this.OnShaked;
            }
            catch (FeatureNotSupportedException featEx)
            {
                // for the emulator as not supported
            }

            base.OnDisappearing();
        }

        private async void OnShaked(object sender, EventArgs e)
        {
            XyrohLib.LogEvent("Shake Detected");

            try
            {
                // capture the screen
                var screenImage = await DependencyService.Get<IScreen>().CaptureScreenAsync();

                // await this.Navigation.PushModalAsync(new SupportPage("Send Feedback", screenImage));
                await this.Navigation.PushAsync(new SupportPage("Send Feedback", screenImage));
            }
            catch (Exception ex)
            {
                XyrohLib.LogCrash(ex);
            }
        }
    }
}
