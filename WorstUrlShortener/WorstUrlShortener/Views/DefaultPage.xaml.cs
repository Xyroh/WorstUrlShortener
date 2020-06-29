using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using com.xyroh.lib;
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
                XyrohLib.LogEvent("Shorten Page : Send Ticket");

                this.ShortenButton.IsEnabled = false;

                if (this.URLShortener.SelectedItem == null)
                {
                    await this.DisplayAlert("Oops!", "Please select a URL Shortener to user", "OK");

                }else{
                    // todo move to videmodel if becomes too unwieldly

                    XyrohLib.Log("Shortener: " + this.URLShortener.SelectedItem.ToString());
                    XyrohLib.Log("Full: " + this.FullURL.Text);

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

        private void showResults()
        {
            this.ShortURL.IsVisible = true;
            this.ResultsLabel.IsVisible = true;
            this.ShortenButton.IsEnabled = true;

        }

        private void hideResults()
        {
            this.ShortURL.IsVisible = false;
            this.ResultsLabel.IsVisible = false;
            this.ShortenButton.IsEnabled = true;
        }
    }
}
