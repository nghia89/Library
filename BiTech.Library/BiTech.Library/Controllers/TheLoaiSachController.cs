using BiTech.Library.BLL.DBLogic;
using BiTech.Library.Models;
using BiTech.Library.DTO;
using BiTech.Library.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace BiTech.Library.Controllers
{
    public class TheLoaiSachController : BaseController
    {

        // GET: TheLoaiSach
        public ActionResult Index(int? page, string idParent)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            TheLoaiSachLogic _TheLoaiSachLogic = new TheLoaiSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

            var list = _TheLoaiSachLogic.GetAllTheLoaiSach();
            List<TheLoaiSachViewModels> list_viewmode = new List<TheLoaiSachViewModels>();
            foreach (TheLoaiSach item in list)
            {
                TheLoaiSachViewModels Theloai = new TheLoaiSachViewModels();
                Theloai.Id = item.Id;
                Theloai.IdParent = item.IdParent;
                Theloai.TenTheLoai = item.TenTheLoai;
                if (item.IdParent != null)
                {
                    var parent = _TheLoaiSachLogic.getById(item.IdParent);
                    if (parent != null)
                        Theloai.TenTheLoaiParent = parent.TenTheLoai;
                }
                Theloai.MoTa = item.MoTa;
                list_viewmode.Add(Theloai);
            }

            //filter
            var folder = _TheLoaiSachLogic.getById(idParent);
            if (idParent != null)
            {
                //Nếu các thể loại con của idParent
                list_viewmode = list_viewmode.Where(_ => _.IdParent == idParent).ToList();
                
                if(folder != null)
                {
                    ViewBag.URLBackParent = (folder.IdParent != null) ? "/TheLoaiSach?idParent=" + folder.IdParent : "/TheLoaiSach";              
                }
            }
            else
            {
                //Lấy các câu không có idParent
                list_viewmode = list_viewmode.Where(_ => _.IdParent == null).ToList();
            }

            var list_viewmode_view = list_viewmode.OrderBy(_ => _.TenTheLoai);
            ViewBag.idParent = (idParent == null || idParent == "") ? null : idParent;
            ViewBag.url = GetViewLinkURL(folder);
            // Phân trang
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            ViewBag.pageSize = pageSize;
            ViewBag.pages = pageNumber;
            return View(list_viewmode_view.ToPagedList(pageNumber, pageSize));
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
                    IdParent = model.IdParent,
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

            var folder = _TheLoaiSachLogic.getById(id);
            if (folder != null)
            {
                ViewBag.URLBackParent = (folder.IdParent != null) ? "/TheLoaiSach?idParent=" + folder.IdParent : "/TheLoaiSach";
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
            //return RedirectToAction("Index");
            return RedirectToAction("Index", "TheLoaiSach", new { idParent = TLS.IdParent });
        }

     

        [HttpPost]
        public ActionResult Xoa(string Id)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            TheLoaiSachLogic _TheLoaiSachLogic = new TheLoaiSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

           var TLS = _TheLoaiSachLogic.getById(Id);
            _TheLoaiSachLogic.XoaTheLoaiSach(TLS.Id);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Giao diện thêm thể loại
        /// </summary>
        /// <returns></returns>
        public ActionResult RequestThemTheLoaiGui(string idParent)
        {
            ViewBag.idParent = (idParent == null || idParent == "") ? null : idParent;
            return PartialView("_NhapLoaiSach");
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


        #region Phong function
        private string GetViewLinkURL(TheLoaiSach TLS)
        {
            //
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            #endregion

            TheLoaiSachLogic _TheLoaiSachLogic = new TheLoaiSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

            string kq = "";
            if (TLS == null)
                return kq;
            else
            {
                kq = kq + "<a href=/TheLoaiSach?idParent="+ TLS.Id+">/" + TLS.TenTheLoai + "</a>";
                TheLoaiSach TLSParent = _TheLoaiSachLogic.getById(TLS.IdParent);
                kq = GetViewLinkURL(TLSParent) + kq;
            }
            return kq;
        }
        #endregion
    }
}