using BiTech.Library.BLL.DBLogic;
using BiTech.Library.DTO;
using BiTech.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BiTech.Library.Controllers
{
    public class TrangThaiController : Controller
    {
        TrangThaiSachLogic _TrangThaiSachLogic = new TrangThaiSachLogic("mongodb://localhost:27017/BiTechLibraryDB", "BiTechLibraryDB");
        // GET: TrangThai
        public ActionResult DanhSachTrangThai()
        {
            var list = _TrangThaiSachLogic.GetAll();
            ViewBag.ListTT = list;
            return View();
        }

        public ActionResult ThemTT()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ThemTT(TrangThaiSachViewModels model)
        {
            TrangThaiSach tts = new TrangThaiSach()
            {
                TenTT = model.TenTT
            };
            _TrangThaiSachLogic.ThemTrangThai(tts);
            return RedirectToAction("DanhSachTrangThai");
        }

        public ActionResult SuaTT(string id)
        {
            TrangThaiSach tts = _TrangThaiSachLogic.getById(id);
            TrangThaiSachViewModels VM = new TrangThaiSachViewModels()
            {
                Id = tts.Id,
                TenTT = tts.TenTT
            };
            return View(VM);
        }

        [HttpPost]
        public ActionResult SuaTT(TrangThaiSachViewModels model)
        {
            TrangThaiSach tts = _TrangThaiSachLogic.getById(model.Id);
            tts.TenTT = model.TenTT;
            _TrangThaiSachLogic.SuaTrangThai(tts);
            return RedirectToAction("DanhSachTrangThai");
        }

        public ActionResult XoaTT(string id)
        {
            TrangThaiSach tts = _TrangThaiSachLogic.getById(id);
            TrangThaiSachViewModels VM = new TrangThaiSachViewModels()
            {
                Id = tts.Id,
                TenTT = tts.TenTT
            };
            return View(VM);
        }

        [HttpPost]
        public ActionResult XoaTT(TrangThaiSachViewModels model)
        {
            TrangThaiSach tts = _TrangThaiSachLogic.getById(model.Id);
            _TrangThaiSachLogic.XoaTrangThai(tts.Id);
            return RedirectToAction("DanhSachTrangThai");
        }
    }
}