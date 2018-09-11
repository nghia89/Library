using BiTech.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BiTech.Library.Controllers
{
    //[AuthorizeRoles(Role.CustomerUser, Role.CustomerAdmin)]
    public class HelpController : Controller
    {
        // GET: Controllers/Help
        public ActionResult Index()
        {
            return View();
        }
    }
}