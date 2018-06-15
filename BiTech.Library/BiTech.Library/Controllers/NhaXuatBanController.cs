using BiTech.Library.BLL.DBLogic;
using BiTech.Library.DTO;
using BiTech.Library.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BiTech.Library.Controllers
{
    public class NhaXuatBanController : Controller
    {    
        NhaXuatBanLogic _NhaXuatBanLogic = new NhaXuatBanLogic("mongodb://localhost:27017/BiTechLibraryDB", "BiTechLibraryDB");
        // GET: NhaXuatBan
        public ActionResult DanhSachNXB()
        {
            var ul = new NhaXuatBanLogic("mongodb://localhost:27017/BiTechLibraryDB", "BiTechLibraryDB");
            var list = ul.getAllNhaXuatBan();
            ViewBag.ListNhaXuaBan = list;
            return View();
        }

        public ActionResult ThemNXB()
        {
             
        }
    }
}