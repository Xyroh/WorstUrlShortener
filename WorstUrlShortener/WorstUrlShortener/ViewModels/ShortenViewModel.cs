using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using com.xyroh.lib;
using Newtonsoft.Json;
using WorstUrlShortener.Models.Json;
using Xamarin.Essentials;

namespace WorstUrlShortener.ViewModels
{
    public class ShortenViewModel : BaseViewModel
    {
        public async Task<string> Shorten(string service, string longUrl)
        {
            var shortenedURl = string.Empty;
            switch (service)
            {
                default:
                case "TinyUrl":
                {
                    var client = new HttpClient();
                    var url = "http://tinyurl.com/api-create.php?url=" + longUrl;
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
                case "Goo.gl":
                {
                    var client = new HttpClient();
                    var url = "https://firebasedynamiclinks.googleapis.com/v1/shortLinks?key=" + SettingsViewModel.FirebaseAPIKey.ToString();
                    client.Timeout = TimeSpan.FromSeconds(5);

                    try
                    {
                        var json = new FirebaseLinkRequest();
                        json.longDynamicLink = SettingsViewModel.FirebaseURLDomain + "/?link=" +
                                               longUrl + "&apn=" + AppInfo.PackageName + "&ibi=" +
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

            return shortenedURl;
        }
    }
}
