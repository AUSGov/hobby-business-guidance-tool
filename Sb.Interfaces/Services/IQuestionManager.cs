using System.Collections.Generic;
using Sb.Interfaces.Models;

namespace Sb.Interfaces.Services
{
    public interface IQuestionManager
    {
        IQuestion GetNextQuestion(string id = null);
        IQuestion GetPreviousQuestion(string id);
        IQuestion GetQuestion(string id);
        IList<IQuestion> GetAllQuestions();
    }
}
