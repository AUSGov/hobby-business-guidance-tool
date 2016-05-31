using Sb.Interfaces.Models;

namespace Sb.Entities.Models
{
    public class Link : ILink
    {
        public string Url { get; set; }

        public string Text { get; set; }
    }
}