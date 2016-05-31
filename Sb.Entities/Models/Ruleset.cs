using Newtonsoft.Json;
using Sb.Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sb.Entities.Models
{
    public class Ruleset : IRuleset
    {
        private List<Func<IVisitor, bool>> _compiledRules;
        private IRule[] _rawRules;

        public string Id { get; set; }

        public string Title { get; set; }

        public string ObligationId { get; set; }

        public bool IsEnabled { get; set; }

        public bool MatchAnyRule { get; set; }

        [JsonConverter(typeof(ConcreteConverter<Rule[]>))]
        public IRule[] Rules
        {
            get
            {
                return _rawRules;
            }
            set
            {
                _rawRules = value;
                _compiledRules = value.Select(x => x.CompileRule<IVisitor>()).ToList();
            }
        }

        public bool IsMatch(IVisitor user)
        {
            return MatchAnyRule ? _compiledRules.Any(r => r(user)) : _compiledRules.All(r => r(user));
        }
    }
}

