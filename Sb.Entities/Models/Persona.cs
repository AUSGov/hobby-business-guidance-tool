using Newtonsoft.Json;
using Sb.Interfaces.Enums;
using Sb.Interfaces.Models;

namespace Sb.Entities.Models
{
    public class Persona : IPersona
    {
        public Persona()
        {
            IncludedAnswerList = new IQuestionAnswerPair[] {};
            ExcludedAnswerList = new IQuestionAnswerPair[] {};
        }

        public string Id { get; set; }

        public OutcomeType OutcomeType { get; set; }

        [JsonConverter(typeof(ConcreteConverter<QuestionAnswerPair[]>))]
        public IQuestionAnswerPair[] IncludedAnswerList { get; set; }

        [JsonConverter(typeof(ConcreteConverter<QuestionAnswerPair[]>))]
        public IQuestionAnswerPair[] ExcludedAnswerList { get; set; }

        public bool IsEnabled { get; set; }
    }
}