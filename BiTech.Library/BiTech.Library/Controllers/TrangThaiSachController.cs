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
    public class TrangThaiSachController : BaseController
    {
        // GET: TrangThai
        public ActionResult Index()
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            TrangThaiSachLogic _TrangThaiSachLogic = new TrangThaiSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

            var list = _TrangThaiSachLogic.GetAll();
            ViewBag.ListTT = list;
            return View();
        }

        public ActionResult Them()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Them(TrangThaiSachViewModels model)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            TrangThaiSachLogic _TrangThaiSachLogic = new TrangThaiSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

            TrangThaiSach tts = new TrangThaiSach()
            {
                TenTT = model.TenTT
            };
            _TrangThaiSachLogic.ThemTrangThai(tts);
            return RedirectToAction("Index");
        }

        public ActionResult Sua(string id)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            TrangThaiSachLogic _TrangThaiSachLogic = new TrangThaiSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

            TrangThaiSach tts = _TrangThaiSachLogic.getById(id);
            TrangThaiSachViewModels VM = new TrangThaiSachViewModels()
            {
                Id = tts.Id,
                TenTT = tts.TenTT
            };
            return View(VM);
        }

        [HttpPost]
        public ActionResult Sua(TrangThaiSachViewModels model)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            TrangThaiSachLogic _TrangThaiSachLogic = new TrangThaiSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

            TrangThaiSach tts = _TrangThaiSachLogic.getById(model.Id);
            tts.TenTT = model.TenTT;
            _TrangThaiSachLogic.SuaTrangThai(tts);
            return RedirectToAction("Index");
        }

        public ActionResult Xoa(string id)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            TrangThaiSachLogic _TrangThaiSachLogic = new TrangThaiSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

            TrangThaiSach tts = _TrangThaiSachLogic.getById(id);
            TrangThaiSachViewModels VM = new TrangThaiSachViewModels()
            {
                Id = tts.Id,
                TenTT = tts.TenTT
            };
            return View(VM);
        }

        [HttpPost]
        public ActionResult Xoa(TrangThaiSachViewModels model)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            TrangThaiSachLogic _TrangThaiSachLogic = new TrangThaiSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

            TrangThaiSach tts = _TrangThaiSachLogic.getById(model.Id);
            _TrangThaiSachLogic.XoaTrangThai(tts.Id);
            return RedirectToAction("Index");
        }
    }
}