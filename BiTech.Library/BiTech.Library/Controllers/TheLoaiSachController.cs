using BiTech.Library.BLL.DBLogic;
using BiTech.Library.Models;
using BiTech.Library.DTO;
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
        public ActionResult DanhSachTheLoaiSach()
        {
            var ul = new TheLoaiSachLogic("mongodb://localhost:27017/BiTechLibraryDB", "BiTechLibraryDB");
            var list = ul.GetAllTheLoaiSach();
            ViewBag.ListTheLoai = list;
            return View();
        }

        public ActionResult ThemTheLoaiSach()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult ThemTheLoaiSach(TheLoaiSach model)
        {
            TheLoaiSach TLS = new TheLoaiSach()
            {
                TenTheLoai = model.TenTheLoai,
                MoTa = model.MoTa
            };
            _theLoaiSachLogic.ThemTheLoaiSach(TLS);
            return RedirectToAction("DanhSachTheLoaiSach");
        }

        public ActionResult SuaTheLoaiSach(string id)
        {
            TheLoaiSach TLS = _theLoaiSachLogic.getById(id);
            TheLoaiSachViewModels VM = new TheLoaiSachViewModels()
            {
                Id = TLS.Id,
                TenTheLoai = TLS.TenTheLoai,
                MoTa = TLS.MoTa,
            };
            return View(VM);
        }

        [HttpPost]
        public ActionResult SuaTheLoaiSach(TheLoaiSachViewModels model)
        {
            TheLoaiSach TLS = _theLoaiSachLogic.getById(model.Id);
            TLS.TenTheLoai = model.TenTheLoai;
            TLS.MoTa = model.MoTa;
            _theLoaiSachLogic.SuaTheLoaiSach(TLS);
            return RedirectToAction("DanhSachTheLoaiSach");
        }

        public ActionResult XoaTheLoaiSach(string id)
        {
            TheLoaiSach TLS = _theLoaiSachLogic.getById(id);
            TheLoaiSachViewModels VM = new TheLoaiSachViewModels()
            {
                Id = TLS.Id,
                TenTheLoai = TLS.TenTheLoai,
                MoTa = TLS.MoTa,
            };
            return View(VM);
        }

        [HttpPost]
        public ActionResult XoaTheLoaiSach(TheLoaiSachViewModels model)
        {
            TheLoaiSach TLS = _theLoaiSachLogic.getById(model.Id);
            _theLoaiSachLogic.XoaTheLoaiSach(TLS.Id);
            return RedirectToAction("DanhSachTheLoaiSach");
        }
    }
}