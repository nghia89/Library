using BiTech.Library.BLL.DBLogic;
using BiTech.Library.Models;
using BiTech.Library.DTO;
using BiTech.Library.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BiTech.Library.Controllers
{
    public class TheLoaiSachController : BaseController
    {

        // GET: TheLoaiSach
        public ActionResult Index()
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            TheLoaiSachLogic _TheLoaiSachLogic = new TheLoaiSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

            var list = _TheLoaiSachLogic.GetAllTheLoaiSach();
            List<TheLoaiSachViewModels> list_viewmode = new List<TheLoaiSachViewModels>();
            foreach(TheLoaiSach item in list)
            {
                TheLoaiSachViewModels Theloai = new TheLoaiSachViewModels();
                Theloai.Id = item.Id;
                Theloai.IdParent = item.IdParent;
                Theloai.TenTheLoai = item.TenTheLoai;
                if(item.IdParent != null)
                {
                    var parent = _TheLoaiSachLogic.getById(item.IdParent);
                    if(parent != null)
                        Theloai.TenTheLoaiParent = parent.TenTheLoai;
                }
                Theloai.MoTa = item.MoTa;
                list_viewmode.Add(Theloai);
            }
            ViewBag.ListTheLoai = list_viewmode;
            return View();
        }

        public ActionResult Them()
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            TheLoaiSachLogic _TheLoaiSachLogic = new TheLoaiSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            ViewBag.ListTheLoai = _TheLoaiSachLogic.GetAllTheLoaiSach();

            return View();
        }

        [HttpPost]
        public ActionResult Them(TheLoaiSach model)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            TheLoaiSachLogic _TheLoaiSachLogic = new TheLoaiSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

            TheLoaiSach TLS = new TheLoaiSach()
            {
                IdParent = model.IdParent,
                TenTheLoai = model.TenTheLoai,
                MoTa = model.MoTa
            };
            
            _TheLoaiSachLogic.ThemTheLoaiSach(TLS);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult ThemAjax(TheLoaiSach model)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            TheLoaiSachLogic _TheLoaiSachLogic = new TheLoaiSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

            try
            {
                TheLoaiSach TLS = new TheLoaiSach()
                {
                    TenTheLoai = model.TenTheLoai,
                    MoTa = model.MoTa
                };
                _TheLoaiSachLogic.ThemTheLoaiSach(TLS);
                return Json(true);
            }
            catch { return Json(false); }
        }

        public ActionResult Sua(string id)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            TheLoaiSachLogic _TheLoaiSachLogic = new TheLoaiSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

            TheLoaiSach TLS = _TheLoaiSachLogic.getById(id);
            TheLoaiSachViewModels VM = new TheLoaiSachViewModels()
            {
                Id = TLS.Id,
                IdParent = TLS.IdParent,
                TenTheLoai = TLS.TenTheLoai,
                MoTa = TLS.MoTa,
            };
            var list_root = _TheLoaiSachLogic.GetAllTheLoaiSachRoot();
            List<TheLoaiSach> ListTheLoai = new List<TheLoaiSach>();
            foreach(TheLoaiSach item in list_root)
            {
                ListTheLoai.Add(item);
                ListTheLoai.AddRange(ListTheLoaiChildren(item, _TheLoaiSachLogic));
            }

            ViewBag.ListTheLoai = ListTheLoai;
            return View(VM);
        }

        public List<TheLoaiSach> ListTheLoaiChildren(TheLoaiSach _TheLoaiSach, TheLoaiSachLogic _TheLoaiSachLogic)
        {
            List<TheLoaiSach> kq = new List<TheLoaiSach>();
            var list = _TheLoaiSachLogic.GetAllTheLoaiSachChildren(_TheLoaiSach.Id);
            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    item.TenTheLoai = " " + item.TenTheLoai;
                    kq.Add(item);
                    kq.AddRange(ListTheLoaiChildren(item, _TheLoaiSachLogic));
                }
            }
            return kq;
        }

        [HttpPost]
        public ActionResult Sua(TheLoaiSachViewModels model)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            TheLoaiSachLogic _TheLoaiSachLogic = new TheLoaiSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

            TheLoaiSach TLS = _TheLoaiSachLogic.getById(model.Id);
            TLS.IdParent = model.IdParent;
            TLS.TenTheLoai = model.TenTheLoai;
            TLS.MoTa = model.MoTa;
            _TheLoaiSachLogic.SuaTheLoaiSach(TLS);
            return RedirectToAction("Index");
        }

        public ActionResult Xoa(string id)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            TheLoaiSachLogic _TheLoaiSachLogic = new TheLoaiSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

            TheLoaiSach TLS = _TheLoaiSachLogic.getById(id);
            TheLoaiSachViewModels VM = new TheLoaiSachViewModels()
            {
                Id = TLS.Id,
                TenTheLoai = TLS.TenTheLoai,
                MoTa = TLS.MoTa,
            };
            return View(VM);
        }

        [HttpPost]
        public ActionResult Xoa(TheLoaiSachViewModels model)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            TheLoaiSachLogic _TheLoaiSachLogic = new TheLoaiSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

            TheLoaiSach TLS = _TheLoaiSachLogic.getById(model.Id);
            _TheLoaiSachLogic.XoaTheLoaiSach(TLS.Id);
            return RedirectToAction("Index");
        }

        #region AngularJS

        public JsonResult Get_AllTheLoaiSach() //JsonResult
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return Json(null, JsonRequestBehavior.AllowGet); //RedirectToAction("LogOff", "Account");
            #endregion
            TheLoaiSachLogic _TheLoaiSachLogic =
                new TheLoaiSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

            var list = _TheLoaiSachLogic.GetAllTheLoaiSach();
            return Json(list, JsonRequestBehavior.AllowGet);

        }
        #endregion
    }
}