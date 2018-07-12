using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BiTech.Library.BLL.DBLogic;
using BiTech.Library.DTO;
using BiTech.Library.Helpers;
using BiTech.Library.Models;
using PagedList;
using PagedList.Mvc;

namespace BiTech.Library.Controllers
{
    public class TacGiaController : BaseController
    {
        // GET: TacGia
        public ActionResult Index(int? page)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            TacGiaLogic _TacGiaLogic = new TacGiaLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            int PageSize = 10;
            int PageNumber = (page ?? 1);
            var lst = _TacGiaLogic.GetAllTacGia();
            List<TacGiaViewModel> lsttg = new List<TacGiaViewModel>();
            foreach (var item in lst)
            {
                TacGiaViewModel tg = new TacGiaViewModel()
                {
                    Id = item.Id,
                    TenTacGia = item.TenTacGia,
                    MoTa = item.MoTa,
                    QuocTich = item.QuocTich
                };
                lsttg.Add(tg);
            }

            return View(lsttg.OrderBy(m => m.TenTacGia).ToPagedList(PageNumber, PageSize));
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(TacGia tacgia)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            TacGiaLogic _TacGiaLogic = new TacGiaLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            _TacGiaLogic.Insert(tacgia);

            return RedirectToAction("Index");
        }
        public ActionResult Edit(string id)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion
            TacGiaLogic _TacGiaLogic = new TacGiaLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            var tacgia = _TacGiaLogic.GetById(id);
            if (tacgia == null)
                return RedirectToAction("NotFound", "Error");
            TacGiaViewModel tg = new TacGiaViewModel()
            {
                TenTacGia = tacgia.TenTacGia,
                QuocTich = tacgia.QuocTich,
                MoTa = tacgia.MoTa,
                Id = tacgia.Id
            };
            return View(tg);
        }
        [HttpPost]
        public ActionResult Edit(TacGia tacgia)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            TacGiaLogic _TacGiaLogic = new TacGiaLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            _TacGiaLogic.Update(tacgia);

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

            TacGiaLogic _TacGiaLogic = new TacGiaLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            var tacgia = _TacGiaLogic.GetById(Id);
            _TacGiaLogic.Delete(tacgia.Id);
            return RedirectToAction("Index");
        }
    }
}