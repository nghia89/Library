using BiTech.Library.BLL.DBLogic;
using BiTech.Library.DTO;
using BiTech.Library.Helpers;
using BiTech.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BiTech.Library.Controllers
{
    public class SachController : BaseController
    {
        public ActionResult Index()
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion
            
            SachLogic _SachLogic = new SachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            TheLoaiSachLogic _TheLoaiSachLogic = new TheLoaiSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            NhaXuatBanLogic _NhaXuatBanLogic = new NhaXuatBanLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            TrangThaiSachLogic _TrangThaiSachLogic = new TrangThaiSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

            List<NhaXuatBan> listNXB = new List<NhaXuatBan>();
            List<TheLoaiSach> listTL = new List<TheLoaiSach>();
            List<TrangThaiSach> listTT = new List<TrangThaiSach>();
            var list = _SachLogic.getAllSach();
            foreach(var item in list)
            {
                var nxb = _NhaXuatBanLogic.getById(item.IdNhaXuatBan);
                var TL = _TheLoaiSachLogic.getById(item.IdTheLoai);
                var TT = _TrangThaiSachLogic.getById(item.IdTrangThai);
                listNXB.Add(nxb);
                listTL.Add(TL);
                listTT.Add(TT);
            }
            ViewBag.ListSach = list;
            ViewBag.listNXB = listNXB;
            ViewBag.listTL = listTL;
            ViewBag.listTT = listTT;
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
            NhaXuatBanLogic _NhaXuatBanLogic = new NhaXuatBanLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            TrangThaiSachLogic _TrangThaiSachLogic = new TrangThaiSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);


            ViewBag.ListTheLoai = _TheLoaiSachLogic.GetAllTheLoaiSach();
            ViewBag.ListNXB = _NhaXuatBanLogic.getAllNhaXuatBan();
            ViewBag.ListTT = _TrangThaiSachLogic.GetAll();
            return View();
        }

        [HttpPost]
        public ActionResult Them(SachViewModels model)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            SachLogic _SachLogic = new SachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            TheLoaiSachLogic _TheLoaiSachLogic = new TheLoaiSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            NhaXuatBanLogic _NhaXuatBanLogic = new NhaXuatBanLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            TrangThaiSachLogic _TrangThaiSachLogic = new TrangThaiSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

            ViewBag.ListTheLoai= _TheLoaiSachLogic.GetAllTheLoaiSach();
            ViewBag.ListNXB = _NhaXuatBanLogic.getAllNhaXuatBan();
            ViewBag.ListTT = _TrangThaiSachLogic.GetAll();
            Sach s = new Sach()
            {
                TenSach = model.TenSach,
                IdDauSach = model.IdDauSach,
                IdTheLoai = model.IdTheLoai,
                IdKeSach = model.IdKeSach,
                IdNhaXuatBan = model.IdNhaXuatBan,
                IdTrangThai = model.IdTrangThai,
                MaKiemSoat = model.MaKiemSoat,
                Hinh = model.Hinh,
                SoTrang = model.SoTrang,
                NgonNgu = model.NgonNgu,
                NamSanXuat = model.NamSanXuat,
                GiaSach = model.GiaSach,
                LinkBiaSach = model.LinkBiaSach,
                TomTat = model.TomTat
            };
            _SachLogic.ThemSach(s);
            return RedirectToAction("Index");
        }
      
        public ActionResult Sua(string id)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            SachLogic _SachLogic = new SachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            TheLoaiSachLogic _TheLoaiSachLogic = new TheLoaiSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            NhaXuatBanLogic _NhaXuatBanLogic = new NhaXuatBanLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            TrangThaiSachLogic _TrangThaiSachLogic = new TrangThaiSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

            ViewBag.ListTheLoai = _TheLoaiSachLogic.GetAllTheLoaiSach();
            ViewBag.ListNXB = _NhaXuatBanLogic.getAllNhaXuatBan();
            ViewBag.ListTT = _TrangThaiSachLogic.GetAll();
            Sach S = _SachLogic.getById(id);
            SachViewModels VM = new SachViewModels()
            {
                Id = S.Id,
                TenSach = S.TenSach,
                IdDauSach = S.IdDauSach,
                IdTheLoai = S.IdTheLoai,
                IdKeSach = S.IdKeSach,
                IdNhaXuatBan = S.IdNhaXuatBan,
                IdTrangThai = S.IdTrangThai,
                MaKiemSoat = S.MaKiemSoat,
                Hinh = S.Hinh,
                SoTrang = S.SoTrang,
                NgonNgu = S.NgonNgu,
                NamSanXuat = S.NamSanXuat,
                GiaSach = S.GiaSach,
                LinkBiaSach = S.LinkBiaSach,
                TomTat = S.TomTat
            };
            return View(VM);
        }

        [HttpPost]
        public ActionResult Sua(SachViewModels model)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            SachLogic _SachLogic = new SachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            TheLoaiSachLogic _TheLoaiSachLogic = new TheLoaiSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            NhaXuatBanLogic _NhaXuatBanLogic = new NhaXuatBanLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            TrangThaiSachLogic _TrangThaiSachLogic = new TrangThaiSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

            ViewBag.ListTheLoai = _TheLoaiSachLogic.GetAllTheLoaiSach();
            ViewBag.ListNXB = _NhaXuatBanLogic.getAllNhaXuatBan();
            ViewBag.ListTT = _TrangThaiSachLogic.GetAll();
            Sach S = _SachLogic.getById(model.Id);
            S.TenSach = model.TenSach;
            S.IdDauSach = model.IdDauSach;
            S.IdTheLoai = model.IdTheLoai;
            S.IdKeSach = model.IdKeSach;
            S.IdNhaXuatBan = model.IdNhaXuatBan;
            S.IdTrangThai = model.IdTrangThai;
            S.MaKiemSoat = model.MaKiemSoat;
            S.Hinh = model.Hinh;
            S.SoTrang = model.SoTrang;
            S.NgonNgu = model.NgonNgu;
            S.NamSanXuat = model.NamSanXuat;
            S.GiaSach = model.GiaSach;
            S.LinkBiaSach = model.LinkBiaSach;
            S.TomTat = model.TomTat;
            _SachLogic.SuaSach(S);
            return RedirectToAction("Index");
        }

        public ActionResult Xoa(string id)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            SachLogic _SachLogic = new SachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            TheLoaiSachLogic _TheLoaiSachLogic = new TheLoaiSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            NhaXuatBanLogic _NhaXuatBanLogic = new NhaXuatBanLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            TrangThaiSachLogic _TrangThaiSachLogic = new TrangThaiSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

            ViewBag.ListTheLoai = _TheLoaiSachLogic.GetAllTheLoaiSach();
            ViewBag.ListNXB = _NhaXuatBanLogic.getAllNhaXuatBan();
            ViewBag.ListTT = _TrangThaiSachLogic.GetAll();
            Sach S = _SachLogic.getById(id);
            SachViewModels VM = new SachViewModels()
            {
                Id = S.Id,
                TenSach = S.TenSach,
                IdDauSach = S.IdDauSach,
                IdTheLoai = S.IdTheLoai,
                IdKeSach = S.IdKeSach,
                IdNhaXuatBan = S.IdNhaXuatBan,
                IdTrangThai = S.IdTrangThai,
                MaKiemSoat = S.MaKiemSoat,
                Hinh = S.Hinh,
                SoTrang = S.SoTrang,
                NgonNgu = S.NgonNgu,
                NamSanXuat = S.NamSanXuat,
                GiaSach = S.GiaSach,
                LinkBiaSach = S.LinkBiaSach,
                TomTat = S.TomTat
            };
            return View(VM);
        }

        [HttpPost]
        public ActionResult Xoa(SachViewModels model)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            SachLogic _SachLogic = new SachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            TheLoaiSachLogic _TheLoaiSachLogic = new TheLoaiSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            NhaXuatBanLogic _NhaXuatBanLogic = new NhaXuatBanLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            TrangThaiSachLogic _TrangThaiSachLogic = new TrangThaiSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

            ViewBag.ListTheLoai = _TheLoaiSachLogic.GetAllTheLoaiSach();
            ViewBag.ListNXB = _NhaXuatBanLogic.getAllNhaXuatBan();
            ViewBag.ListTT = _TrangThaiSachLogic.GetAll();
            Sach s = _SachLogic.getById(model.Id);
            _SachLogic.XoaSach(s.Id);
            return RedirectToAction("Index");
        }
    }
}