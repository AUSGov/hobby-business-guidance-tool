using System;
using System.Collections.Generic;
using System.Linq;
using Sb.Interfaces.Models;
using Sb.Interfaces.Services;

namespace Sb.Services.Managers
{
    public class QuestionManager : IQuestionManager
    {
        private readonly IQuestionLoader _questionLoader;

        public QuestionManager(IQuestionLoader questionLoader)
        {
            if (questionLoader == null)
            {
                throw new ArgumentNullException(nameof(questionLoader));
            }

            _questionLoader = questionLoader;
        }

        public IQuestion GetNextQuestion(string id = null)
        {
            if (string.IsNullOrEmpty(id))
            {
                return _questionLoader.GetQuestions().FirstOrDefault();
            }

            var questions = _questionLoader.GetQuestions();
            var currentItem = questions.Single(x => x.Id == id);
            var currentIndex = questions.IndexOf(currentItem);

            return currentIndex + 1 < questions.Count ? questions[currentIndex + 1] : null;
        }

        public IQuestion GetPreviousQuestion(string id)
        {
            var questions = _questionLoader.GetQuestions();
            var currentItem = questions.Single(x => x.Id == id);
            var currentIndex = questions.IndexOf(currentItem);

            return currentIndex - 1 >= 0 ? questions[currentIndex - 1] : null;
        }

        public IQuestion GetQuestion(string id)
        {
            var questions = _questionLoader.GetQuestions();
            return questions.SingleOrDefault(x => x.Id == id);
        }

        public IList<IQuestion> GetAllQuestions()
        {
            return _questionLoader.GetQuestions();
        }
    }
}