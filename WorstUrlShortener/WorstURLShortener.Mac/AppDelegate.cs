using System;
using System.Threading.Tasks;
using AppKit;
using Foundation;
using com.xyroh.lib;
using WorstUrlShortener;
using WorstUrlShortener.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.MacOS;

namespace WorstURLShortener.Mac
{
    [Register("AppDelegate")]
    public class AppDelegate : NSApplicationDelegate
    {
        private NSStatusItem statusBarItem;
        private NSMenu menu;
        private NSViewController mainPage;

        public AppDelegate()
        {
            global::Xamarin.Forms.Forms.Init();
            CreateStatusItem();
            Application.SetCurrentApplication(new App(string.Empty));
        }

        private void CreateStatusItem()
        {
            // Create the status bar item
            var statusBar = NSStatusBar.SystemStatusBar;
            this.statusBarItem = statusBar.CreateStatusItem(NSStatusItemLength.Variable);
            this.statusBarItem.Button.Image = NSImage.ImageNamed("TrayIcon");
            this.statusBarItem.Button.Action = new ObjCRuntime.Selector("MenuAction:");

            // Listen to touches on the status bar item
            this.statusBarItem.Button.SendActionOn(NSEventType.OtherMouseUp);
            this.statusBarItem.Button.Activated += this.StatusBarActivated;

            // Create the menu that gets opened on a right click
            this.menu = new NSMenu();
            var closeAppItem = new NSMenuItem("Close");
            closeAppItem.Activated += this.CloseMenuActivated;
            this.menu.AddItem(closeAppItem);
            //this.statusBarItem.Menu = this.menu;
        }

        private void CloseMenuActivated(object sender, EventArgs e)
        {
            NSApplication.SharedApplication.Terminate(this);
        }

        private void StatusBarActivated(object sender, EventArgs e)
        {
            var currentEvent = NSApplication.SharedApplication.CurrentEvent;
            switch (currentEvent.Type)
            {
                case NSEventType.LeftMouseDown:
                    this.ShowWindow();
                    break;
                case NSEventType.RightMouseDown:
                    this.statusBarItem.PopUpStatusItemMenu(this.menu);
                    break;
            }
        }

        private void ShowWindow()
        {
            XyrohLib.LogEvent("Showing Window","Mac");
            if(this.mainPage == null)
            {
                // If you dont need a navigation bar, just use this line
                //this.mainPage = Application.Current.MainPage.CreateViewController();
                var xfPage = new DefaultPage();
                this.mainPage = xfPage.CreateViewController();
                this.mainPage.View.Frame = new CoreGraphics.CGRect(0, 0, 400, 500);

                Application.Current.SendStart();
            }
            else
            {
                Application.Current.SendResume();
            }

            var popover = new NSPopover
            {
                ContentViewController = this.mainPage,
                Behavior = NSPopoverBehavior.Transient,
                Delegate = new PopoverDelegate(),
            };
            popover.Show(this.statusBarItem.Button.Bounds, this.statusBarItem.Button, NSRectEdge.MaxYEdge);
        }

        public class PopoverDelegate : NSPopoverDelegate
        {
            public override void DidClose(NSNotification notification)
            {
                Application.Current.SendSleep();
            }
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
