using System;
using System.Collections.Generic;
using com.xyroh.lib;
using WorstUrlShortener.ViewModels;
using Xamarin.Forms;

namespace WorstUrlShortener.Views
{
    public partial class HistoryPage : ContentPage
    {
        private ShortenViewModel viewModel = new ShortenViewModel();

        public HistoryPage()
        {
            XyrohLib.LogEvent("Page : History ");

            this.BindingContext = this.viewModel;
            this.InitializeComponent();


            foreach(var hist in this.viewModel.History)
            {
                XyrohLib.Log("HIST: " + hist.ShortUrl);
            }
        }
    }
}
