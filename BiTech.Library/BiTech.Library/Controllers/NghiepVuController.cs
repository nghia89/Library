using BiTech.Library.Controllers.BaseClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BiTech.Library.Controllers
{
    public class NghiepVuController : BaseController
    {
        // GET: NghiepVu
        public ActionResult Index()
        {
            return View();
        }
    }
}