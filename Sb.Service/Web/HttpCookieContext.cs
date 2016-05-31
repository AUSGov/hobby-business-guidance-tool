using System;
using System.Web;
using Sb.Interfaces.Services;

namespace Sb.Services.Web
{
    public class HttpCookieContext : ICookieContext
    {
        public HttpCookie GetCookie(string cookieName)
        {
            return HttpContextProvider.Current == null ? null : HttpContextProvider.Current.Request.Cookies[cookieName];
        }

        /// <summary>
        /// Adds configurable path of CookiePath to the cookie, to avoid different portals mixing up cookies.
        /// </summary>
        /// <param name="cookie"></param>
        public void SetCookie(HttpCookie cookie)
        {
            cookie.Path = CookiePath;
            HttpContextProvider.Current.Response.Cookies.Add(cookie);
        }

        public void RemoveCookie(string cookieName)
        {
            var cookie = GetCookie(cookieName);

            if (cookie == null) return;

            var newCookie = new HttpCookie(cookieName) { Path = CookiePath, Expires = DateTime.Now.AddYears(-1), HttpOnly = true };
            SetCookie(newCookie);
        }

        private static string CookiePath => HttpContextProvider.Current.Request.ApplicationPath;
    }
}
