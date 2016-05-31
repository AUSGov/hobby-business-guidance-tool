using System.Collections.Generic;
using Sb.Interfaces.Enums;

namespace Sb.Interfaces.Models
{
    public interface IObligation
    {
        string Id { get; set; }

        string Title { get; set; }

        string Description { get; set; }

        bool IsEnabled { get; set; }

        ObligationType ObligationType { get; set; }

        ILink Link { get; set; }

    }
}
