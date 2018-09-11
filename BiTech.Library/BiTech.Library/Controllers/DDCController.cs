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
using BiTech.Library.Controllers.BaseClass;

namespace BiTech.Library.Controllers
{
    public class DDCController : BaseController
    {
        public ActionResult Index(int? page)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            DDCLogic _DDCLogic = new DDCLogic(userdata.MyApps[_AppCode].ConnectionString, userdata.MyApps[_AppCode].DatabaseName);
            var list = _DDCLogic.GetAllDDC();
            List<DDC> lst = new List<DDC>();
            foreach (var item in list)
            {
                DDC ks = new DDC()
                {
                    Id = item.Id,
                    MaDDC = item.MaDDC,
                    Ten = item.Ten,
                    CreateDateTime = item.CreateDateTime
                };
                lst.Add(ks);
            }
            int PageSize = 10;
            int PageNumber = (page ?? 1);
            ViewBag.pageSize = PageSize;
            ViewBag.pages = PageNumber;

            return View(lst.OrderBy(x => x.Ten).ToPagedList(PageNumber, PageSize));
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(DDC model)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            DDCLogic _DDCLogic = new DDCLogic(userdata.MyApps[_AppCode].ConnectionString, userdata.MyApps[_AppCode].DatabaseName);

            DDC ddc = new DDC()
            {
                MaDDC = model.MaDDC,
                Ten = model.Ten
            };
            _DDCLogic.Add(ddc);
            return RedirectToAction("Index");
        }

        public ActionResult Edit(string Id)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion
            DDCLogic _DDCLogic = new DDCLogic(userdata.MyApps[_AppCode].ConnectionString, userdata.MyApps[_AppCode].DatabaseName);
            var item = _DDCLogic.getById(Id);
            if (item == null)
                return RedirectToAction("NotFound", "Error");
            DDC ddc = new DDC()
            {
                Id = item.Id,
                Ten = item.Ten,
                MaDDC = item.MaDDC,
            };

            return View(ddc);
        }

        [HttpPost]
        public ActionResult Edit(DDC model)
        {

            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            DDCLogic _DDCLogic = new DDCLogic(userdata.MyApps[_AppCode].ConnectionString, userdata.MyApps[_AppCode].DatabaseName);
            DDC ddc = new DDC()
            {
                Id = model.Id,
                Ten = model.Ten,
                MaDDC = model.MaDDC,
            };
            _DDCLogic.Update(ddc);
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
            try
            {
                DDCLogic _DDCLogic = new DDCLogic(userdata.MyApps[_AppCode].ConnectionString, userdata.MyApps[_AppCode].DatabaseName);
                var dl = _DDCLogic.getById(Id);
                _DDCLogic.Delete(dl.Id);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return RedirectToAction("NotFound", "Error");
            }
        }

    }
}