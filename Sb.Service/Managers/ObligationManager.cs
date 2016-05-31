using Sb.Interfaces.Models;
using Sb.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sb.Services.Managers
{
    public class ObligationManager : IObligationManager
    {
        private readonly IObligationLoader _obligationLoader;
        private readonly IRulesetLoader _rulesetLoader;

        public ObligationManager(IObligationLoader obligationLoader, IRulesetLoader rulesetLoader)
        {
            if (obligationLoader == null)
            {
                throw new ArgumentNullException(nameof(obligationLoader));
            }

            if (rulesetLoader == null)
            {
                throw new ArgumentNullException(nameof(rulesetLoader));
            }

            _obligationLoader = obligationLoader;
            _rulesetLoader = rulesetLoader;
        }

        public IList<IObligation> GetObligationsForVisitor(IVisitor visitor)
        {
            var obligationIds = _rulesetLoader.GetRulesets()
                .Where(r => r.IsMatch(visitor))
                .Select(r => r.ObligationId)
                .Distinct();

            return _obligationLoader.GetObligations().Where(o => obligationIds.Contains(o.Id)).ToList();
        }

        public IList<IObligation> GetAllObligations()
        {
            return _obligationLoader.GetObligations();
        }
    }
}

