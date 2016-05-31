namespace Sb.Interfaces.Models
{
    public interface IQuestionAnswerPair
    {
        string QuestionId { get; set; }
        string Answer { get; set; }
    }
}