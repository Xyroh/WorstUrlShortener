using System;
using System.IO;
using System.Threading.Tasks;
using com.xyroh.lib;
using Foundation;
using WorstUrlShortener.Interfaces;
using WorstUrlShortener.iOS.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Dependency(typeof(IOSDBPath))]
namespace WorstUrlShortener.iOS.Services
{
    
    public class IOSDBPath : IDBPath
    {
        public string GetDBPath()
        {

            var dbPath = Path.Combine(FileSystem.AppDataDirectory, BaseConfig.dbName);

            var FileManager = new NSFileManager();
            var appGroupContainer = FileManager.GetContainerUrl("group.com.xyroh.worst.urlshortener");
            
            if(appGroupContainer != null)
            {
                // XyrohLib.Log(" CONT:" + appGroupContainer.AbsoluteString);
                dbPath = appGroupContainer.Path;
                // yrohLib.Log(" PATH:" + dbPath);
            }
            

            return dbPath;
        }
    }

}
