using Sb.Interfaces.Enums;

namespace Sb.Interfaces.Models
{
    public interface IQuestion
    {
        string Id { get; set; }

        string Title { get; set; }

        string ShortTitle { get; set; }

        string Description { get; set; }

        string MoreInfo { get; set; }

        string Answer { get; set; }

        QuestionType QuestionType { get; set; }

        IAnswer[] AnswerList { get; set; }

        bool IsEnabled { get; set; }

        int QuestionNumber { get; set; }

        int QuestionCount { get; set; }
    }
}
