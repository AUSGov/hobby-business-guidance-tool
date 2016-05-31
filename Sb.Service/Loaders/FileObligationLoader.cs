using System.Collections.Generic;
using Sb.Entities.Models;
using Sb.Interfaces.Models;
using Sb.Interfaces.Services;

namespace Sb.Services.Loaders
{
    public class FileObligationLoader : FileLoader, IObligationLoader
    {
        public FileObligationLoader(ICacheManager cacheManager, IFilePathResolver filePathResolver, string directoryName, string cacheName) : base(cacheManager, filePathResolver, directoryName, cacheName)
        {
        }

        public IList<IObligation> GetObligations()
        {
            if (!CacheManager.Contains(CacheName))
            {
                RefreshObligations();
            }

            return CacheManager.Get(CacheName) as List<IObligation>;            
        }

        public void RefreshObligations()
        {
            var result = new List<IObligation>();
            var jsonFiles = GetJsonFiles();

            foreach (var filePath in jsonFiles)
            {
                var obligation = Deserialise<Obligation>(filePath);

                if (obligation.IsEnabled)
                {
                    if (string.IsNullOrEmpty(obligation.Description))
                    {
                        obligation.Description = LoadHtmlFromFile(filePath, obligation.Id, "Description");
                    }

                    result.Add(obligation);
                }
            }

            CacheManager.Add(CacheName, result);
        }
    }
}

