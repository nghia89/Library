﻿using BiTech.Library.BLL.DBLogic;
using BiTech.Library.Controllers.BaseClass;
using BiTech.Library.DTO;
using BiTech.Library.Helpers;
using BiTech.Library.Models;
using Newtonsoft.Json;
using PagedList;
using PagedList.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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

            TacGiaLogic _TacGiaLogic = new TacGiaLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
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
            ViewBag.pageSize = PageSize;
            ViewBag.pages = PageNumber;
            return View(lsttg.OrderBy(m => m.TenTacGia).ToPagedList(PageNumber, PageSize));
        }

        //
        // GET: TacGia/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TacGia/Create
        //
        [HttpPost]
        public ActionResult Create(TacGia tacgia)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            TacGiaLogic _TacGiaLogic = new TacGiaLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            _TacGiaLogic.Insert(tacgia);

            return RedirectToAction("Index");
        }

        //
        // GET: TacGia/Edit
        public ActionResult Edit(string id)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion
            TacGiaLogic _TacGiaLogic = new TacGiaLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
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

        // POST: TacGia/Edit
        //
        [HttpPost]
        public ActionResult Edit(TacGia tacgia)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            TacGiaLogic _TacGiaLogic = new TacGiaLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            _TacGiaLogic.Update(tacgia);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult GetAll()
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return Json(null, JsonRequestBehavior.AllowGet); //RedirectToAction("LogOff", "Account");
            #endregion
            TacGiaLogic _TacGiaLogic = new TacGiaLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);

            var list = _TacGiaLogic.GetAllTacGia();
            return Json(list, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult GetAllTacGiaByIdSach(string idSach)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return Json(null, JsonRequestBehavior.AllowGet); //RedirectToAction("LogOff", "Account");
            #endregion
            TacGiaLogic _TacGiaLogic = new TacGiaLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            SachTacGiaLogic _SachTacGiaLogic = new SachTacGiaLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);

            List<SachTacGia> list_IDTacGia = _SachTacGiaLogic.getListById(idSach);
            List<string> List_TenTacGia = new List<string>();
            foreach (SachTacGia item in list_IDTacGia)
            {
                TacGia tg = _TacGiaLogic.GetById(item.IdTacGia);
                if (tg != null)
                    List_TenTacGia.Add(tg.TenTacGia);
            }
            
            var list = _TacGiaLogic.GetAllTacGia();
            return Json(List_TenTacGia, JsonRequestBehavior.AllowGet);

        }

        public JsonResult FindTacGia(string query)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return Json(null, JsonRequestBehavior.AllowGet); //RedirectToAction("LogOff", "Account");
            #endregion

            TacGiaLogic _TacGiaLogic = new TacGiaLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);

            var list = _TacGiaLogic.FindTacGia(query);

            List<TacGiaShortViewModel> tgs = new List<TacGiaShortViewModel>();
            foreach(var item in list)
            {
                tgs.Add(new TacGiaShortViewModel() { Id = item.Id, TenTacGia = item.TenTacGia });
            }
            
            return Json(JsonConvert.SerializeObject(tgs), JsonRequestBehavior.AllowGet);

        }
        
        [HttpPost]
        public ActionResult Delete(string Id)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            TacGiaLogic _TacGiaLogic = new TacGiaLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            var tacgia = _TacGiaLogic.GetById(Id);
            _TacGiaLogic.Delete(tacgia.Id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult ThemAjax(TacGiaViewModel model)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            TacGiaLogic _TacGiaLogic = new TacGiaLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);

            TacGia TG = new TacGia()
            {
                TenTacGia = model.TenTacGia,
                MoTa = model.MoTa,
                QuocTich = model.QuocTich
            };
            _TacGiaLogic.Insert(TG);
            return Json(true);
        }
    }
}