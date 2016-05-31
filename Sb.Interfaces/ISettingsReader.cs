namespace Sb.Interfaces
{
    public interface ISettingsReader
    {
        string this[string key] { get; }

        int ReadInt(string key, int defaultIfAbsent);

        bool ReadBool(string key, bool defaultIfAbsent);
    }
}
