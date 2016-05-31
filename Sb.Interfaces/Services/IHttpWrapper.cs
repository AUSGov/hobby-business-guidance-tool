using System;
using System.Collections.Specialized;
using System.Web;

namespace Sb.Interfaces.Services
{
    public interface IHttpWrapper
    {
        NameValueCollection QueryString { get; }

        NameValueCollection Form { get; }

        Uri UrlReferrer { get; }

        HttpContextBase HttpContext { get; }
    }
}