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
    public class KeSachController : BaseController
    {
  
        // GET: KeSach
        public ActionResult Index()
        {

            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            KeSachLogic _keSachLogic = new KeSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

            var list = _keSachLogic.GetAll();
            ViewBag.ListKeSach = list;
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(KesachViewModels model)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            KeSachLogic _keSachLogic = new KeSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

            KeSach keSach = new KeSach()
            {
                TenKe = model.TenKe,
                ViTri = model.ViTri,
                GhiChu = model.GhiChu
            };
            _keSachLogic.Add(keSach);
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

            KeSachLogic _keSachLogic = new KeSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            //var dl = _keSachLogic.GetById(id);
            _keSachLogic.Delete(id);
            return RedirectToAction("Index");
        }
        public ActionResult Edit(string Id)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            KeSachLogic _keSachLogic = new KeSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            var list = _keSachLogic.getById(Id);
            KesachViewModels ks = new KesachViewModels();
            {
                ks.TenKe = list.TenKe;
                ks.ViTri = list.ViTri;
                ks.GhiChu = list.ViTri;
               
            }
            return View(ks);
        }
        [HttpPost]
        public ActionResult Edit(KesachViewModels model)
        {

            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            KeSachLogic _keSachLogic = new KeSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            KeSach keSach = new KeSach()
            {
                TenKe = model.TenKe,
                ViTri = model.ViTri,
                GhiChu = model.GhiChu
            };
            _keSachLogic.Add(keSach);
            return RedirectToAction("Index");
        }
    }
}