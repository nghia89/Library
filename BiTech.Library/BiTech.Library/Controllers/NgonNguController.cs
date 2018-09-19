using BiTech.Library.BLL.DBLogic;
using BiTech.Library.Controllers.BaseClass;
using BiTech.Library.DTO;
using BiTech.Library.Models;
using PagedList;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BiTech.Library.Helpers;

namespace BiTech.Library.Controllers
{
    [AuthorizeRoles(true, Role.CustomerAdmin, Role.CustomerUser)]
    public class NgonNguController : BaseController
    {
        public ActionResult Index(int? page)
        {
            LanguageLogic _LanguageLogic = new LanguageLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);

            int PageSize = 10;
            int PageNumber = (page ?? 1);

            List<Language> list = _LanguageLogic.GetAll();
            ViewBag.pageSize = PageSize;
            ViewBag.pages = PageNumber;
            return View(list.OrderBy(m => m.Ten).ToPagedList(PageNumber, PageSize));
        }

        public ActionResult Create()
        {
            ViewBag.UnSuccess = TempData["UnSuccess"];

            return View(new LanguageModel());
        }

        [HttpPost]
        public ActionResult Create(LanguageModel model)
        {
            if (ModelState.IsValid)
            {
                LanguageLogic _LanguageLogic = new LanguageLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
                string rs = _LanguageLogic.InsertNew(model.TheL);

                if (rs.Length == 24)
                    return RedirectToAction("Index");

                TempData["UnSuccess"] = "Thêm thất bại";
                return View(model);
            }
            return View(model);
        }
        public JsonResult GetAll()
        {
            LanguageLogic _LanguageLogic = new LanguageLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            var list = _LanguageLogic.GetAll();
            return Json(list, JsonRequestBehavior.AllowGet);

        }

        public ActionResult Edit(string Id)
        {
            LanguageLogic _LanguageLogic = new LanguageLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            var model = _LanguageLogic.GetById(Id);

            if (model == null)
                return RedirectToAction("NotFound", "Error");
            return View(model);
			
        }

        [HttpPost]
        public ActionResult Edit(Language model)
        {
            if (ModelState.IsValid)
            {
                LanguageLogic _LanguageLogic = new LanguageLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
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
            LanguageLogic _LanguageLogic = new LanguageLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            _LanguageLogic.Remove(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult ThemAjax(LanguageViewModels model)
        {
            LanguageLogic _LanguageLogic = new LanguageLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);

            Language LG = new Language()
            {
                Ten=model.Ten,
                TenNgan=model.TenNgan
            };
            _LanguageLogic.InsertNew(LG);
            return Json(true);
        }
    }
}