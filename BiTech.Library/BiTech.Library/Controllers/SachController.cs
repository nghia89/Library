using BiTech.Library.BLL.DBLogic;
using BiTech.Library.DTO;
using BiTech.Library.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static BiTech.Library.Helpers.Tool;

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
            KeSachLogic _KeSachLogic = new KeSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

            ListBooks model = new ListBooks();

            var list = _SachLogic.getAllSach();
            foreach (var item in list)
            {
                var nxb = _NhaXuatBanLogic.getById(item.IdNhaXuatBan);
                var TL = _TheLoaiSachLogic.getById(item.IdTheLoai);
                var TT = _TrangThaiSachLogic.getById(item.IdTrangThai);

                model.Books.Add(new BookView()
                {
                    Id = item.Id,
                    GiaSach = item.GiaSach.ToString("0.##"),
                    IdDauSach = item.IdDauSach,
                    KeSach = item.IdKeSach, //_KeSachLogic.GetKeSach(item.IdKeSach).Name
                    LinkBiaSach = item.LinkBiaSach,
                    MaKiemSoat = item.MaKiemSoat,
                    NgonNgu = item.NgonNgu.ToString(),
                    NamXuatBan = item.NamSanXuat,
                    SoLuongConLai = item.SoLuong,
                    SoLuongTong = item.SoLuong,
                    TenSach = item.TenSach,
                    TheLoai = _TheLoaiSachLogic.getById(item.IdTheLoai).TenTheLoai,
                    NhaXuatBan = _NhaXuatBanLogic.getById(item.IdNhaXuatBan).Ten,
                    TomTat = item.TomTat
                });
            }

            return View(model);
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
            ViewBag.ListNXB = _NhaXuatBanLogic.GetAllNhaXuatBan();
            ViewBag.ListTT = _TrangThaiSachLogic.GetAll();
            SachViewModels model = new SachViewModels();

            return View(model);
        }

        [HttpPost]
        public ActionResult Them(SachViewModels model)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            if (ModelState.IsValid)
            {
                model.IdNhaXuatBan = model.IdNhaXuatBan.Replace("string:", "");
                model.IdTheLoai = model.IdTheLoai.Replace("string:", "");

                SachLogic _SachLogic = new SachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
                TheLoaiSachLogic _TheLoaiSachLogic = new TheLoaiSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
                NhaXuatBanLogic _NhaXuatBanLogic = new NhaXuatBanLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
                TrangThaiSachLogic _TrangThaiSachLogic = new TrangThaiSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

                ViewBag.ListTheLoai = _TheLoaiSachLogic.GetAllTheLoaiSach();
                ViewBag.ListNXB = _NhaXuatBanLogic.GetAllNhaXuatBan();
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
                    TomTat = model.TomTat,
                    LinkBiaSach = "",
                    SoLuong = 0,
                    CreateDateTime = DateTime.Now
                };
                string id = _SachLogic.ThemSach(s);

                try
                {
                    string physicalWebRootPath = Server.MapPath("~/");

                    string uploadFolder = GetUploadFolder(Helpers.UploadFolder.BookCovers) + id;

                    var uploadFileName = Path.Combine(physicalWebRootPath, uploadFolder, Guid.NewGuid() + Path.GetExtension(model.FileImageCover.FileName));
                    string location = Path.GetDirectoryName(uploadFileName);

                    if (!Directory.Exists(location))
                    {
                        Directory.CreateDirectory(location);
                    }

                    using (FileStream fileStream = new FileStream(uploadFileName, FileMode.Create))
                    {
                        model.FileImageCover.InputStream.CopyTo(fileStream);

                        var book = _SachLogic.GetById(id);
                        book.LinkBiaSach = uploadFileName.Replace(physicalWebRootPath, "./").Replace(@"\",@"/").Replace(@"//",@"/");
                        _SachLogic.Update(book);
                    }
                }
                catch (Exception ex)
                {

                }

                return RedirectToAction("Index");
            }
            return View(model);
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
            ViewBag.ListNXB = _NhaXuatBanLogic.GetAllNhaXuatBan();
            ViewBag.ListTT = _TrangThaiSachLogic.GetAll();
            Sach S = _SachLogic.GetById(id);
            SachViewModels model = new SachViewModels()
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
            return View(model);
        }

        [HttpPost]
        public ActionResult Sua(SachViewModels model)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            if (ModelState.IsValid)
            {
                SachLogic _SachLogic = new SachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
                TheLoaiSachLogic _TheLoaiSachLogic = new TheLoaiSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
                NhaXuatBanLogic _NhaXuatBanLogic = new NhaXuatBanLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
                TrangThaiSachLogic _TrangThaiSachLogic = new TrangThaiSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

                ViewBag.ListTheLoai = _TheLoaiSachLogic.GetAllTheLoaiSach();
                ViewBag.ListNXB = _NhaXuatBanLogic.GetAllNhaXuatBan();
                ViewBag.ListTT = _TrangThaiSachLogic.GetAll();
                Sach S = _SachLogic.GetById(model.Id);
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
                _SachLogic.Update(S);
                return RedirectToAction("Index");
            }
            return View(model);
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
            ViewBag.ListNXB = _NhaXuatBanLogic.GetAllNhaXuatBan();
            ViewBag.ListTT = _TrangThaiSachLogic.GetAll();
            Sach S = _SachLogic.GetById(id);
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
            ViewBag.ListNXB = _NhaXuatBanLogic.GetAllNhaXuatBan();
            ViewBag.ListTT = _TrangThaiSachLogic.GetAll();
            Sach s = _SachLogic.GetById(model.Id);
            _SachLogic.XoaSach(s.Id);
            return RedirectToAction("Index");
        }

        // Popup PartialView

        /// <summary>
        /// Giao diện thêm thể loại
        /// </summary>
        /// <returns></returns>
        public ActionResult RequestThemTheLoaiGui()
        {
            return PartialView("_NhapLoaiSach");
        }

        /// <summary>
        /// Giao diện thêm nhà xuất bản
        /// </summary>
        /// <returns></returns>
        public ActionResult RequestThemNhaXuatBan()
        {
            return PartialView("_NhapNhaXuatBan");
        }
    }
}