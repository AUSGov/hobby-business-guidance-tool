using Sb.Interfaces.Models;

namespace Sb.Interfaces.Mappers
{
    public interface IQuestionMapper<out T> where T: IQuestion
    {
        T Map(IQuestion question);
    }
}