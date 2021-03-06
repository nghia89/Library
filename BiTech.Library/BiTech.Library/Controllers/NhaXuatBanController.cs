﻿using BiTech.Library.BLL.DBLogic;
using BiTech.Library.Controllers.BaseClass;
using BiTech.Library.DTO;
using BiTech.Library.Helpers;
using BiTech.Library.Models;
using PagedList;
using PagedList.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BiTech.Library.Controllers
{
    [AuthorizeRoles(true, Role.CustomerAdmin, Role.CustomerUser)]
#if DEBUG
    [AllowAnonymous]
#endif
    public class NhaXuatBanController : BaseController
    {
        // GET: NhaXuatBan
        public ActionResult Index(int? page)
        {
            NhaXuatBanLogic _NhaXuatBanLogic = new NhaXuatBanLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);

            var list = _NhaXuatBanLogic.GetAllNhaXuatBan();
            int PageSize = 10;
            int PageNumber = (page ?? 1);
            List<NhaXuatBanViewModels> lst = new List<NhaXuatBanViewModels>();
            foreach (var item in list)
            {
                NhaXuatBanViewModels nxb = new NhaXuatBanViewModels()
                {
                    Id = item.Id,
                    Ten = item.Ten,
                    GhiChu = item.GhiChu
                };
                lst.Add(nxb);
            }
            ViewBag.pageSize = PageSize;
            ViewBag.pages = PageNumber;
            return View(lst.OrderBy(m => m.Ten).ToPagedList(PageNumber, PageSize));
        }

        public ActionResult Them()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Them(NhaXuatBanViewModels model)
        {
            NhaXuatBanLogic _NhaXuatBanLogic = new NhaXuatBanLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);

            NhaXuatBan nxb = new NhaXuatBan()
            {
                Ten = model.Ten,
                GhiChu = model.GhiChu
            };
            _NhaXuatBanLogic.ThemNXB(nxb);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult ThemAjax(NhaXuatBanViewModels model)
        {
            NhaXuatBanLogic _NhaXuatBanLogic = new NhaXuatBanLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);

            NhaXuatBan nxb = new NhaXuatBan()
            {
                Ten = model.Ten,
                GhiChu = model.GhiChu
            };
            _NhaXuatBanLogic.ThemNXB(nxb);
            return Json(true);
        }

        public ActionResult Sua(string id)
        {
            NhaXuatBanLogic _NhaXuatBanLogic = new NhaXuatBanLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);

            NhaXuatBan nxb = _NhaXuatBanLogic.getById(id);
            if (nxb == null)
                return RedirectToAction("NotFound", "Error");
            NhaXuatBanViewModels VM = new NhaXuatBanViewModels()
            {
                Id = nxb.Id,
                Ten = nxb.Ten,
                GhiChu = nxb.GhiChu
            };
            return View(VM);
        }

        [HttpPost]
        public ActionResult Sua(NhaXuatBanViewModels model)
        {
            NhaXuatBanLogic _NhaXuatBanLogic = new NhaXuatBanLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);

            NhaXuatBan nxb = _NhaXuatBanLogic.getById(model.Id);
            nxb.Ten = model.Ten;
            nxb.GhiChu = model.GhiChu;
            _NhaXuatBanLogic.SuaNXB(nxb);
            return RedirectToAction("Index");
        }


        [HttpPost]
        public ActionResult Xoa(string Id)
        {
            NhaXuatBanLogic _NhaXuatBanLogic = new NhaXuatBanLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            var nxb = _NhaXuatBanLogic.getById(Id);
            _NhaXuatBanLogic.XoaNXB(nxb.Id);
            return RedirectToAction("Index");
        }
        #region AngularJS

        public JsonResult Get_AllNhaXuatBan() //JsonResult
        {
            NhaXuatBanLogic _NhaXuatBanLogic =
                new NhaXuatBanLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);

            var list = _NhaXuatBanLogic.GetAllNhaXuatBan();
            return Json(list, JsonRequestBehavior.AllowGet);

        }
        #endregion
    }
}