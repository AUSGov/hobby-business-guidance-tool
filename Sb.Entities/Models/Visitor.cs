using Sb.Interfaces.Models;
using System.Collections.Generic;
using System.Linq;
using Sb.Interfaces.Enums;

namespace Sb.Entities.Models
{
    public class Visitor : IVisitor
    {
        private readonly OutcomeType _outcomeType;

        public Visitor(OutcomeType outcomeType, string personaId, IDictionary<string, IAnswer[]> answers)
        {
            _outcomeType = outcomeType;
            Items = answers;
            PersonaId = personaId;
        }

        public IDictionary<string, IAnswer[]> Items { get; }

        public string this[string key] => Items.ContainsKey(key) ? string.Join(",", Items[key].Select(x => x.Id)) : string.Empty;

        public string OutcomeType => _outcomeType.ToString();

        public string PersonaId { get; }
    }
}