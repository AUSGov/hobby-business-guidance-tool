using Sb.Interfaces.Models;
using System.Collections.Generic;

namespace Sb.Interfaces.Services
{
    public interface IObligationManager
    {
        IList<IObligation> GetObligationsForVisitor(IVisitor visitor);
        IList<IObligation> GetAllObligations();
    }
}
