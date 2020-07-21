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
    public partial class ShareExtensionPage : ContentPage
    {
        private ExtensionViewModel viewModel = new ExtensionViewModel();

        public ShareExtensionPage()
        {
            XyrohLib.LogEvent("Page : Share Extension ");

            this.BindingContext = this.viewModel;
            this.InitializeComponent();

        }


        private void OnFullURLFocused(object sender, FocusEventArgs e)
        {
            // reset page as assume new request
            this.viewModel.HasResults = false;
            this.viewModel.ShortURL = string.Empty;
        }

        /*private void showResults()
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
        }*/

    }
}
