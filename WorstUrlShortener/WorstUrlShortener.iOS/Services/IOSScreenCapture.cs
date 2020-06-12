using System;
using System.Threading.Tasks;
using UIKit;
using WorstUrlShortener.iOS.Services;

[assembly: Xamarin.Forms.Dependency(typeof(iOSScreenCapture))]

namespace WorstUrlShortener.iOS.Services
{
    public class iOSScreenCapture
    {
        public async Task<byte[]> CaptureScreenAsync()
        {
            var view = UIApplication.SharedApplication.KeyWindow.RootViewController.View;

            UIGraphics.BeginImageContext(view.Frame.Size);
            view.DrawViewHierarchy(view.Frame, true);
            var image = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();

            using (var imageData = image.AsPNG())
            {
                var bytes = new byte[imageData.Length];
                System.Runtime.InteropServices.Marshal.Copy(imageData.Bytes, bytes, 0, Convert.ToInt32(imageData.Length));
                return bytes;
            }
        }
    }
}
