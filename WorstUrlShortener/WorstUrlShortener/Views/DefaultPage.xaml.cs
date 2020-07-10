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

            this.URLShortener.SelectedIndex = 0;
        }

        private async void OnShortenButtonClicked(object sender, EventArgs e)
        {
            try
            {
                XyrohLib.LogEvent("Shorten Page : Shorten Clicked");

                this.ShortenButton.IsEnabled = false;

                if (this.URLShortener.SelectedItem == null)
                {
                    await this.DisplayAlert("Oops!", "Please select a URL Shortener to user", "OK");
                }
                else
                {
                    // todo move to videmodel if becomes too unwieldly

                    XyrohLib.Log("Shortener: " + this.URLShortener.SelectedItem.ToString());
                    XyrohLib.Log("Full: " + this.FullURL.Text);

                    var dict = new Dictionary<string, string>();
                    dict.Add("Shortener", this.URLShortener.SelectedItem.ToString());
                    XyrohLib.LogEvent("Shorten Page : Shorten", dict);

                    var shortenedURl =
                        await this.viewModel.Shorten(this.URLShortener.SelectedItem.ToString(), this.FullURL.Text);

                    if (!string.IsNullOrEmpty(shortenedURl))
                    {
                        this.ShortURL.IsVisible = true;
                        this.ShortURL.Text = shortenedURl;

                        XyrohLib.LogEvent("Shorten Page : Copy to Clipboard");
                        await Clipboard.SetTextAsync(this.ShortURL.Text);

                        this.showResults();
                    }
                    else
                    {
                        await this.DisplayAlert("Oops", this.viewModel.LastError, "OK");
                        this.ShortenButton.IsEnabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                XyrohLib.LogCrash(ex);

                await this.DisplayAlert("Error", "Error: " + ex.Message, "OK");
                this.ShortenButton.IsEnabled = true;
            }
        }

        private void OnFullURLFocused(object sender, FocusEventArgs e)
        {
            // reset page as assume new request
            this.hideResults();
        }

        private async void OnShareButtonClicked(object sender, EventArgs e)
        {
            XyrohLib.LogEvent("Shorten Page : Share Link");
        }

        private void showResults()
        {
            this.CopyToClipBoardGrid.IsVisible = true;
            this.ResultsLabel.IsVisible = true;
            this.ShortenButton.IsEnabled = true;

            // TODO - Show 'shared to clipboard message'
        }

        private void hideResults()
        {
            this.CopyToClipBoardGrid.IsVisible = false;
            this.ResultsLabel.IsVisible = false;
            this.ShortenButton.IsEnabled = true;
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
