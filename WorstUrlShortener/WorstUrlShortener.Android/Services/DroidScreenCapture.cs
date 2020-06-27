using System.IO;
using System.Threading.Tasks;
using Android.Graphics;
using WorstUrlShortener.Droid.Services;
using WorstUrlShortener.Interfaces;

[assembly: Xamarin.Forms.Dependency(typeof(DroidScreenCapture))]

namespace WorstUrlShortener.Droid.Services
{
    public class DroidScreenCapture : IScreen
    {
        public async Task<byte[]> CaptureScreenAsync()
        {
            var activity = Xamarin.Forms.Forms.Context as MainActivity;
            if (activity == null)
            {
                return null;
            }

            var view = activity.Window.DecorView;
            view.DrawingCacheEnabled = true;
            var bitmap = view.GetDrawingCache(true);
            byte[] bitmapData;
            using (var stream = new MemoryStream())
            {
                bitmap.Compress(Bitmap.CompressFormat.Png, 0, stream);
                bitmapData = stream.ToArray();
            }

            return bitmapData;
        }
    }
}
