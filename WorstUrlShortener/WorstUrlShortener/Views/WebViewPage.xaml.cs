using System;
using System.Collections.Generic;

using com.xyroh.lib;

using Xamarin.Forms;

namespace WorstUrlShortener.Views
{
    public partial class WebViewPage
    {
        public Button PopupButton { get { return popupButton; } }

        public String Url { get; set; }

        public WebViewPage(string url)
        {
            XyrohLib.LogEvent("Page : Web View", "Page");

            this.Url = url;
            InitializeComponent();

            webView.Source = this.Url;

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            PopupButton.Clicked += (o, s) =>
            {

                XyrohLib.LogEvent("FAQ : Closed", "Event");

                Navigation.PopModalAsync();
            };
        }

    }
}
