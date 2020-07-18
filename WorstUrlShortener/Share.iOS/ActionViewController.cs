using System;
using CoreFoundation;
using MobileCoreServices;
using Foundation;
using UIKit;
using WorstUrlShortener.ViewModels;
using WorstUrlShortener.Views;
using Xamarin.Forms;
using System.Diagnostics;

/*
 * TO Debug (breakpoints, output etc)you need to run this as the Startup project in VS
 */

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

            var inputItems = this.ExtensionContext.InputItems;


            global::Xamarin.Forms.Forms.Init();
            var xfPage = new ShareExtensionPage();
            var viewModel = (ExtensionViewModel)xfPage.BindingContext;
            viewModel.FinishedCommand = new Command(() => DoneClicked(this));
            var newController = xfPage.CreateViewController();
            this.PresentModalViewController(newController, false);

            var urlString = string.Empty;
            var inputItem = inputItems[0];
            var itemProvider = inputItem.Attachments?[0];
            if(itemProvider !=null && itemProvider.HasItemConformingTo("public.url"))
            {

                    itemProvider.LoadItem(UTType.URL, null, ((o, error) =>
                    {
                        if (o == null)
                        {
                            return;
                        }

                        var url = (NSUrl) o;
                        urlString = url.AbsoluteString;
                        // Debug.WriteLine("URL: " + urlString);

                        viewModel.LongURL = urlString;
                        // Debug.WriteLine("VM URL: " + viewModel.LongURL);

                        // Can't present the XF View here, just times out, so present earlier and rely on the binding

                    }));

            }

            
        }

        partial void DoneClicked(NSObject sender)
        {
            // Return any edited content to the host app.
            // This template doesn't do anything, so we just echo the passed-in items.
            ExtensionContext.CompleteRequest(ExtensionContext.InputItems, null);
        }
    }
}
