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

            TrangThaiSachLogic _TrangThaiSachLogic = new TrangThaiSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

           
            int PageSize = 10;
            int PageNumber = (page ?? 1);
            var lst = _TrangThaiSachLogic.GetAll();
            List<TrangThaiSachViewModels> lsttt = new List<TrangThaiSachViewModels>();
            foreach (var item in lst)
            {
                TrangThaiSachViewModels tt = new TrangThaiSachViewModels()
                {
                    Id = item.Id,
                    TenTT = item.TenTT
                   
                };
                lsttt.Add(tt);
            }
           
            return View(lsttt.OrderBy(x=>x.TenTT).ToPagedList(PageNumber,PageSize));
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

            TrangThaiSachLogic _TrangThaiSachLogic = new TrangThaiSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

            TrangThaiSach tts = new TrangThaiSach()
            {
                TenTT = model.TenTT
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
            try
            {
                TrangThaiSachLogic _TrangThaiSachLogic = new TrangThaiSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

                TrangThaiSach tts = _TrangThaiSachLogic.getById(id);
                TrangThaiSachViewModels VM = new TrangThaiSachViewModels()
                {
                    Id = tts.Id,
                    TenTT = tts.TenTT
                };
                return View(VM);
            }
            catch (Exception)
            {
                return RedirectToAction("NotFound", "Error");
            }                       
        }

        [HttpPost]
        public ActionResult Sua(TrangThaiSachViewModels model)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            TrangThaiSachLogic _TrangThaiSachLogic = new TrangThaiSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

            TrangThaiSach tts = _TrangThaiSachLogic.getById(model.Id);
            tts.TenTT = model.TenTT;
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

            TrangThaiSachLogic _TrangThaiSachLogic = new TrangThaiSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

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
                new TrangThaiSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

            var list = _TrangThaiSachLogic.GetAll();
            return Json(list, JsonRequestBehavior.AllowGet);

        }
        #endregion
    }
}