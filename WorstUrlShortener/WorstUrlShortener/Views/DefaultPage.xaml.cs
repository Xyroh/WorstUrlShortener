using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

            URLShortener.SelectedIndex = 0;
        }


        private async void OnShortenButtonClicked(object sender, EventArgs e)
        {
            try
            {
                XyrohLib.LogEvent("Shorten Page : Send Ticket");

                ShortenButton.IsEnabled = false;

                ShortURL.IsVisible = true;
                ShortURL.Text = URLShortener.SelectedItem.ToString() + " - " + FullURL.Text;
            }
            catch (Exception ex)
            {
                XyrohLib.LogCrash(ex);

                await this.DisplayAlert("Error", "Error: " + ex.Message, "OK");
                this.ShortenButton.IsEnabled = true;
            }
        }
    }
}
