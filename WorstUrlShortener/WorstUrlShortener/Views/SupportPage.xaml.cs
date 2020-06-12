using System;
using System.Collections.Generic;
using System.IO;
using com.xyroh.lib;
using WorstUrlShortener.Utilities;
using WorstUrlShortener.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WorstUrlShortener.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SupportPage : ContentPage
    {
        private byte[] capturedImageBytes;
        private string captureImagePath;

        public SupportPage(byte[] imageArray)
        {
            XyrohLib.LogEvent("Page : Support : With Screenshot");
            this.capturedImageBytes = imageArray;

            try
            {
                // save to disk
                this.captureImagePath =
                    "Screen" + new DateTimeOffset(DateTime.Now).ToUnixTimeMilliseconds().ToString() + "_" +
                    Guid.NewGuid().ToString().ToString() + ".jpg";
                this.captureImagePath = Path.Combine(App.ImagesStore, this.captureImagePath);
                XyrohLib.Log("** fileName: " + this.captureImagePath);

                File.WriteAllBytes(this.captureImagePath, imageArray);
            }
            catch (Exception ex)
            {
                XyrohLib.LogCrash("Screen Capture", ex);
            }

            this.Init();
        }

        public SupportPage()
        {
            XyrohLib.LogEvent("Page : Support");
            this.Init();
        }

        private async void Init()
        {
            this.InitializeComponent();

            this.TicketSubject.Text = "Worst URL Shortener Support Request - " + VersionTracking.CurrentVersion + "#" + VersionTracking.CurrentBuild;

            try
            {
                // Resize and show on screen
                if (this.capturedImageBytes != null)
                {
                    var resizedImage = await ImageResizer.ResizeImage(this.capturedImageBytes, 300, 300);

                    this.ScreenShotImage.Source = ImageSource.FromStream(() => new MemoryStream(resizedImage));
                    this.ScreenShotImage.IsVisible = true;

                    this.capturedImageBytes = null;
                }
            }
            catch (Exception ex)
            {
                XyrohLib.LogCrash("Screen Capture Resize", ex);
            }
        }

        private async void OnSendTicketButtonClicked(object sender, EventArgs e)
        {
            try
            {
                XyrohLib.LogEvent("Support Page : Send Ticket");

                this.SendButton.IsEnabled = false;

                // get log and db
                var attachments = new List<string>();
                attachments.Add(XyrohLib.GetLogPath());

                // screencap?
                if (File.Exists(this.captureImagePath))
                {
                    XyrohLib.Log("Attaching: " + this.captureImagePath);
                    attachments.Add(this.captureImagePath);
                }

                var ticketId = await XyrohLib.CreateTicketWithAttachment(this.TicketEmail.Text, this.TicketSubject.Text, this.TicketDescription.Text, new string[] { "urlshortener", "worst", "mobile", "xyrohlib" }, attachments);
                if (ticketId > 0)
                {
                    await this.DisplayAlert("Ticket Created", "Ticket Ref: " + ticketId.ToString() + " Successfully Created", "OK");
                    await this.Navigation.PopModalAsync();
                }
                else
                {
                    throw new Exception("Couldn't create Ticket - Please try again");
                }
            }
            catch (Exception ex)
            {
                XyrohLib.LogCrash(ex);

                await this.DisplayAlert("Error", "Error: " + ex.Message, "OK");
                this.SendButton.IsEnabled = true;
            }
        }

        private async void OnCloseButtonClicked(object sender, EventArgs e)
        {
            try
            {
                XyrohLib.LogEvent("Support Page : Closed");

                await this.Navigation.PopModalAsync();
            }
            catch (Exception ex)
            {
                XyrohLib.LogCrash(ex);
            }
        }
    }
}
