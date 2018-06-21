using System.Configuration;
using System.Web;
using System.Web.Mvc;

namespace BiTech.Library.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return Redirect(string.Format("{0}{1}", ConfigurationManager.AppSettings["LoginUrl"], HttpRuntime.AppDomainAppVirtualPath));
            //return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            return Redirect(string.Format("{0}{1}", ConfigurationManager.AppSettings["LogoffUrl"], HttpRuntime.AppDomainAppVirtualPath));
        }
    }
}