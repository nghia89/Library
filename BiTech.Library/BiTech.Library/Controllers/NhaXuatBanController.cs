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
            return View();
        }

        [HttpPost]
        public ActionResult ThemNXB(NhaXuatBanViewModels model)
        {
            NhaXuatBan nxb = new NhaXuatBan()
            {
                Ten = model.Ten,
                GhiChu = model.GhiChu
            };
            _NhaXuatBanLogic.ThemNXB(nxb);
            return RedirectToAction("DanhSachNXB");
        }

        public ActionResult SuaNXB(string id)
        {
            NhaXuatBan nxb = _NhaXuatBanLogic.getById(id);
            NhaXuatBanViewModels VM = new NhaXuatBanViewModels()
            {
                Id = nxb.Id,
                Ten = nxb.Ten,
                GhiChu = nxb.GhiChu
            };
            return View(VM);
        }

        [HttpPost]
        public ActionResult SuaNXB(NhaXuatBanViewModels model)
        {
            NhaXuatBan nxb = _NhaXuatBanLogic.getById(model.Id);
            nxb.Ten = model.Ten;
            nxb.GhiChu = model.GhiChu;
            _NhaXuatBanLogic.SuaNXB(nxb);
            return RedirectToAction("DanhSachNXB");
        }

        public ActionResult XoaNXB(string id)
        {
            NhaXuatBan nxb = _NhaXuatBanLogic.getById(id);
            NhaXuatBanViewModels VM = new NhaXuatBanViewModels()
            {
                Id = nxb.Id,
                Ten = nxb.Ten,
                GhiChu = nxb.GhiChu
            };
            return View(VM);
        }

        [HttpPost]
        public ActionResult XoaNXB(NhaXuatBanViewModels model)
        {
            NhaXuatBan nxb = _NhaXuatBanLogic.getById(model.Id);
            _NhaXuatBanLogic.XoaNXB(nxb.Id);
            return RedirectToAction("DanhSachNXB");
        }
    }
}