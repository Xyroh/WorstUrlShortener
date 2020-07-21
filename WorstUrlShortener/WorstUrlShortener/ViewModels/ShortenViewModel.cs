using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using com.xyroh.lib;
using Newtonsoft.Json;
using WorstUrlShortener.Models.Json;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WorstUrlShortener.ViewModels
{
    public class ShortenViewModel : BaseViewModel
    {
        private ICommand shortenCmd;
        private string shortenService = "TinyUrl";
        private List<string> shortenServices;
        private string longURL = "https://xyroh.com";
        private string shortURL;
        private bool hasResults = false;

        public ICommand ShortenCommand
        {
            get { return this.shortenCmd; }

            set
            {
                this.SetProperty(ref this.shortenCmd, value, "ShortenCommand");
            }
        }

        public string ShortenService
        {
            get => this.shortenService;
            set => this.SetProperty(ref this.shortenService, value, "ShortenService");
        }

        public List<string> ShortenServices
        {
            get => this.shortenServices;
            set => this.SetProperty(ref this.shortenServices, value, "ShortenServices");
        }

        public string LongURL
        {
            get => this.longURL;
            set => this.SetProperty(ref this.longURL, value, "LongURL");
        }

        public string ShortURL
        {
            get => this.shortURL;
            set => this.SetProperty(ref this.shortURL, value, "ShortURL");
        }

        public bool HasResults
        {
            get => this.hasResults;
            set => this.SetProperty(ref this.hasResults, value, "HasResults");
        }

        public ShortenViewModel()
        {
            this.ShortenCommand = new Command(this.OnShortenCommandExecuted);

            this.ShortenServices = new List<string>();
            this.ShortenServices.Add("TinyUrl");
            this.ShortenServices.Add("Firebase");

        }

        private async void OnShortenCommandExecuted(object state)
        {
            XyrohLib.Log("SERVICE: " + this.ShortenService);
            XyrohLib.Log("URL: " + this.LongURL);

            var shortenedURl = string.Empty;
            this.ShortURL = shortenedURl;
            this.HasResults = false;

            switch (this.shortenService)
            {
                default:
                case "TinyUrl":
                {
                    var client = new HttpClient();
                    var url = "http://tinyurl.com/api-create.php?url=" + this.LongURL;
                    XyrohLib.Log("REQUEST: " + url);
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
                        }
                        else
                        {
                            this.LastError = "Something went wrong, error: " + response.StatusCode.ToString();
                        }
                    }
                    catch (Exception postEx)
                    {
                        XyrohLib.LogCrash(postEx);
                        this.LastError = "We couldn't shorten that URL, please check and try again";
                    }
                    break;
                }
                case "Firebase":
                {
                    var client = new HttpClient();
                    var url = "https://firebasedynamiclinks.googleapis.com/v1/shortLinks?key=" + SettingsViewModel.FirebaseAPIKey.ToString();
                    client.Timeout = TimeSpan.FromSeconds(5);

                    try
                    {
                        var json = new FirebaseLinkRequest();
                        json.longDynamicLink = SettingsViewModel.FirebaseURLDomain + "/?link=" +
                                               this.LongURL + "&apn=" + AppInfo.PackageName + "&ibi=" +
                                               AppInfo.PackageName;

                        XyrohLib.Log("JSON: " + JsonConvert.SerializeObject(json));

                        var requestBody = new StringContent(JsonConvert.SerializeObject(json).ToString(), Encoding.UTF8, "application/json");
                        var response = await client.PostAsync(url, requestBody);
                        var responseString = await response.Content.ReadAsStringAsync();
                        XyrohLib.Log("RESP: " + responseString);
                        XyrohLib.Log("Status Code: " + response.StatusCode.ToString());
                        if (response.IsSuccessStatusCode)
                        {
                            var responseBody = JsonConvert.DeserializeObject<FirebaseLinkResponse>(responseString);

                            shortenedURl = responseBody.shortLink.ToString();
                            XyrohLib.Log("Short: " + shortenedURl);
                        }
                        else
                        {
                            this.LastError = "Something went wrong, error: " + response.StatusCode.ToString();
                        }
                    }
                    catch (Exception postEx)
                    {
                        XyrohLib.LogCrash(postEx);
                        this.LastError = "We couldn't shorten that URL, please check and try again";
                    }
                    break;
                }
            }

            this.ShortURL = shortenedURl;
            if (!string.IsNullOrEmpty(this.ShortURL))
            {
                await Clipboard.SetTextAsync(shortenedURl);
                this.HasResults = true;
                XyrohLib.Log("Final: " + this.ShortURL);
            }

        }
    }
}
