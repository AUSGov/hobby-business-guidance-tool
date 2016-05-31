namespace Sb.Interfaces.Models
{
    public interface IRuleset
    {
        string Id { get; set; }

        string Title { get; set; }

        string ObligationId { get; set; }

        bool IsEnabled { get; set; }

        IRule[] Rules { get;  set; }

        bool MatchAnyRule { get; set; }

        bool IsMatch(IVisitor visitor);
    }
}
