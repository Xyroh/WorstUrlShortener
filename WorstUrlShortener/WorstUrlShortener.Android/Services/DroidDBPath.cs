using System;
using System.IO;
using System.Threading.Tasks;
using WorstUrlShortener.Droid.Services;
using WorstUrlShortener.Interfaces;
using Xamarin.Essentials;

[assembly: Xamarin.Forms.Dependency(typeof(DroidDBPath))]

namespace WorstUrlShortener.Droid.Services
{
    public class DroidDBPath : IDBPath
    {
        public string GetDBPath()
        {
            var dbPath = FileSystem.AppDataDirectory;

            return dbPath;
        }
    }
}
