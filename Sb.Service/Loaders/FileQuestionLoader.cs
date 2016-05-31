using System.Collections.Generic;
using System.Linq;
using Sb.Entities.Models;
using Sb.Interfaces.Models;
using Sb.Interfaces.Services;

namespace Sb.Services.Loaders
{
    public class FileQuestionLoader : FileLoader, IQuestionLoader
    {
        public FileQuestionLoader(ICacheManager cacheManager, IFilePathResolver filePathResolver, string directoryName, string cacheName) : base(cacheManager, filePathResolver, directoryName, cacheName)
        {
        }

        public IList<IQuestion> GetQuestions()
        {
            if (!CacheManager.Contains(CacheName))
            {
                RefreshQuestions();
            }

            return CacheManager.Get(CacheName) as List<IQuestion>;
        }

        public void RefreshQuestions()
        {
            var result = new List<IQuestion>();
            var jsonFiles = GetJsonFiles();

            var questionNumber = 0;

            foreach (var filePath in jsonFiles)
            {
                var question = Deserialise<Question>(filePath);

                if (question.IsEnabled)
                {
                    if (string.IsNullOrEmpty(question.Description))
                    {
                        question.Description = LoadHtmlFromFile(filePath, question.Id, "Description");
                    }

                    if (string.IsNullOrEmpty(question.MoreInfo))
                    {
                        question.MoreInfo = LoadHtmlFromFile(filePath, question.Id, "MoreInfo");
                    }

                    question.QuestionNumber = ++questionNumber;

                    result.Add(question);
                }
            }

            CacheManager.Add(CacheName, result.Select(q => { q.QuestionCount = result.Count; return q; }).ToList());
        }
    }
}

