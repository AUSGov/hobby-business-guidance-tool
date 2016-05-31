namespace Sb.Interfaces.Models
{
    public interface IVisitor
    {
        string this[string key] { get; }

        string OutcomeType { get; }

        string PersonaId { get; }
    }
}
