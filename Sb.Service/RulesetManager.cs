using Sb.Entities.Models;
using Sb.Interfaces.Models;
using Sb.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;

/*
    http://stackoverflow.com/questions/6488034/how-to-implement-a-rule-engine
    https://msdn.microsoft.com/en-us/library/system.linq.expressions.binaryexpression(v=vs.110).aspx
*/

namespace Sb.Services
{

    public class RulesetManager : IRulesetManager
    {
        private readonly IRulesetLoader _ruleLoader;

        public RulesetManager(IRulesetLoader ruleLoader)
        {
            if (ruleLoader == null)
            {
                throw new ArgumentNullException(nameof(ruleLoader));
            }
            _ruleLoader = ruleLoader;
        }

        public IList<IRuleset> GetRulesets()
        {
            return _ruleLoader.GetRulesets();
        }

        public IList<IRuleset> GetRulesetsForObligation(IObligation obligation)
        {
            return _ruleLoader.GetRulesets().Where(x => x.Target == obligation.Id).ToList();
        }
    }
}

