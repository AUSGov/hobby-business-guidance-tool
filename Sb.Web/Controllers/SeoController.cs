using System;
using System.Web.Mvc;
using Sb.Entities;
using Sb.Interfaces;

namespace Sb.Web.Controllers
{
    public partial class SeoController : Controller
    {
        private readonly ISettingsReader _settingsReader;

        public SeoController(ISettingsReader settingsReader)
        {
            if (settingsReader == null)
            {
                throw new ArgumentNullException(nameof(settingsReader));
            }

            _settingsReader = settingsReader;
        }

        public virtual ActionResult Robots()
        {
            return File(_settingsReader[Constants.AppSettings.RobotsFile], "text/plain");
        }
    }
}