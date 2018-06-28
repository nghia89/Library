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
    public class LyDoXuatController : BaseController
    {
        // GET: LyDoXuat
        public ActionResult Index()
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            LyDoXuatLogic _LyDoLogic = new LyDoXuatLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            var lst = _LyDoLogic.GetAll();
            ViewBag.ListLyDo = lst;

            return View();
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(LyDoXuat ldx)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            LyDoXuatLogic _LyDoXuatLogic = new LyDoXuatLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            _LyDoXuatLogic.Insert(ldx);

            return RedirectToAction("Index");
        }
        public ActionResult Edit(string id)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            LyDoXuatLogic _LyDoXuatLogic = new LyDoXuatLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            var lydo = _LyDoXuatLogic.GetById(id);
            LyDoXuatViewModel ld = new LyDoXuatViewModel()
            {
                Id = lydo.Id,
                LyDo=lydo.LyDo
            };
            return View(ld);
        }
        [HttpPost]
        public ActionResult Edit(LyDoXuat ldx)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            LyDoXuatLogic _LyDoXuatLogic = new LyDoXuatLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            _LyDoXuatLogic.Update(ldx);

            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult Delete(string id)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            LyDoXuatLogic _LyDoXuatLogic = new LyDoXuatLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            var lydo = _LyDoXuatLogic.GetById(id);
            _LyDoXuatLogic.Delete(lydo.Id);
            return RedirectToAction("Index");
        }
    }
}