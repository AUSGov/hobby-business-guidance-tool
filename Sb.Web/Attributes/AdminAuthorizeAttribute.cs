using System;
using System.Configuration;
using System.Web;
using System.Web.Mvc;
using Sb.Entities;

namespace Sb.Web.Attributes
{
    public class AdminAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return ConfigurationManager.AppSettings[Constants.AppSettings.AdminPagesEnabled].Equals(true.ToString(), StringComparison.CurrentCultureIgnoreCase);
        }
    }
}