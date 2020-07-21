using System;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using com.xyroh.lib;
using WorstUrlShortener.Messages;
using Xamarin.Forms;

namespace WorstUrlShortener.Droid
{
    [IntentFilter(new[] {Android.Content.Intent.ActionSend}, Categories = new[] { Android.Content.Intent.CategoryDefault }, DataMimeTypes = new[] { "text/plain" })]
    [Activity(Label = "WorstUrlShortener", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            var sharedURL = string.Empty;

            if (this.Intent.Action == Android.Content.Intent.ActionSend)
            {
                XyrohLib.LogEvent("Android Share Intent Received");
                sharedURL = this.Intent.Extras.GetString(Android.Content.Intent.ExtraText);
            }

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            this.LoadApplication(new App(sharedURL));


        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
