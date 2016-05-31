using System.Collections.Generic;
using Sb.Interfaces.Models;

namespace Sb.Interfaces.Services
{
    public interface IQuestionLoader
    {
        IList<IQuestion> GetQuestions();

        void RefreshQuestions();
    }
}
