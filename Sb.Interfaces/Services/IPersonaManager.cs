using System.Collections.Generic;
using Sb.Interfaces.Enums;
using Sb.Interfaces.Models;

namespace Sb.Interfaces.Services
{
    public interface IPersonaManager
    {
        OutcomeType GetOutcomeType(IDictionary<string, IAnswer[]> answers);

        IPersona GetPersona(IDictionary<string, IAnswer[]> answers);

        IList<IPersona> GetAllPersonas();
    }
}