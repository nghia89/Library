using BiTech.Library.BLL.DBLogic;
using BiTech.Library.Controllers.BaseClass;
using BiTech.Library.DTO;
using BiTech.Library.Models;
using PagedList;
using PagedList.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BiTech.Library.Helpers;

namespace BiTech.Library.Controllers
{
    [AuthorizeRoles(true, Role.CustomerAdmin, Role.CustomerUser)]
#if DEBUG
    [AllowAnonymous]
#endif
    public class KeSachController : BaseController
    {

        // GET: KeSach
        public ActionResult Index(int? page)
        {
            KeSachLogic _keSachLogic = new KeSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            var list = _keSachLogic.GetAll();
            List<KesachViewModels> lst = new List<KesachViewModels>();
            foreach (var item in list)
            {
                KesachViewModels ks = new KesachViewModels()
                {
                    Id = item.Id,
                    TenKe = item.TenKe,
                    CreateDateTime = item.CreateDateTime,
                    GhiChu = item.GhiChu,
                    ViTri = item.ViTri
                };
                lst.Add(ks);
            }
            int PageSize = 10;
            int PageNumber = (page ?? 1);
            ViewBag.pageSize = PageSize;
            ViewBag.pages = PageNumber;
            
            return View(lst.OrderBy(x => x.TenKe).ToPagedList(PageNumber, PageSize));
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(KesachViewModels model)
        {
            KeSachLogic _keSachLogic = new KeSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);

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
        public ActionResult Delete(string Id)
        {
            try
            {
                KeSachLogic _keSachLogic = new KeSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
                var dl = _keSachLogic.getById(Id);
                _keSachLogic.Delete(dl.Id);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return RedirectToAction("NotFound", "Error");
            }
        }

        public ActionResult Edit(string Id)
        {
            KeSachLogic _keSachLogic = new KeSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            var kesach = _keSachLogic.getById(Id);
            if (kesach == null)
                return RedirectToAction("NotFound", "Error");
            KesachViewModels ks = new KesachViewModels()
            {
                Id = kesach.Id,
                TenKe = kesach.TenKe,
                ViTri = kesach.ViTri,
                GhiChu = kesach.GhiChu,
            };

            return View(ks);
        }

        [HttpPost]
        public ActionResult Edit(KesachViewModels model)
        {
            KeSachLogic _keSachLogic = new KeSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            KeSach keSach = new KeSach()
            {
                Id = model.Id,
                TenKe = model.TenKe,
                ViTri = model.ViTri,
                GhiChu = model.GhiChu
            };
            _keSachLogic.Update(keSach);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult ThemAjax(KesachViewModels model)
        {
            KeSachLogic _keSachLogic = new KeSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);

            KeSach keSach = new KeSach()
            {
                TenKe = model.TenKe,
                ViTri = model.ViTri,
                GhiChu = model.GhiChu
            };
            _keSachLogic.Add(keSach);
            return Json(true);
        }

        public JsonResult GetAll() 
        {
            KeSachLogic _keSachLogic =new KeSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);

            var list = _keSachLogic.GetAll();
            return Json(list, JsonRequestBehavior.AllowGet);

        }
    }
}