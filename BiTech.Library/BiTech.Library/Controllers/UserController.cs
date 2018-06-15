using BiTech.Library.BLL.DBLogic;
using BiTech.Library.DTO;
using BiTech.Library.Helpers;
using BiTech.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BiTech.Library.Controllers
{
    public class UserController : BaseController
    {
        private ThanhVienLogic _ThanhVienLogic;
        public UserController()
        {
            _ThanhVienLogic = new ThanhVienLogic(Tool.GetConfiguration("ConnectionString"), Tool.GetConfiguration("DatabaseName"));
        }

        // GET: User
        public ActionResult Index()
        {
            return View();
        }
        public PartialViewResult _PartialUser()
        {
            List<ThanhVien> lstUser = _ThanhVienLogic.GetAll();
            ViewBag.lstUser = lstUser; //Danh sach thanh vien ...
            return PartialView();
        }

    }
}