using BiTech.Library.BLL.DBLogic;
using BiTech.Library.Models;
using BiTech.Library.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BiTech.Library.Controllers
{
    public class NhapSachController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NhapSach()
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            NhapSachLogic _NhapSachLogic = new NhapSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            SachLogic _SachLogic = new SachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

            var list = _SachLogic.getAllSach();

            List<SachViewModels> ListdsSach = new List<SachViewModels>();
            foreach (var item in list)
            {
                SachViewModels model = new SachViewModels
                {
                    Id = item.Id,
                    TenSach = item.TenSach,
                    IdDauSach = item.IdDauSach,
                    IdTheLoai = item.IdTheLoai,
                    IdKeSach = item.IdKeSach,
                    IdNhaXuatBan = item.IdNhaXuatBan,
                    MaKiemSoat = item.MaKiemSoat,
                    Hinh = item.Hinh,
                    NgonNgu = item.NgonNgu,
                    NamSanXuat = item.NamSanXuat,
                    GiaSach = item.GiaSach,
                    LinkBiaSach = item.LinkBiaSach,
                    SoLuong = item.SoLuong
                };
                ListdsSach.Add(model);
            }
            ViewBag.DSSach = ListdsSach;
            return View();
        }

        [HttpPost]
        public ActionResult NhapSach(NhapSachViewModels model)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            NhapSachLogic _NhapSachLogic = new NhapSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

            NhapSach NS = new NhapSach()
            {
                Id = model.Id,
                TenSach = model.TenSach,
                SoLuong =model.SoLuong
            };
            _NhapSachLogic.NhapSach(NS);
            return View();
        }
        
    }
}