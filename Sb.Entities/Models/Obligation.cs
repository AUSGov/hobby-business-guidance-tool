using Newtonsoft.Json;
using Sb.Interfaces.Enums;
using Sb.Interfaces.Models;

namespace Sb.Entities.Models
{
    public class Obligation : IObligation
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public ObligationType ObligationType { get; set; }

        [JsonConverter(typeof(ConcreteConverter<Link>))]
        public ILink Link { get; set; }

        public bool IsEnabled { get; set; }

    }
}

