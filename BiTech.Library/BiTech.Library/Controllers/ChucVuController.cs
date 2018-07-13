using BiTech.Library.BLL.DBLogic;
using BiTech.Library.DTO;
using BiTech.Library.Models;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.Mvc;

namespace BiTech.Library.Controllers
{
    public class ChucVuController : BaseController
    {
        public ActionResult Index()
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            ChucVuLogic _ChucVuLogic = new ChucVuLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            List<ChucVu> list = _ChucVuLogic.GetAllChucVu();

            return View(list);
        }

        public ActionResult Create()
        {
            ViewBag.UnSuccess = TempData["UnSuccess"];

            return View(new ChucVuModels());
        }

        [HttpPost]
        public ActionResult Create(ChucVuModels model)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            if (ModelState.IsValid)
            {
                ChucVuLogic _ChucVuLogic = new ChucVuLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
                ChucVu cv = new ChucVu()
                {
                    MaChucVu = model.MaChucVu,
                    TenChucVu = model.TenChucVu
                };
                string rs = _ChucVuLogic.ThemChucVu(cv);

                if (rs.Length == 24)
                    return RedirectToAction("Index");

                TempData["UnSuccess"] = "Thêm thất bại";
                return View(model);
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult ThemAjax(ChucVuModels model)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            ChucVuLogic _ChucVuLogicLogic = new ChucVuLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

            try
            {
                ChucVu cv = new ChucVu()
                {
                    MaChucVu = model.MaChucVu,
                    TenChucVu = model.TenChucVu,
                };
                _ChucVuLogicLogic.ThemChucVu(cv);
                return Json(true);
            }
            catch { return Json(false); }
        }

        public ActionResult Edit(string Id)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            ChucVuLogic _ChucVuLogic = new ChucVuLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            var model = _ChucVuLogic.getById(Id);
            if (model == null)
                return RedirectToAction("Index", "Error");
            ChucVuModels cv = new ChucVuModels()
            {
                Id = model.Id,
                TenChucVu = model.TenChucVu,
                MaChucVu = model.MaChucVu
            };
            if (model == null)
                return RedirectToAction("Index");
            return View(cv);
        }

        [HttpPost]
        public ActionResult Edit(ChucVuModels model)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            if (ModelState.IsValid)
            {
                ChucVuLogic _ChucVuLogic = new ChucVuLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
                ChucVu cv = _ChucVuLogic.getById(model.Id);
                if (cv == null)
                    return RedirectToAction("Index", "Error");
                cv.MaChucVu = model.MaChucVu;
                cv.TenChucVu = model.TenChucVu;
                
                var rs = _ChucVuLogic.SuaChucVu(cv);
                if (rs)
                    return RedirectToAction("Index");
                return View(model);
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(string id)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            ChucVuLogic _ChucVuLogic = new ChucVuLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            _ChucVuLogic.XoaChucVu(id);
            return RedirectToAction("Index");
        }



        #region AngularJS

        public JsonResult Get_AllChucVu() //JsonResult
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return Json(null, JsonRequestBehavior.AllowGet); //RedirectToAction("LogOff", "Account");
            #endregion
            ChucVuLogic _ChucVuLogic =
                new ChucVuLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

            var list = _ChucVuLogic.GetAllChucVu();
            return Json(list, JsonRequestBehavior.AllowGet);

        }

        public JsonResult Get_Name(string id) //JsonResult
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return Json(null, JsonRequestBehavior.AllowGet); //RedirectToAction("LogOff", "Account");
            #endregion
            ChucVuLogic _ChucVuLogic =
                new ChucVuLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

            var list = _ChucVuLogic.getById(id);
            return Json(list, JsonRequestBehavior.AllowGet);

        }
        #endregion

    }
}