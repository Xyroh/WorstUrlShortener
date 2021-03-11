using System;
using System.Threading.Tasks;
using Foundation;
using Share.iOS.Services;
using WorstUrlShortener.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(ShareDBPath))]
namespace Share.iOS.Services
{
    public class ShareDBPath : IDBPath
    {
        public string GetDBPath()
        {
            var dbPath = string.Empty;

            var FileManager = new NSFileManager();
            var appGroupContainer = FileManager.GetContainerUrl("group.com.xyroh.worst.urlshortener");
            dbPath = appGroupContainer.Path;

            return dbPath;
        }
    }

}
