using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using com.xyroh.lib;
using Newtonsoft.Json;
using WorstUrlShortener.Interfaces;
using WorstUrlShortener.Models;
using WorstUrlShortener.Models.Json;
using WorstUrlShortener.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WorstUrlShortener.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DefaultPage : ContentPage
    {
        private ShortenViewModel viewModel = new ShortenViewModel();

        public DefaultPage()
        {
            XyrohLib.LogEvent("Page : Shorten ");

            this.BindingContext = this.viewModel;
            this.InitializeComponent();

            if (!string.IsNullOrEmpty(App.SharedURL))
            {
                this.viewModel.LongURL = App.SharedURL;
            }

        }

        private void OnFullURLFocused(object sender, FocusEventArgs e)
        {
            // reset page as assume new request
            this.viewModel.HasResults = false;
            this.viewModel.ShortURL = string.Empty;
        }

        private async void onShareButtonClicked(object sender, EventArgs e)
        {
            XyrohLib.LogEvent("Button : Share URL");

            // var button = sender as ImageButton;
            // var selectedItem = button.BindingContext as ShortenedUrl;

            if (!string.IsNullOrEmpty(this.viewModel.ShortURL))
            {
                await Clipboard.SetTextAsync(this.viewModel.ShortURL);

            }
        }

        /*private async void OnShareButtonClicked(object sender, EventArgs e)
        {
            XyrohLib.LogEvent("Shorten Page : Share Link");

            await Share.RequestAsync(new ShareTextRequest
            {
                Uri = this.viewModel.ShortURL,
                Title = "Shortened Link"
            });
        }*/

        /*private void showResults()
        {
            this.CopyToClipBoardGrid.IsVisible = true;
            this.ResultsLabel.IsVisible = true;
            this.ShortenButton.IsEnabled = true;

        }

        private void hideResults()
        {
            this.CopyToClipBoardGrid.IsVisible = false;
            this.ResultsLabel.IsVisible = false;
            this.ShortenButton.IsEnabled = true;
        }*/

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
