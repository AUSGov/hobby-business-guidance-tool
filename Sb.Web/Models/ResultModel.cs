using System.Collections.Generic;
using Sb.Interfaces.Enums;
using Sb.Interfaces.Models;

namespace Sb.Web.Models
{
    public class ResultModel
    {
        public IList<IObligation> Obligations { get; set; }
        public IDictionary<IQuestion, IAnswer[]> Answers { get; set; }
        public OutcomeType OutcomeType { get; set; }
    }
}