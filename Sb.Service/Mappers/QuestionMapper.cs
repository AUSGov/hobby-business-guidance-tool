using Sb.Interfaces.Mappers;
using Sb.Interfaces.Models;

namespace Sb.Services.Mappers
{
    public class QuestionMapper<T> : IQuestionMapper<T> where T : IQuestion, new()
    {
        public T Map(IQuestion question)
        {
            return new T
            {
                Id = question.Id,
                Title = question.Title,
                Description = question.Description,
                MoreInfo = question.MoreInfo,
                Answer = question.Answer,
                QuestionType = question.QuestionType,
                AnswerList = question.AnswerList,
                IsEnabled = question.IsEnabled,
                QuestionNumber = question.QuestionNumber,
                QuestionCount = question.QuestionCount
            };
        }
    }
}