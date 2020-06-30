using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using com.xyroh.lib;
using WorstUrlShortener.Interfaces;
using WorstUrlShortener.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WorstUrlShortener.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DefaultPage : ContentPage
    {
        public DefaultPage()
        {
            XyrohLib.LogEvent("Page : Shorten ");

            this.BindingContext = App.ViewModel;
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

                }else{
                    // todo move to videmodel if becomes too unwieldly

                    XyrohLib.Log("Shortener: " + this.URLShortener.SelectedItem.ToString());
                    XyrohLib.Log("Full: " + this.FullURL.Text);

                    var dict = new Dictionary<string, string>();
                    dict.Add("Shortener", this.URLShortener.SelectedItem.ToString());
                    XyrohLib.LogEvent("Shorten Page : Shorten", dict);

                    var shortenedURl = string.Empty;
                    switch (this.URLShortener.SelectedItem.ToString())
                    {
                        default:
                        case "TinyUrl":
                        {
                            var client = new HttpClient();
                            var url = "http://tinyurl.com/api-create.php?url=" + this.FullURL.Text;
                            client.Timeout = TimeSpan.FromSeconds(5);

                            try
                            {
                                var response = await client.GetAsync(url);
                                var responseBody = await response.Content.ReadAsStringAsync();
                                XyrohLib.Log("RESP: " + responseBody);
                                XyrohLib.Log("Status Code: " + response.StatusCode.ToString());
                                if (response.IsSuccessStatusCode)
                                {
                                    shortenedURl = responseBody.ToString();
                                    XyrohLib.Log("Short: " + shortenedURl);

                                    this.showResults();
                                }
                                else
                                {
                                    await this.DisplayAlert("Oops", "Something went wrong, error: " + response.StatusCode.ToString(), "OK");
                                    this.ShortenButton.IsEnabled = true;
                                }
                            }
                            catch (Exception postEx)
                            {
                                XyrohLib.LogCrash(postEx);
                                await this.DisplayAlert("Sorry", "We couldn't shorten that URL, please check and try again", "OK");
                                this.ShortenButton.IsEnabled = true;
                            }
                            break;
                        }
                        case "Goo.gl":
                        {
                            var client = new HttpClient();
                            var url = "https://firebasedynamiclinks.googleapis.com/v1/shortLinks?key=" + SettingsViewModel.FirebaseAPIKey.ToString();
                            client.Timeout = TimeSpan.FromSeconds(5);

                            try
                            {
                                var response = await client.GetAsync(url);
                                var responseBody = await response.Content.ReadAsStringAsync();
                                XyrohLib.Log("RESP: " + responseBody);
                                XyrohLib.Log("Status Code: " + response.StatusCode.ToString());
                                if (response.IsSuccessStatusCode)
                                {
                                    shortenedURl = responseBody.ToString();
                                    XyrohLib.Log("Short: " + shortenedURl);

                                    this.showResults();
                                }
                                else
                                {
                                    await this.DisplayAlert("Oops", "Something went wrong, error: " + response.StatusCode.ToString(), "OK");
                                    this.ShortenButton.IsEnabled = true;
                                }
                            }
                            catch (Exception postEx)
                            {
                                XyrohLib.LogCrash(postEx);
                                await this.DisplayAlert("Sorry", "We couldn't shorten that URL, please check and try again", "OK");
                                this.ShortenButton.IsEnabled = true;
                            }
                            break;
                        }
                    }
                    this.ShortURL.IsVisible = true;
                    this.ShortURL.Text = shortenedURl;
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

        private async void OnClipboardButtonClicked(object sender, EventArgs e)
        {
            XyrohLib.LogEvent("Shorten Page : Copy to Clipboard");

            await Clipboard.SetTextAsync(this.ShortURL.Text);
        }

        private void showResults()
        {
            this.CopyToClipBoardStack.IsVisible = true;
            this.ResultsLabel.IsVisible = true;
            this.ShortenButton.IsEnabled = true;

        }

        private void hideResults()
        {
            this.CopyToClipBoardStack.IsVisible = false;
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
