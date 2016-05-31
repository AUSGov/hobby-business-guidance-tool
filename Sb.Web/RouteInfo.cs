using System;
using System.Web;
using System.Web.Routing;

namespace Sb.Web
{
    public class RouteInfo
    {
        public RouteInfo(Uri uri, string applicaionPath)
        {
            RouteData = RouteTable.Routes.GetRouteData(new InternalHttpContext(uri, applicaionPath));
        }

        public RouteData RouteData { get; private set; }

        private class InternalHttpContext : HttpContextBase
        {
            public InternalHttpContext(Uri uri, string applicationPath)
            {
                Request = new InternalRequestContext(uri, applicationPath);
            }

            public override HttpRequestBase Request { get; }
        }

        private class InternalRequestContext : HttpRequestBase
        {
            private readonly string _appRelativePath;

            public InternalRequestContext(Uri uri, string applicationPath)
            {
                PathInfo = string.Empty;

                if (string.IsNullOrEmpty(applicationPath) || !uri.AbsolutePath.StartsWith(applicationPath, StringComparison.OrdinalIgnoreCase))
                {
                    _appRelativePath = uri.AbsolutePath.Substring(applicationPath.Length);
                }
                else
                {
                    _appRelativePath = uri.AbsolutePath;
                }
            }

            public override string AppRelativeCurrentExecutionFilePath => string.Concat("~", _appRelativePath);

            public override string PathInfo { get; }
        }
    }
}