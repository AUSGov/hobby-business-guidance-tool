using Sb.Interfaces.Enums;

namespace Sb.Interfaces.Models
{
    public interface IPersona
    {
        string Id { get; set; }

        OutcomeType OutcomeType { get; set; }

        IQuestionAnswerPair[] IncludedAnswerList { get; set; }

        IQuestionAnswerPair[] ExcludedAnswerList { get; set; }

        bool IsEnabled { get; set; }
    }
}
