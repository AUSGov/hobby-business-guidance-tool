using System.Web.Mvc;
using System.Web.Routing;

namespace Sb.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("robots", "robots.txt",
                new { controller = "Seo", action = "Robots" });

            routes.MapRoute("Questions", "Questions/{id}",
                 new { controller = MVC.Questions.Name, action = MVC.Questions.ActionNames.Index, id = UrlParameter.Optional });

            routes.MapRoute("Default", "{controller}/{action}/{id}",
                new { controller = MVC.Home.Name, action = MVC.Home.ActionNames.Index, id = UrlParameter.Optional }
            );
        }
    }
}
