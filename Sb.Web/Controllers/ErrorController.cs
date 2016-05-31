using System.Web.Mvc;

namespace Sb.Web.Controllers
{
    public partial class ErrorController : Controller
    {
        // GET: Error
        public virtual ActionResult Error()
        {
            return View();
        }

        public virtual ActionResult NotFound()
        {
            return View();
        }

        public virtual ActionResult BadRequest()
        {
            return View();
        }
    }
}