using System.Collections.Generic;
using Sb.Interfaces.Models;

namespace Sb.Interfaces.Services
{
    public interface IPersonaLoader
    {
        IList<IPersona> GetPersonas();

        void RefreshPersonas();
    }
}