using System;
using System.IO;
using com.xyroh.lib;
using CoreFoundation;
using Foundation;
using Social;
using UIKit;
using WorstUrlShortener;
using WorstUrlShortener.Views;
using Xamarin.Forms;

namespace Share.iOS
{
    public partial class ShareViewController : SLComposeServiceViewController
    {
        protected ShareViewController(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
            XyrohLib.setFileLog(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "sharedebug.txt"), 500000); // 0.5MB
            XyrohLib.setCrashreporter(BaseConfig.SentryKey);
            XyrohLib.setAnalytics(BaseConfig.AppCenteriOSKey, BaseConfig.AppCenterAndroidKey);
            XyrohLib.Log("**HERE**");

            XyrohLib.LogEvent("Extension : Share ");
        }

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Do any additional setup after loading the view.
            // Initialize Xamarin.Forms framework
            global::Xamarin.Forms.Forms.Init();
            // Create an instance of XF page with associated View Model
            var xfPage = new TestPage();
            //var viewModel = (MainPageViewModel)xfPage.BindingContext;
            //viewModel.Message = "Welcome to XF Page created from an iOS Extension";
            // Override the behavior to complete the execution of the Extension when a user press the button
            //viewModel.DoCommand = new Command(() => DoneClicked(this));
            // Convert XF page to a native UIViewController which can be consumed by the iOS Extension
            var newController = xfPage.CreateViewController();
            // Present new view controller as a regular view controller
            this.PresentModalViewController(newController, false);
        }

        public override bool IsContentValid()
        {
            // Do validation of contentText and/or NSExtensionContext attachments here
            return true;
        }

        public override void DidSelectPost()
        {
            System.Diagnostics.Debug.WriteLine("HERE");
            // This is called after the user selects Post. Do the upload of contentText and/or NSExtensionContext attachments.
            var alert = UIAlertController.Create("Share extension", $"This is the step where you should post the ContentText value: '{ContentText}' to your targeted service.", UIAlertControllerStyle.Alert);
            PresentViewController(alert, true, () =>
            {
                DispatchQueue.MainQueue.DispatchAfter(new DispatchTime(DispatchTime.Now, 5000000000), () =>
                {
                    // Inform the host that we're done, so it un-blocks its UI. Note: Alternatively you could call super's -didSelectPost, which will similarly complete the extension context.
                    ExtensionContext.CompleteRequest(new NSExtensionItem[0], null);
                });
            });

            // Inform the host that we're done, so it un-blocks its UI. Note: Alternatively you could call super's -didSelectPost, which will similarly complete the extension context.
            //ExtensionContext.CompleteRequest(new NSExtensionItem[0], null);
        }

        public override SLComposeSheetConfigurationItem[] GetConfigurationItems()
        {
            // To add configuration options via table cells at the bottom of the sheet, return an array of SLComposeSheetConfigurationItem here.
            return new SLComposeSheetConfigurationItem[0];
        }
    }
}
