using BiTech.Library.BLL.DBLogic;
using BiTech.Library.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BiTech.Library.Controllers
{
    public class TheLoaiSachController : Controller
    {
        private TheLoaiSachLogic _theLoaiSachLogic;
        public TheLoaiSachController()
        {
            _theLoaiSachLogic = new TheLoaiSachLogic(Tool.GetConfiguration("ConnectionString"), Tool.GetConfiguration("DatabaseName"));
        }

        // GET: TheLoaiSach
        public ActionResult Index()
        {
            return View();
        }
    }
}