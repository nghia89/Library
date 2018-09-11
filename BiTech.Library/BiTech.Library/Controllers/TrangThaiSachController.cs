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

namespace BiTech.Library.Controllers
{
    public class TrangThaiSachController : BaseController
    {
        // GET: TrangThai
        public ActionResult Index(int? page)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            TrangThaiSachLogic _TrangThaiSachLogic = new TrangThaiSachLogic(userdata.MyApps[_AppCode].ConnectionString, userdata.MyApps[_AppCode].DatabaseName);


            
            var lst = _TrangThaiSachLogic.GetAll();
            List<TrangThaiSachViewModels> lsttt = new List<TrangThaiSachViewModels>();
            foreach (var item in lst)
            {
                TrangThaiSachViewModels tt = new TrangThaiSachViewModels()
                {
                    Id = item.Id,
                    TenTT = item.TenTT,
                    TrangThai = item.TrangThai
                };
                lsttt.Add(tt);
            }
            // Phân trang
            int PageSize = 10;
            int PageNumber = (page ?? 1);
            ViewBag.pageSize = PageSize;
            ViewBag.pages = PageNumber;
            return View(lsttt.OrderBy(x => x.TenTT).ToPagedList(PageNumber, PageSize));
        }

        public ActionResult Them()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Them(TrangThaiSachViewModels model)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            TrangThaiSachLogic _TrangThaiSachLogic = new TrangThaiSachLogic(userdata.MyApps[_AppCode].ConnectionString, userdata.MyApps[_AppCode].DatabaseName);

            TrangThaiSach tts = new TrangThaiSach()
            {
                TenTT = model.TenTT,
                TrangThai = model.TrangThai
            };
            _TrangThaiSachLogic.ThemTrangThai(tts);
            return RedirectToAction("Index");
        }

        public ActionResult Sua(string id)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion
            TrangThaiSachLogic _TrangThaiSachLogic = new TrangThaiSachLogic(userdata.MyApps[_AppCode].ConnectionString, userdata.MyApps[_AppCode].DatabaseName);

            TrangThaiSach tts = _TrangThaiSachLogic.getById(id);
            if (tts == null)
                return RedirectToAction("NotFound", "Error");
            TrangThaiSachViewModels VM = new TrangThaiSachViewModels()
            {
                Id = tts.Id,
                TenTT = tts.TenTT,
                TrangThai = tts.TrangThai
            };
            return View(VM);
        }

        [HttpPost]
        public ActionResult Sua(TrangThaiSachViewModels model)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            TrangThaiSachLogic _TrangThaiSachLogic = new TrangThaiSachLogic(userdata.MyApps[_AppCode].ConnectionString, userdata.MyApps[_AppCode].DatabaseName);

            TrangThaiSach tts = _TrangThaiSachLogic.getById(model.Id);
            tts.TenTT = model.TenTT;
            tts.TrangThai = model.TrangThai;
            _TrangThaiSachLogic.SuaTrangThai(tts);
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

            TrangThaiSachLogic _TrangThaiSachLogic = new TrangThaiSachLogic(userdata.MyApps[_AppCode].ConnectionString, userdata.MyApps[_AppCode].DatabaseName);

            var tts = _TrangThaiSachLogic.getById(Id);
            _TrangThaiSachLogic.XoaTrangThai(tts.Id);
            return RedirectToAction("Index");
        }
        #region AngularJS

        public JsonResult Get_AllTrangThaiSach() //JsonResult
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return Json(null, JsonRequestBehavior.AllowGet); //RedirectToAction("LogOff", "Account");
            #endregion
            TrangThaiSachLogic _TrangThaiSachLogic =
                new TrangThaiSachLogic(userdata.MyApps[_AppCode].ConnectionString, userdata.MyApps[_AppCode].DatabaseName);

            var list = _TrangThaiSachLogic.GetAll();
            return Json(list, JsonRequestBehavior.AllowGet);

        }
        #endregion
    }
}