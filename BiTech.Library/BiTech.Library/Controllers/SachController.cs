using BiTech.Library.BLL.DBLogic;
using BiTech.Library.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BiTech.Library.Controllers
{
    public class SachController : Controller
    {
        private SachLogic _SachLogic;
        private TheLoaiSachLogic _theLoaiSachLogic;
        public SachController()
        {
            _SachLogic = new SachLogic(Tool.GetConfiguration("ConnectionString"),Tool.GetConfiguration("DatabaseName"));
            _theLoaiSachLogic = new TheLoaiSachLogic(Tool.GetConfiguration("ConnectionString"), Tool.GetConfiguration("DatabaseName"));
        }
        // GET: Sach
        public ActionResult Index()
        {            
            return View();
        }

        //[ChildActionOnly]
        //public PartialViewResult TheLoaiSach()
        //{
        //    var list = _theLoaiSachLogic.GetAllTheLoaiSach();
        //    ViewBag.ListTheLoaiSach = list;
        //    return PartialView();
        //}
      
    }
}