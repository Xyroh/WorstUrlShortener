using System;
using System.Collections.Generic;
using com.xyroh.lib;
using WorstUrlShortener.Interfaces;
using WorstUrlShortener.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WorstUrlShortener.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            this.InitializeComponent();

            this.TableView.Root = new TableRoot("Help");

            var mainSection = new TableSection("URL Shortener");
            this.TableView.Root.Add(mainSection);

            var supportCell = new ImageCell
            {
                Text = "Support & Suggestions >", Detail = "help@xyroh.com", ImageSource = ImageSource.FromFile("help.png")
            };
            supportCell.Tapped += this.OnSupportCellTapped;
            mainSection.Add(supportCell);

            var ideaCell = new ImageCell
            {
                Text = "Feature Requests >",
                Detail = "Got an idea or like us to implement a feature?",
                ImageSource = ImageSource.FromFile("idea.png")
            };
            ideaCell.Tapped += this.OnIdeaCellTapped;
            mainSection.Add(ideaCell);

            var aboutSection = new TableSection("About");
            this.TableView.Root.Add(aboutSection);
            if (Device.RuntimePlatform != "macOS" && Device.RuntimePlatform != "WPF")
            {
                aboutSection.Add(new TextCell
                {
                    Text = "App Version",
                    Detail = VersionTracking.CurrentVersion + "#" + VersionTracking.CurrentBuild,
                    IsEnabled = false
                });
            }

            aboutSection.Add(new TextCell { Text = "Author", Detail = "Xyroh Ltd - www.xyroh.com", IsEnabled = false });

            var releaseNotesCell = new ImageCell
            {
                Text = "What's New? >", Detail = "Latest release notes", ImageSource = ImageSource.FromFile("releasenotes.png")
            };
            releaseNotesCell.Tapped += this.OnReleaseNotesCellTapped;
            aboutSection.Add(releaseNotesCell);
        }

        protected async void OnSupportCellTapped(object sender, EventArgs e)
        {
            XyrohLib.LogEvent("Help : Support : Tapped");

            await this.Navigation.PushAsync(new SupportPage("Raise a Support Ticket"));
        }

        protected async void OnIdeaCellTapped(object sender, EventArgs e)
        {
            XyrohLib.LogEvent("Help : Idea : Tapped");

            await this.Navigation.PushAsync(new SupportPage("Suggest a Feature?"));
        }

        protected async void OnReleaseNotesCellTapped(object sender, EventArgs e)
        {
            XyrohLib.LogEvent("Help : Release Notes : Tapped");

            await this.Navigation.PushAsync(new ReleaseNotesPage());
        }

        protected override void OnAppearing()
        {
            if (Device.RuntimePlatform != "macOS" && Device.RuntimePlatform != "WPF")
            {
                try
                {
                    Accelerometer.ShakeDetected += this.OnShaked;
                    Accelerometer.Start(SensorSpeed.Default);
                }
                catch (FeatureNotSupportedException featEx)
                {
                    // for the emulator as not supported
                }
            }

            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            if (Device.RuntimePlatform != "macOS" && Device.RuntimePlatform != "WPF")
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
