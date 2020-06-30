using System.IO;
using System.Threading.Tasks;
using Android.Graphics;
using WorstUrlShortener.Droid.Services;
using WorstUrlShortener.Interfaces;

[assembly: Xamarin.Forms.Dependency(typeof(DroidResizeImage))]

namespace WorstUrlShortener.Droid.Services
{
    public class DroidResizeImage : IResizeImage
    {
        public byte[] ResizeImage(byte[] imageData, float width, float height)
        {
            // Load the bitmap
            var originalImage = BitmapFactory.DecodeByteArray(imageData, 0, imageData.Length);
            var resizedImage = Bitmap.CreateScaledBitmap(originalImage, (int) width, (int) height, false);

            using (var ms = new MemoryStream())
            {
                resizedImage.Compress(Bitmap.CompressFormat.Jpeg, 100, ms);
                return ms.ToArray();
            }
        }
    }
}
