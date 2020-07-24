using System;
using System.Threading.Tasks;
using AppKit;
using Foundation;
using com.xyroh.lib;

namespace WorstURLShortener.Mac
{
    [Register("AppDelegate")]
    public class AppDelegate : NSApplicationDelegate
    {
        public AppDelegate()
        {
        }

        public override void DidFinishLaunching(NSNotification notification)
        {
            // Insert code here to initialize your application
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionHandler;
            TaskScheduler.UnobservedTaskException += UnobservedTaskExceptionHandler;

        }

        public override void WillTerminate(NSNotification notification)
        {
            // Insert code here to tear down your application
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
