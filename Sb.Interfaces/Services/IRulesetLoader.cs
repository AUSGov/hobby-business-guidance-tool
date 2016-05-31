using System.Collections.Generic;
using Sb.Interfaces.Models;

namespace Sb.Interfaces.Services
{
    public interface IRulesetLoader
    {
        IList<IRuleset> GetRulesets();

        void RefreshRulesets();
    }
}
