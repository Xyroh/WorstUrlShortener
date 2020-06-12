using System;
using System.Collections.Generic;
using com.xyroh.lib;
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

            var mainSection = new TableSection("Server Monitor");
            this.TableView.Root.Add(mainSection);

            var supportCell = new ImageCell
            {
                Text = "Support & Suggestions >", Detail = "help@xyroh.com", ImageSource = "Help"
            };
            supportCell.Tapped += this.OnSupportCellTapped;
            mainSection.Add(supportCell);

            var ideaCell = new ImageCell
            {
                Text = "Feature Requests >",
                Detail = "Got an idea or like us to implement a feature?",
                ImageSource = "Idea"
            };
            ideaCell.Tapped += this.OnSupportCellTapped;
            mainSection.Add(ideaCell);

            var aboutSection = new TableSection("About");
            this.TableView.Root.Add(aboutSection);
            aboutSection.Add(new TextCell
            {
                Text = "App Version",
                Detail = VersionTracking.CurrentVersion + "#" + VersionTracking.CurrentBuild,
                IsEnabled = false
            });
            aboutSection.Add(new TextCell { Text = "Author", Detail = "Xyroh Ltd - www.xyroh.com", IsEnabled = false });

            var releaseNotesCell = new ImageCell
            {
                Text = "What's New? >", Detail = "Latest release notes", ImageSource = "ReleaseNotes"
            };
            releaseNotesCell.Tapped += this.OnReleaseNotesCellTapped;
            aboutSection.Add(releaseNotesCell);
        }

        protected async void OnSupportCellTapped(object sender, EventArgs e)
        {
            XyrohLib.LogEvent("Help : Support : Tapped");

            await this.Navigation.PushAsync(new SupportPage());
        }

        protected async void OnReleaseNotesCellTapped(object sender, EventArgs e)
        {
            XyrohLib.LogEvent("Help : Release Notes : Tapped");

            await this.Navigation.PushAsync(new ReleaseNotesPage());
        }
    }
}
