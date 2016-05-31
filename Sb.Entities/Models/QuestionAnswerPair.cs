using Sb.Interfaces.Models;

namespace Sb.Entities.Models
{
    public class QuestionAnswerPair : IQuestionAnswerPair
    {
        public string QuestionId { get; set; }

        public string Answer { get; set; }
    }
}