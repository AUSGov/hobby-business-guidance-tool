using System.Collections.Generic;
using System.Linq;
using Sb.Interfaces.Models;
using Sb.Entities.Models;
using Sb.Interfaces.Services;

namespace Sb.Services.Loaders
{
    public class FileRulesetLoader : FileLoader, IRulesetLoader
    {
        public FileRulesetLoader(ICacheManager cacheManager, IFilePathResolver filePathResolver, string directoryName, string cacheName) : base(cacheManager, filePathResolver, directoryName, cacheName)
        {
        }

        public IList<IRuleset> GetRulesets()
        {
            if (!CacheManager.Contains(CacheName))
            {
                RefreshRulesets();
            }

            return CacheManager.Get(CacheName) as IList<IRuleset>;
        }

        public void RefreshRulesets()
        {
            var jsonFiles = GetJsonFiles();

            var result = jsonFiles.Select(Deserialise<Ruleset>).Where(ruleset => ruleset.IsEnabled).Cast<IRuleset>().ToList();

            CacheManager.Add(CacheName, result);
        }
    }
}

