using System.Web;

namespace Sb.Interfaces.Services
{
    public interface ICookieContext
    {
        HttpCookie GetCookie(string cookieName);
        void SetCookie(HttpCookie cookie);
        void RemoveCookie(string cookieName);
    }
}