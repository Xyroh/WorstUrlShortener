using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorstUrlShortener.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WorstUrlShortener.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReleaseNotesPage : ContentPage
    {
        public ReleaseNotesPage()
        {
            this.InitializeComponent();

            this.ReleaseNotesEditor.Text = SettingsViewModel.ReleaseNotesMonitor;
        }
    }
}
