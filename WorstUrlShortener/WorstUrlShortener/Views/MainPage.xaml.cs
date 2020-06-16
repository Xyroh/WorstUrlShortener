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
            try
            {
                Accelerometer.ShakeDetected += this.OnShaked;
                Accelerometer.Start(SensorSpeed.Default);
            }
            catch (FeatureNotSupportedException featEx)
            {
                // for the emulator as not supported
            }

            base.OnAppearing();
        }

        protected override void OnDisappearing()
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

            base.OnDisappearing();
        }

        private async void OnShaked(object sender, EventArgs e)
        {
            XyrohLib.LogEvent("Shake Detected");

            try
            {
                // capture the screen
                var screenImage = await DependencyService.Get<IScreen>().CaptureScreenAsync();
                await this.Navigation.PushModalAsync(new SupportPage("Send Feedback", screenImage));
            }
            catch (Exception ex)
            {
                XyrohLib.LogCrash(ex);
            }
        }
    }
}
