using System.Collections.Generic;
using System.Linq;
using Sb.Interfaces.Models;
using Sb.Entities.Models;
using Sb.Interfaces.Services;

namespace Sb.Services.Loaders
{
    public class FilePersonaLoader : FileLoader, IPersonaLoader
    {
        public FilePersonaLoader(ICacheManager cacheManager, IFilePathResolver filePathResolver, string directoryName, string cacheName) : base(cacheManager, filePathResolver, directoryName, cacheName)
        {
        }

        public IList<IPersona> GetPersonas()
        {
            if (!CacheManager.Contains(CacheName))
            {
                RefreshPersonas();
            }

            return CacheManager.Get(CacheName) as IList<IPersona>;
        }

        public void RefreshPersonas()
        {
            var jsonFiles = GetJsonFiles();

            var result = jsonFiles.Select(Deserialise<Persona>).Where(persona => persona.IsEnabled).Cast<IPersona>().ToList();

            CacheManager.Add(CacheName, result);
        }
    }
}

