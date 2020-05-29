using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using com.xyroh.lib;
using Foundation;
using UIKit;

namespace WorstUrlShortener.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();

            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionHandler;
            TaskScheduler.UnobservedTaskException += UnobservedTaskExceptionHandler;

            this.LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }

        static void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = (Exception)e.ExceptionObject;
            XyrohLib.LogException("Unhandled Exception", ex);
            Environment.Exit(1);
        }

        static void UnobservedTaskExceptionHandler(object sender, UnobservedTaskExceptionEventArgs e)
        {
            var ex = (Exception)e.Exception;
            XyrohLib.LogException("Unobserved Task Exception", ex);
        }
    }
}
