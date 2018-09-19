using BiTech.Library.Models;
using System.Web.Mvc;

namespace BiTech.Library.Controllers
{
    [AuthorizeRoles(true, Role.CustomerAdmin, Role.CustomerUser)]
#if DEBUG
    [AllowAnonymous]
#endif
    public class HelpController : Controller
    {
        // GET: Controllers/Help
        public ActionResult Index()
        {
            return View();
        }
    }
}