namespace Sb.Interfaces.Models
{
    public interface IRule
    {
        string MemberName { get; set; }
        string Operator { get; set; }
        string TargetValue { get; set; }
    }
}
