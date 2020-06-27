using System;
using System.Threading.Tasks;
using Android.App;
using Android.Runtime;
using com.xyroh.lib;
using Plugin.CurrentActivity;

namespace WorstUrlShortener.Droid
{
    #if DEBUG
    [Application(Debuggable = true)]
    #else
	[Application(Debuggable = false)]
    #endif
    public class MainApplication : Application
    {
        public MainApplication(IntPtr handle, JniHandleOwnership transer)
            : base(handle, transer)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();

            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionHandler;
            TaskScheduler.UnobservedTaskException += UnobservedTaskExceptionHandler;
            AndroidEnvironment.UnhandledExceptionRaiser += UnhandledAndroidEnvironmentExceptionHandler;

            CrossCurrentActivity.Current.Init(this);
        }

        private static void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = (Exception)e.ExceptionObject;
            XyrohLib.LogException("Unhandled Exception", ex);
        }

        private static void UnobservedTaskExceptionHandler(object sender, UnobservedTaskExceptionEventArgs e)
        {
            var ex = (Exception)e.Exception;
            XyrohLib.LogException("Unobserved Task Exception", ex);
        }

        private static void UnhandledAndroidEnvironmentExceptionHandler(object sender, RaiseThrowableEventArgs e)
        {
            var ex = (Exception)e.Exception;
            XyrohLib.LogException("Unhandled Android Environment Task Exception", ex);
        }
    }
}
