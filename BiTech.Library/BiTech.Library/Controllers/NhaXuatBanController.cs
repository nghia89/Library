using BiTech.Library.BLL.DBLogic;
using BiTech.Library.DTO;
using BiTech.Library.Helpers;
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
    public class NhaXuatBanController : BaseController
    {    
        // GET: NhaXuatBan
        public ActionResult Index(int? page)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            NhaXuatBanLogic _NhaXuatBanLogic = new NhaXuatBanLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

            var list = _NhaXuatBanLogic.GetAllNhaXuatBan();
            int PageSize = 10;
            int PageNumber = (page ?? 1);
            List<NhaXuatBanViewModels> lst = new List<NhaXuatBanViewModels>();
            foreach(var item in list)
            {
                NhaXuatBanViewModels nxb = new NhaXuatBanViewModels()
                {
                    Id = item.Id,
                    Ten = item.Ten,
                    GhiChu = item.GhiChu
                };
                lst.Add(nxb);
            }

            return View(lst.OrderBy(m => m.Ten).ToPagedList(PageNumber, PageSize));
        }

        public ActionResult Them()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Them(NhaXuatBanViewModels model)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            NhaXuatBanLogic _NhaXuatBanLogic = new NhaXuatBanLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

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
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            NhaXuatBanLogic _NhaXuatBanLogic = new NhaXuatBanLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

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
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            NhaXuatBanLogic _NhaXuatBanLogic = new NhaXuatBanLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

            NhaXuatBan nxb = _NhaXuatBanLogic.getById(id);
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
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            NhaXuatBanLogic _NhaXuatBanLogic = new NhaXuatBanLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

            NhaXuatBan nxb = _NhaXuatBanLogic.getById(model.Id);
            nxb.Ten = model.Ten;
            nxb.GhiChu = model.GhiChu;
            _NhaXuatBanLogic.SuaNXB(nxb);
            return RedirectToAction("Index");
        }

       
        [HttpPost]
        public ActionResult Xoa(string Id)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            NhaXuatBanLogic _NhaXuatBanLogic = new NhaXuatBanLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            var nxb = _NhaXuatBanLogic.getById(Id);
            _NhaXuatBanLogic.XoaNXB(nxb.Id);
            return RedirectToAction("Index");
        }
        #region AngularJS

        public JsonResult Get_AllNhaXuatBan() //JsonResult
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return Json(null, JsonRequestBehavior.AllowGet); //RedirectToAction("LogOff", "Account");
            #endregion
            NhaXuatBanLogic _NhaXuatBanLogic =
                new NhaXuatBanLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

            var list = _NhaXuatBanLogic.GetAllNhaXuatBan();
            return Json(list, JsonRequestBehavior.AllowGet);

        }
        #endregion
    }
}