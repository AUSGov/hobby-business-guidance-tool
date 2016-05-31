using Sb.Interfaces.Models;

namespace Sb.Entities.Models
{
    public class Rule : IRule
    {
        public string MemberName { get; set; }

        public string Operator { get; set; }

        public string TargetValue { get; set; }
    }
}