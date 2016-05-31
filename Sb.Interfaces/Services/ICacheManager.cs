namespace Sb.Interfaces.Services
{
    public interface ICacheManager
    {
        bool Contains(string key);

        object Get(string key);

        void Add(string key, object value);
    }
}
