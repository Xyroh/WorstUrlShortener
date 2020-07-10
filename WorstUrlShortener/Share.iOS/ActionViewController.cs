using System;

using MobileCoreServices;
using Foundation;
using UIKit;
using WorstUrlShortener.ViewModels;
using WorstUrlShortener.Views;
using Xamarin.Forms;

namespace Action.iOS
{
    public partial class ActionViewController : UIViewController
    {
        protected ActionViewController(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
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

            // Initialize Xamarin.Forms framework
            global::Xamarin.Forms.Forms.Init();
            // Create an instance of XF page with associated View Model
            var xfPage = new TestPage();
            var viewModel = (ExtensionViewModel)xfPage.BindingContext;
            viewModel.Message = "Welcome to XF Page created from an iOS Extension";
            // Override the behavior to complete the execution of the Extension when a user press the button
            viewModel.FinishedCommand = new Command(() => DoneClicked(this));
            // Convert XF page to a native UIViewController which can be consumed by the iOS Extension
            var newController = xfPage.CreateViewController();
            // Present new view controller as a regular view controller
            this.PresentModalViewController(newController, false);
        }

        partial void DoneClicked(NSObject sender)
        {
            // Return any edited content to the host app.
            // This template doesn't do anything, so we just echo the passed-in items.
            ExtensionContext.CompleteRequest(ExtensionContext.InputItems, null);
        }
    }
}
