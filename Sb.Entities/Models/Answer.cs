using Sb.Interfaces.Enums;
using Sb.Interfaces.Models;

namespace Sb.Entities.Models
{
    public class Answer : IAnswer
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public IndicationType IndicationType { get; set; }

        public string CssClass { get; set; }
    }
}