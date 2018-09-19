using BiTech.Library.Models;
using System.Web.Mvc;

namespace BiTech.Library.Controllers
{
    [AuthorizeRoles(true, Role.CustomerAdmin, Role.CustomerUser)]
    public class HelpController : Controller
    {
        // GET: Controllers/Help
        public ActionResult Index()
        {
            return View();
        }
    }
}