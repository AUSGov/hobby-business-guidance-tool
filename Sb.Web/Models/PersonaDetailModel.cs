using System.Collections.Generic;
using Sb.Interfaces.Models;

namespace Sb.Web.Models
{
    public class PersonaDetailModel
    {
        public IList<IPersona> Personas { get; set; }
        public IList<IQuestion> Questions { get; set; }
    }
}