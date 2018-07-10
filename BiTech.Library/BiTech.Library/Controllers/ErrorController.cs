using BiTech.Library.Models;
using System.Web.Mvc;

namespace BiTech.Library.Controllers
{
    [AuthorizeRoles(Role.CustomerUser, Role.CustomerAdmin)]
    public class ErrorController : BaseController
    {
        [AllowAnonymous]
        public ActionResult AccessDenied()
        {
            return View();
        }


        [AllowAnonymous]
        public ActionResult NotFound()
        {
            return View();
        }
        
    }
}