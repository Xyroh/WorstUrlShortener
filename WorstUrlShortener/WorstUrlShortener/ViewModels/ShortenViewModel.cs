using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using com.xyroh.lib;
using Newtonsoft.Json;
using WorstUrlShortener.DAO;
using WorstUrlShortener.Models;
using WorstUrlShortener.Models.Json;
using WorstUrlShortener.Utilities;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WorstUrlShortener.ViewModels
{
    public class ShortenViewModel : BaseViewModel
    {
        private ICommand shortenCmd;
        private ICommand shareCmd;

        private string shortenService = "TinyUrl";
        private List<string> shortenServices;
        private string longURL = "https://xyroh.com";
        private string shortURL;
        private bool hasResults = false;

        private SQLiteContext db;

        private ObservableRangeCollection<ShortenedUrl> history = new ObservableRangeCollection<ShortenedUrl>();

        private Command refreshCmd;

        private async Task refresh()
        {
            await ExecuteRefreshCommand();
        }

        private async Task ExecuteRefreshCommand()
        {
            XyrohLib.LogEvent("Refresh History", "DB");


            if (IsBusy)
            {
                XyrohLib.Log("VM BUSY - RETURNING");
                return;
            }
            IsBusy = true;
            try
            {
                var record = await this.GetHistory();

            }

            catch (Exception ex)
            {
                XyrohLib.LogCrash("DB", ex);

            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task<ObservableRangeCollection<ShortenedUrl>> GetHistory()
        {
            this.IsBusy = true;
            this.LastError = string.Empty;


            try
            {
                this.History.Clear();

                var records = this.db.ShortenedUrl.ToList();
                records = records.OrderByDescending(x => x.CreatedAt).ToList();

                this.History.ReplaceRange(records);
            }
            catch (Exception ex)
            {
                this.LastError = "ERROR: " + ex.Message;
                XyrohLib.LogException("GetHistory", ex);
            }

            this.IsBusy = false;

            return this.History;
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
                        var url = "https://firebasedynamiclinks.googleapis.com/v1/shortLinks?key=" + BaseConfig.FirebaseAPIKey.ToString();
                        client.Timeout = TimeSpan.FromSeconds(5);

                        try
                        {
                            var json = new FirebaseLinkRequest();
                            json.longDynamicLink = BaseConfig.FirebaseURLDomain + "/?link=" +
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
                // save to history
                var history = new ShortenedUrl();
                history.ShortenService = this.shortenService;
                history.FullUrl = this.LongURL;
                history.ShortUrl = this.ShortURL;

                try
                {
                    // Can't use App singleton as App doesn't exist for the extension
                    this.db.Add(history);
                    await this.db.SaveChangesAsync();

                    XyrohLib.Log("Saved History: " + shortenedURl);
                }
                catch (Exception ex)
                {
                    XyrohLib.LogCrash("DB", ex);
                }

                await Clipboard.SetTextAsync(shortenedURl);

                this.HasResults = true;
                XyrohLib.Log("Final: " + this.ShortURL);


                await this.refresh();
            }

        }

        private async void OnShareCommandExecuted(object state)
        {
            XyrohLib.LogEvent("Shorten Page : Share Link");

            await Share.RequestAsync(new ShareTextRequest
            {
                Uri = this.ShortURL,
                Title = "Shortened Link"
            });
        }


        // initiated in the private def, refreshed in the constructor
        public ObservableRangeCollection<ShortenedUrl> History
        {
            get
            {
                return this.history;
            }

            set
            {
                this.history = value;
                OnPropertyChanged("History");
            }
        }

        public Command RefreshCommand
        {
            get
            {
                return this.refreshCmd ?? (this.refreshCmd = new Command(async () => await ExecuteRefreshCommand()));
            }
        }


        public ICommand ShortenCommand
        {
            get { return this.shortenCmd; }

            set
            {
                this.SetProperty(ref this.shortenCmd, value, "ShortenCommand");
            }
        }

        public ICommand ShareCommand
        {
            get { return this.shareCmd; }

            set
            {
                this.SetProperty(ref this.shareCmd, value, "ShareCommand");
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
            this.ShareCommand = new Command(this.OnShareCommandExecuted);

            this.ShortenServices = new List<string>();
            this.ShortenServices.Add("TinyUrl");
            this.ShortenServices.Add("Firebase");

            this.db = new SQLiteContext();

            this.refresh();

        }


    }
}
