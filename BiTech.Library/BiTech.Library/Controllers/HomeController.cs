using BiTech.Library.Controllers.BaseClass;
using BiTech.Library.Models;
using System.Web.Mvc;

namespace BiTech.Library.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult GioiThieu()
        {
            ViewBag.data = @"../upload/thuvien/thuviensohcm/gioithieu.html";
            return View();
        }

        public ActionResult ChucNangNhiemVu()
        {
            ViewBag.data = @"../upload/thuvien/thuviensohcm/gioithieu.html";
            return View();
        }

        public ActionResult CoCauToChuc()
        {
            ViewBag.data = @"../upload/thuvien/thuviensohcm/gioithieu.html";
            return View();
        }
    }
}