using System;
using com.xyroh.lib;
using WorstUrlShortener.Interfaces;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WorstUrlShortener.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : TabbedPage
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override void OnAppearing()
        {
            Accelerometer.ShakeDetected += this.OnShaked;
            try
            {
                Accelerometer.Start(SensorSpeed.Default);
            }
            catch
            {
                // for the emulator as not supported
            }

            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            Accelerometer.Stop();
            Accelerometer.ShakeDetected -= this.OnShaked;
            base.OnDisappearing();
        }

        private async void OnShaked(object sender, EventArgs e)
        {
            XyrohLib.LogEvent("Shake Detected");

            try
            {
                // capture the screen
                var screenImage = await DependencyService.Get<IScreen>().CaptureScreenAsync();
                await this.Navigation.PushModalAsync(new SupportPage(screenImage));
            }
            catch (Exception ex)
            {
                XyrohLib.LogCrash(ex);
            }
        }
    }
}
