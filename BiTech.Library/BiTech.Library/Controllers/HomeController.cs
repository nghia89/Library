using BiTech.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BiTech.Library.Controllers
{
    [AuthorizeRoles(Role.CustomerUser, Role.CustomerAdmin)]
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            //userdata.MyApps[AppCode].ConnectionString
            //userdata.MyApps[AppCode].DatabaseName

            //_HRM_Logic = new HRM_Logic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

            // do things here
            return View();
        }

        public ActionResult About()
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            //userdata.MyApps[AppCode].ConnectionString
            //userdata.MyApps[AppCode].DatabaseName

            //_HRM_Logic = new HRM_Logic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

            // do things here
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            //userdata.MyApps[AppCode].ConnectionString
            //userdata.MyApps[AppCode].DatabaseName

            //_HRM_Logic = new HRM_Logic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

            // do things here

            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}