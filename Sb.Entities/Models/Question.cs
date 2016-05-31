using Sb.Interfaces.Models;
using Newtonsoft.Json;
using Sb.Interfaces.Enums;

namespace Sb.Entities.Models
{
    public class Question : IQuestion
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string ShortTitle { get; set; }

        public string Description { get; set; }

        public string MoreInfo { get; set; }

        public string Answer { get; set; }

        public QuestionType QuestionType { get; set; }

        public bool IsEnabled { get; set; }

        public int QuestionNumber { get; set; }

        public int QuestionCount { get; set; }

        [JsonConverter(typeof(ConcreteConverter<Answer[]>))]
        public IAnswer[] AnswerList { get; set; }
    }
}