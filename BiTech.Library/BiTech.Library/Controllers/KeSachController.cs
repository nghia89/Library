using BiTech.Library.BLL.DBLogic;
using BiTech.Library.DTO;
using BiTech.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;

namespace BiTech.Library.Controllers
{
    public class KeSachController : BaseController
    {
        // GET: KeSach
        public ActionResult Index(int? page)
        {

            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            KeSachLogic _keSachLogic = new KeSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            int PageSize = 10;
            int PageNumber = (page ?? 1);
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

            return View(lst.OrderBy(x => x.TenKe).ToPagedList(PageNumber, PageSize));
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
        public ActionResult Delete(string Id)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            KeSachLogic _keSachLogic = new KeSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            var dl = _keSachLogic.getById(Id);
            _keSachLogic.Delete(dl.Id);
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

            KesachViewModels ks = new KesachViewModels()
            {
                Id = list.Id,
                TenKe = list.TenKe,
                ViTri = list.ViTri,
                GhiChu = list.GhiChu,
            };

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
                Id = model.Id,
                TenKe = model.TenKe,
                ViTri = model.ViTri,
                GhiChu = model.GhiChu
            };
            _keSachLogic.Update(keSach);
            return RedirectToAction("Index");
        }
    }
}