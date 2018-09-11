using BiTech.Library.Controllers.BaseClass;
using BiTech.Library.Models;
using System.Web.Mvc;

namespace BiTech.Library.Controllers
{
    [AllowAnonymous]
    public class ErrorController : Controller
    {
        // Error
        public ActionResult AccessDenied()
        {
            return View();
        }
        
        public ActionResult NotFound()
        {
            return View();
        }

        public ActionResult ErrorOccur()
        {
            return View();
        }
    }
}