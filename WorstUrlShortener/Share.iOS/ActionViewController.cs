using System;
using CoreFoundation;
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

            var inputItems = this.ExtensionContext.InputItems;
            //var alert = UIAlertController.Create("TEST", $"Context {inputItems[0].AttributedContentText.Value}", UIAlertControllerStyle.Alert);

            //var urlString = string.Empty;
            var urlString = "ZIP, NADA";
            var inputItem = inputItems[0];
            var itemProvider = inputItem.Attachments?[0];
            if(itemProvider !=null && itemProvider.HasItemConformingTo("public.url"))
            {
                var alert = UIAlertController.Create("TEST 1", $"WE HAVE URL {urlString}", UIAlertControllerStyle.Alert);

                PresentViewController(alert, true, () =>
                {
                    itemProvider.LoadItem(UTType.URL,null, ((o, error) =>
                    {
                        /*if (o == null)
                        {
                            return;
                        }*/

                        var url = (NSUrl) o;
                        urlString = url.ToString();

                        var alert2 = UIAlertController.Create("TEST", $"Context {urlString}", UIAlertControllerStyle.Alert);

                        PresentViewController(alert2, true, () =>
                        {
                            DispatchQueue.MainQueue.DispatchAfter(new DispatchTime(DispatchTime.Now, 5000000000), () =>
                            {
                                // Inform the host that we're done, so it un-blocks its UI. Note: Alternatively you could call super's -didSelectPost, which will similarly complete the extension context.
                                ExtensionContext.CompleteRequest(new NSExtensionItem[0], null);
                            });
                        });
                    }));

                });


            }


            /*
            // Initialize Xamarin.Forms framework
            global::Xamarin.Forms.Forms.Init();
            // Create an instance of XF page with associated View Model
            var xfPage = new ShareExtensionPage();
            var viewModel = (ExtensionViewModel)xfPage.BindingContext;
            //viewModel.Message = "Welcome to XF Page created from an iOS Extension";
            // Override the behavior to complete the execution of the Extension when a user press the button
            viewModel.FinishedCommand = new Command(() => DoneClicked(this));
            // Convert XF page to a native UIViewController which can be consumed by the iOS Extension
            var newController = xfPage.CreateViewController();
            // Present new view controller as a regular view controller
            this.PresentModalViewController(newController, false);
            */
        }

        partial void DoneClicked(NSObject sender)
        {
            // Return any edited content to the host app.
            // This template doesn't do anything, so we just echo the passed-in items.
            ExtensionContext.CompleteRequest(ExtensionContext.InputItems, null);
        }
    }
}
