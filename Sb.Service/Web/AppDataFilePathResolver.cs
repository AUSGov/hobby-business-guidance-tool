using System;
using System.IO;
using System.Web;
using Sb.Interfaces.Services;

namespace Sb.Services.Web
{
    public class AppDataFilePathResolver : IFilePathResolver
    {
        public string GetWorkingDirectory()
        {
            if (HttpContext.Current.Request.PhysicalApplicationPath != null)
            {
                return Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "App_Data");
            }

            throw new InvalidOperationException("HttpContext not available");
        }
    }
}
