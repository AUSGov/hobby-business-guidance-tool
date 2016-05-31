using Sb.Interfaces.Enums;

namespace Sb.Interfaces.Models
{
    public interface IAnswer
    {
        string Id { get; set; }

        string Title { get; set; }

        string Description { get; set; }

        IndicationType IndicationType { get; set; }

        string CssClass { get; set; }
    }
}
