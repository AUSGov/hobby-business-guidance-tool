using System.Collections.Generic;
using System.Collections.Specialized;
using Sb.Interfaces.Models;

namespace Sb.Interfaces.Services
{
    public interface IAnswerManager
    {
        string RetrieveAnswerId(string questionId);

        void StoreAnswerId(string questionId, string answer);

        void StoreAnswers(NameValueCollection nameValueCollection);

        IDictionary<IQuestion, IAnswer[]> GetAnswers();

        NameValueCollection GetNameValueCollection();
    }
}
