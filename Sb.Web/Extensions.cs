using System;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Sb.Web
{
    public static class Extensions
    {
        public static bool IsRouteMatch(this Uri uri, HttpContextBase httpContext, string controllerName, string actionName)
        {
            if (uri == null)
            {
                return false;
            }

            if (httpContext == null)
            {
                return false;
            }

            if (httpContext.Request.Url != null && uri.DnsSafeHost != httpContext.Request.Url.DnsSafeHost)
            {
                return false;
            }

            var routeInfo = new RouteInfo(uri, httpContext.Request.ApplicationPath);

            if (routeInfo.RouteData == null)
            {
                return false;
            }

            if (!routeInfo.RouteData.Values.ContainsKey("controller"))
            {
                return false;
            }

            if (!routeInfo.RouteData.Values.ContainsKey("action"))
            {
                return false;
            }

            return routeInfo.RouteData.Values["controller"].ToString() == controllerName &&
                   routeInfo.RouteData.Values["action"].ToString() == actionName;
        }

        public static string GetRouteParameterValue(this Uri uri, HttpContextBase httpContext, string parameterName)
        {
            if (uri == null)
            {
                return string.Empty;
            }

            if (httpContext == null)
            {
                return string.Empty;
            }

            var routeInfo = new RouteInfo(uri, httpContext.Request.ApplicationPath);

            return routeInfo.RouteData.Values[parameterName] != null
                ? routeInfo.RouteData.Values[parameterName].ToString()
                : string.Empty;
        }

        public static string RootSiteUrl(this HtmlHelper helper)
        {
            return HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority;
        }
    }
}