using System;
using System.IO;
using Newtonsoft.Json;
using Sb.Interfaces.Services;

namespace Sb.Services.Loaders
{
    public abstract class FileLoader
    {
        protected ICacheManager CacheManager { get; }
        protected string DirectoryPath { get; }
        protected string CacheName { get; }

        protected FileLoader(ICacheManager cacheManager, IFilePathResolver filePathResolver, string directoryName, string cacheName)
        {
            if (cacheManager == null)
            {
                throw new ArgumentNullException(nameof(cacheManager));
            }

            if (filePathResolver == null)
            {
                throw new ArgumentNullException(nameof(filePathResolver));
            }

            if (string.IsNullOrEmpty(cacheName))
            {
                throw new ArgumentNullException(nameof(cacheName));
            }

            CacheManager = cacheManager;
            DirectoryPath = filePathResolver.GetWorkingDirectory() + "\\" + directoryName;
            CacheName = cacheName;
        }

        protected string[] GetJsonFiles()
        {
            return Directory.GetFiles(DirectoryPath, "*.json", SearchOption.AllDirectories);
        }

        public T Deserialise<T>(string filePath)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(File.ReadAllText(filePath));
            }
            catch (Exception ex)
            {
                throw new Exception("Error in file: " + Path.GetFileName(filePath) + " ("+ ex.Message + ")", ex);
            }
        }

        protected static string LoadHtmlFromFile(string parentFilePath, string id, string fieldName)
        {
            var directoryName = Path.GetDirectoryName(parentFilePath);
            var htmlFilePath = $@"{directoryName}\_{fieldName}\{id}.html";

            return File.Exists(htmlFilePath) ? File.ReadAllText(htmlFilePath) : null;
        }
    }
}

