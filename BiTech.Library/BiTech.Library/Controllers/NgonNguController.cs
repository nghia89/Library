using BiTech.Library.BLL.DBLogic;
using BiTech.Library.DTO;
using BiTech.Library.Models;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.Mvc;

namespace BiTech.Library.Controllers
{
    public class NgonNguController : BaseController
    {
        public ActionResult Index()
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            LanguageLogic _LanguageLogic = new LanguageLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            List<Language> list = _LanguageLogic.GetAll();

            return View(list);
        }

        public ActionResult Create()
        {
            ViewBag.UnSuccess = TempData["UnSuccess"];

            return View(new LanguageModel());
        }

        [HttpPost]
        public ActionResult Create(LanguageModel model)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            if (ModelState.IsValid)
            {
                LanguageLogic _LanguageLogic = new LanguageLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
                string rs = _LanguageLogic.InsertNew(model.TheL);

                if (rs.Length == 24)
                    return RedirectToAction("Index");

                TempData["UnSuccess"] = "Thêm thất bại";
                return View(model);
            }
            return View(model);
        }
        public ActionResult Edit(string Id)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            LanguageLogic _LanguageLogic = new LanguageLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            var model = _LanguageLogic.GetById(Id);

            if (model == null)
                return RedirectToAction("Index");
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(Language model)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            if (ModelState.IsValid)
            {
                LanguageLogic _LanguageLogic = new LanguageLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
                var rs = _LanguageLogic.Update(model);
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

            LanguageLogic _LanguageLogic = new LanguageLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            _LanguageLogic.Remove(id);
            return RedirectToAction("Index");
        }
    }
}