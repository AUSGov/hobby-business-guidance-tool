using System.Collections.Generic;
using Sb.Interfaces.Models;

namespace Sb.Interfaces.Services
{
    public interface IObligationLoader
    {
        IList<IObligation> GetObligations();

        void RefreshObligations();
    }
}
