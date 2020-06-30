using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using CoreGraphics;
using UIKit;
using WorstUrlShortener.Interfaces;
using WorstUrlShortener.iOS.Services;

[assembly: Xamarin.Forms.Dependency(typeof(IOSResizeImage))]

namespace WorstUrlShortener.iOS.Services
{
    public class IOSResizeImage : IResizeImage
    {
        public byte[] ResizeImage(byte[] imageData, float width, float height)
        {
            var originalImage = ImageFromByteArray(imageData);
            var orientation = originalImage.Orientation;

            // create a 24bit RGB image
            using (var context = new CGBitmapContext(IntPtr.Zero, (int)width, (int)height, 8, 4 * (int)width, CGColorSpace.CreateDeviceRGB(), CGImageAlphaInfo.PremultipliedFirst))
            {
                var imageRect = new RectangleF(0, 0, width, height);

                // draw the image
                context.DrawImage(imageRect, originalImage.CGImage);

                var resizedImage = UIKit.UIImage.FromImage(context.ToImage(), 0, orientation);

                // save the image as a jpeg
                return resizedImage.AsJPEG().ToArray();
            }
        }

        public static UIKit.UIImage ImageFromByteArray(byte[] data)
        {
            if (data == null)
            {
                return null;
            }

            UIKit.UIImage image;
            try
            {
                image = new UIKit.UIImage(Foundation.NSData.FromArray(data));
            }
            catch (Exception e)
            {
                Console.WriteLine("Image load failed: " + e.Message);
                return null;
            }
            return image;
        }
    }
}
