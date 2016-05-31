using System;
using System.Collections.Specialized;
using System.Web;
using Sb.Interfaces.Services;

namespace Sb.Services.Web
{
    public class HttpWrapper : IHttpWrapper
    {
        public HttpContextBase HttpContext => HttpContextProvider.Current;

        public NameValueCollection QueryString => HttpContext.Request.QueryString;

        public NameValueCollection Form => HttpContext.Request.Form;

        public Uri UrlReferrer => HttpContext.Request.UrlReferrer;
    }
}