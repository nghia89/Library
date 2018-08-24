using BiTech.Library.BLL.DBLogic;
using BiTech.Library.BLL.BarCode_QR;
using BiTech.Library.Common;
using BiTech.Library.DTO;
using BiTech.Library.Models;
using Newtonsoft.Json;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static BiTech.Library.Helpers.Tool;
using BiTech.Library.Controllers.BaseClass;

namespace BiTech.Library.Controllers
{
    public class SachController : BaseController
    {
        SachCommon sachCommon;
        public SachController()
        {
            sachCommon = new SachCommon();
        }
		
        public ActionResult Index(KeySearchViewModel KeySearch, int? page)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion                   

            SachLogic _SachLogic = new SachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            TheLoaiSachLogic _TheLoaiSachLogic = new TheLoaiSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            NhaXuatBanLogic _NhaXuatBanLogic = new NhaXuatBanLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            KeSachLogic _KeSachLogic = new KeSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            LanguageLogic _LanguageLogic = new LanguageLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            TacGiaLogic _TacGiaLogic = new TacGiaLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            SachTacGiaLogic _SachTacGiaLogic = new SachTacGiaLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

            ListBooksModel model = new ListBooksModel();

            int pageSize = 30;
            int pageNumber = (page ?? 1);
            ViewBag.theLoaiSach = _TheLoaiSachLogic.GetAllTheLoaiSach();
            ViewBag.tacGia = _TacGiaLogic.GetAllTacGia();
            ViewBag.NXB = _NhaXuatBanLogic.GetAllNhaXuatBan();

            var list = _SachLogic.getPageSach(KeySearch);
            ViewBag.number = list.Count();
            foreach (var item in list)
            {
                var IdSachTG = _SachTacGiaLogic.getById(item.Id);
                BookView book = new BookView(item);

                book.Ten_TheLoai = _TheLoaiSachLogic.getById(item.IdTheLoai)?.TenTheLoai ?? "--";
                book.Ten_NhaXuatBan = _NhaXuatBanLogic.getById(item.IdNhaXuatBan)?.Ten ?? "--";
                book.Ten_KeSach = _KeSachLogic.getById(item.IdKeSach)?.TenKe ?? "--";
                book.Ten_NgonNgu = _LanguageLogic.GetById(item.IdNgonNgu)?.Ten ?? "--";
                book.Ten_TacGia = _TacGiaLogic.GetByIdTG(IdSachTG.IdTacGia)?.TenTacGia ?? "--";

                model.Books.Add(book);
            }

            return View(model.Books.ToPagedList(pageNumber, pageSize));
        }

        public PartialViewResult _PartSlAndTT()
        {
            return PartialView();
        }

        public ActionResult Create()
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            LanguageLogic _LanguageLogic = new LanguageLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            TacGiaLogic _TacGiaLogic = new TacGiaLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

            //var idTG = _TacGiaLogic.GetAllTacGia();
            //ViewBag.IdTacGia = idTG;

            SachUploadModel model = new SachUploadModel();
            model.Languages = _LanguageLogic.GetAll();

            ViewBag.Message = TempData["ThemSachMsg"] = "";
            return View(model.SachDTO);
        }

        [HttpPost]
        public ActionResult Create(SachUploadModel model)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            LanguageLogic _LanguageLogic = new LanguageLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            ViewBag.Message = TempData["ThemSachMsg"] = "";

            if (ModelState.IsValid)
            {
                SachLogic _SachLogic = new SachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
                SachTacGiaLogic _SachTacGiaLogic = new SachTacGiaLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
                TacGiaLogic _TacGiaLogic = new TacGiaLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

                string id = _SachLogic.ThemSach(model.SachDTO);

                if (id.Length > 0)
                {
                    string failTG = "";
                    foreach (var tg in model.ListTacGiaJson)
                    {
                        var item = JsonConvert.DeserializeObject<TacGiaViewModel>(tg);
                        string tgId = "";

                        if (string.IsNullOrEmpty(item.Id))
                        {
                            tgId = _TacGiaLogic.Insert(new TacGia() { TenTacGia = item.TenTacGia, MoTa = "", QuocTich = "" });
                        }
                        else
                        {
                            tgId = _TacGiaLogic.GetById(item.Id)?.Id ?? "";
                        }

                        if (!string.IsNullOrEmpty(tgId))
                        {
                            _SachTacGiaLogic.ThemSachTacGia(new SachTacGia()
                            {
                                IdSach = id,
                                IdTacGia = tgId
                            });
                        }
                        else
                        {
                            failTG += item.TenTacGia + ", ";
                        }
                    }

                    if (model.FileImageCover != null)
                    {
                        try
                        {
                            string physicalWebRootPath = Server.MapPath("~/");
                            string uploadFolder = GetUploadFolder(Helpers.UploadFolder.BookCovers) + id;

                            var uploadFileName = Path.Combine(physicalWebRootPath, uploadFolder, Guid.NewGuid()
                                + Path.GetExtension(model.FileImageCover.FileName));
                            string location = Path.GetDirectoryName(uploadFileName);

                            if (!Directory.Exists(location))
                            {
                                Directory.CreateDirectory(location);
                            }

                            using (FileStream fileStream = new FileStream(uploadFileName, FileMode.Create))
                            {
                                model.FileImageCover.InputStream.CopyTo(fileStream);

                                var book = _SachLogic.GetById(id);
                                book.LinkBiaSach = uploadFileName.Replace(physicalWebRootPath, "/").Replace(@"\", @"/").Replace(@"//", @"/");
                                _SachLogic.Update(book);
                            }
                        }
                        catch { }
                    }
                    // Lưu mã QR
                    try
                    {
                        Sach sach = _SachLogic.GetBookById(id);
                        string physicalWebRootPath = Server.MapPath("/");
                        Sach temp = sachCommon.LuuMaVachSach(physicalWebRootPath, sach, null);
                        if (temp != null)
                        {
                            sach.QRlink = temp.QRlink;
                            sach.QRData = temp.QRData;
                            _SachLogic.Update(sach);
                        }
                    }
                    catch { }
                    if (failTG.Length > 0)
                    {
                        failTG = failTG.Substring(0, failTG.Length - 2);
                        TempData["ThemSachMsg"] = string.Format("Chú ý: Chọn tác giả {0} thất bại, vui lòng cập nhật sau.", failTG);
                    }
                    return RedirectToAction("Index");
                }
                TempData["ThemSachMsg"] = "Thêm sách thất bại";
            }
            model.Languages = _LanguageLogic.GetAll();

            return View(model);
        }

        public ActionResult CreateSuccess()
        {
            ViewBag.Message = TempData["ThemSachMsg"];

            return View();
        }

        public ActionResult Edit(string id)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("Index");
            }

            SachLogic _SachLogic = new SachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            LanguageLogic _LanguageLogic = new LanguageLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            TacGiaLogic _TacGiaLogic = new TacGiaLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            SoLuongSachTrangThaiLogic _SlTrangThaisach = new SoLuongSachTrangThaiLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

            Sach sachDTO = _SachLogic.GetById(id);
            if (sachDTO == null)
            {
                return RedirectToAction("Index");
            }
            var sltts = _SlTrangThaisach.GetByFindId(id);
            ViewBag.SlTTsach = sltts;
            var idTG = _TacGiaLogic.GetAllTacGia();
            ViewBag.IdTacGia = idTG;
            SachUploadModel model = new SachUploadModel(sachDTO);
            model.Languages = _LanguageLogic.GetAll();
            ViewBag.TLS = model.SachDTO.IdTheLoai;
            ViewBag.NXB = model.SachDTO.IdNhaXuatBan;
            ViewBag.KeSach = model.SachDTO.IdKeSach;
            ViewBag.NgonNgu = model.SachDTO.IdNgonNgu;
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(SachUploadModel model)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            if (ModelState.IsValid)
            {
                SachLogic _SachLogic = new SachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
                SachTacGiaLogic _SachTacGiaLogic = new SachTacGiaLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
                TacGiaLogic _TacGiaLogic = new TacGiaLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

                Sach sach = _SachLogic.GetBookById(model.SachDTO.Id);
                if (sach != null)
                {
//<<<<<<< HEAD
                    // Id = model.SachDTO.Id,
                    // IdTheLoai = model.SachDTO.IdTheLoai,
                    // IdKeSach = model.SachDTO.IdKeSach,
                    // IdNhaXuatBan = model.SachDTO.IdNhaXuatBan,
                    // MaKiemSoat = model.SachDTO.MaKiemSoat,
                    // SoLuongTong = model.SachDTO.SoLuongTong,
                    // SoTrang = model.SachDTO.SoTrang,
                    // IdNgonNgu = model.SachDTO.IdNgonNgu,
                    // NamXuatBan = model.SachDTO.NamXuatBan,
                    // GiaBia = model.SachDTO.GiaBia,
                    // TaiBan = model.SachDTO.TaiBan,
                    // TenSach = model.SachDTO.TenSach,
                    // TomTat = model.SachDTO.TomTat,
                    // PhiMuonSach = model.SachDTO.PhiMuonSach,
                    // ISBN=model.SachDTO.ISBN,
                    // XuatXu=model.SachDTO.XuatXu
//=======
                    sach.Id = model.SachDTO.Id;
                    //sach.MaKiemSoat = model.SachDTO.MaKiemSoat;
                    sach.DDC = model.SachDTO.DDC;
                    sach.TenSach = model.SachDTO.TenSach;
                    sach.ISBN = model.SachDTO.ISBN;
                    sach.IdTheLoai = model.SachDTO.IdTheLoai;
                    sach.IdNhaXuatBan = model.SachDTO.IdNhaXuatBan;
                    sach.IdKeSach = model.SachDTO.IdKeSach;
                    sach.SoTrang = model.SachDTO.SoTrang;
                    sach.IdNgonNgu = model.SachDTO.IdNgonNgu;
                    sach.NamXuatBan = model.SachDTO.NamXuatBan;
                    sach.GiaBia = model.SachDTO.GiaBia;
                    sach.PhiMuonSach = model.SachDTO.PhiMuonSach;
                    sach.XuatXu = model.SachDTO.XuatXu;
                    sach.NguoiBienDich = model.SachDTO.NguoiBienDich;
                    sach.TaiBan = model.SachDTO.TaiBan;
                    sach.TomTat = model.SachDTO.TomTat;
                    //sach.SoLuongTong = model.SachDTO.SoLuongTong;
//>>>>>>> Phongv25
                    //LinkBiaSach = model.FileImageCover.ToString()

                    string failTG = "";
                    if (_SachTacGiaLogic.DeleteAllTacGiaByidSach(sach.Id))
                    {
                        foreach (var tg in model.ListTacGiaJson)
                        {
                            var item = JsonConvert.DeserializeObject<TacGiaViewModel>(tg);
                            string tgId = "";

                            if (!string.IsNullOrEmpty(item.TenTacGia))
                            {
                                TacGia tg_temp = _TacGiaLogic.GetByTenTacGia(item.TenTacGia);
                                if (tg_temp != null)
                                {
                                    tgId = tg_temp.Id;
                                }
                                else
                                {

                                    tgId = _TacGiaLogic.Insert(new TacGia() { TenTacGia = item.TenTacGia, MoTa = "", QuocTich = "" });
                                }
                            }

                            if (!string.IsNullOrEmpty(tgId))
                            {
                                _SachTacGiaLogic.ThemSachTacGia(new SachTacGia()
                                {
                                    IdSach = sach.Id,
                                    IdTacGia = tgId
                                });
                            }
                            else
                            {
                                failTG += item.TenTacGia + ", ";
                            }
                        }
                    }
                }

                if (model.FileImageCover != null)
                {
                    try
                    {
                        string physicalWebRootPath = Server.MapPath("~/");
                        string uploadFolder = GetUploadFolder(Helpers.UploadFolder.BookCovers) + model.SachDTO.Id;

                        var uploadFileName = Path.Combine(physicalWebRootPath, uploadFolder, Guid.NewGuid()
                            + Path.GetExtension(model.FileImageCover.FileName));
                        string location = Path.GetDirectoryName(uploadFileName);

                        if (!Directory.Exists(location))
                        {
                            Directory.CreateDirectory(location);
                        }

                        using (FileStream fileStream = new FileStream(uploadFileName, FileMode.Create))
                        {
                            model.FileImageCover.InputStream.CopyTo(fileStream);

                            //var book = _SachLogic.GetById(model.SachDTO.Id);
                            sach.LinkBiaSach = uploadFileName.Replace(physicalWebRootPath, "/").Replace(@"\", @"/").Replace(@"//", @"/");

                        }
                    }
                    catch { }
                }

                #region Tai

                try
                {
                    // cập nhật QR
                    string physicalWebRootPath = Server.MapPath("/");
                    string uploadFolder = GetUploadFolder(Helpers.UploadFolder.QRCodeUser);
                    string imageName = null;
                    if (sach.QRlink != null)
                        imageName = sach.QRlink.Replace(@"/Upload/QRCodeUser/", @"").Replace(@"/", @"\").Replace(@"/", @"//");
                    Sach temp = sachCommon.LuuMaVachSach(physicalWebRootPath, sach, imageName);
                    if (temp != null)
                    {
                        sach.QRlink = temp.QRlink;
                        sach.QRData = temp.QRData;
                    }
                }
                catch { }

                #endregion

                _SachLogic.Update(sach);

                return RedirectToAction("Index");
            }

            LanguageLogic _LanguageLogic = new LanguageLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            model.Languages = _LanguageLogic.GetAll();
            ViewBag.TLS = model.SachDTO.IdTheLoai;
            ViewBag.NXB = model.SachDTO.IdNhaXuatBan;
            return View(model);
        }

        public JsonResult GetByFindId(string Id)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            //if (userdata == null)
            //    return RedirectToAction("LogOff", "Account");
            if (userdata == null)
                return Json(null, JsonRequestBehavior.AllowGet);
            #endregion
            SoLuongSachTrangThaiLogic _SlTrangThaisach = new SoLuongSachTrangThaiLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            TrangThaiSachLogic _TrangThaiSachLogic = new TrangThaiSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

            var model = _SlTrangThaisach.GetByFindId(Id);
            var tt = _TrangThaiSachLogic.GetAll();
            List<SoLuongTrangThaiSachVM> list = new List<SoLuongTrangThaiSachVM>();
            foreach (var i in model)
            {
                SoLuongTrangThaiSachVM vm = new SoLuongTrangThaiSachVM();
                foreach (var item in tt)
                {

                    if (item.Id == i.IdTrangThai)
                    {
                        vm.Id = i.Id;
                        vm.IdSach = i.IdSach;
                        vm.SoLuong = i.SoLuong;
                        vm.TrangThai = item.TenTT;
                        vm.IdTrangThai = i.IdTrangThai;
                    }
                }
                list.Add(vm);
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EditSaveChange(SoLuongSachTrangThai vm, string txtIdttCategory)
        {
            var userdata = GetUserData();
            //if (userdata == null)
            //    return RedirectToAction("LogOff", "Account");
            if (userdata == null)
                return Json(null, JsonRequestBehavior.AllowGet);

            SoLuongSachTrangThaiLogic _SlTrangThaisach = new SoLuongSachTrangThaiLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            var id = _SlTrangThaisach.GetById(vm.Id);
            int numberSl = id.SoLuong - vm.SoLuong;
            var IdSlTT = _SlTrangThaisach.GetByIdTT(txtIdttCategory, vm.IdSach);
            if (IdSlTT != null)
            {
                SoLuongSachTrangThai md = new SoLuongSachTrangThai();

                md.Id = IdSlTT.Id;
                md.IdSach = IdSlTT.IdSach;
                md.IdTrangThai = IdSlTT.IdTrangThai;
                md.SoLuong = IdSlTT.SoLuong + vm.SoLuong;
                _SlTrangThaisach.Update(md);
            }
            else
            {
                SoLuongSachTrangThai md = new SoLuongSachTrangThai();

                md.Id = vm.Id;
                md.IdSach = vm.IdSach;
                md.IdTrangThai = txtIdttCategory;
                md.SoLuong = vm.SoLuong;
                _SlTrangThaisach.Insert(md);
            }
            vm.SoLuong = numberSl;
            var model = _SlTrangThaisach.Update(vm);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetById(string Id)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            //if (userdata == null)
            //    return RedirectToAction("LogOff", "Account");
            if (userdata == null)
                return Json(null, JsonRequestBehavior.AllowGet);
            #endregion
            SoLuongSachTrangThaiLogic _SlTrangThaisach = new SoLuongSachTrangThaiLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            TrangThaiSachLogic _TrangThaiSachLogic = new TrangThaiSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

            var model = _SlTrangThaisach.GetById(Id);
            var tt = _TrangThaiSachLogic.GetAll();

            SoLuongTrangThaiSachVM vm = new SoLuongTrangThaiSachVM();
            foreach (var item in tt)
            {
                if (item.Id == model.IdTrangThai)
                {
                    vm.Id = model.Id;
                    vm.IdSach = model.IdSach;
                    vm.SoLuong = model.SoLuong;
                    vm.TrangThai = item.TenTT;
                    vm.IdTrangThai = model.IdTrangThai;
                }
            }

            return Json(vm, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetAllTT(string id)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            //if (userdata == null)
            //    return RedirectToAction("LogOff", "Account");
            if (userdata == null)
                return Json(null, JsonRequestBehavior.AllowGet);
            #endregion
            TrangThaiSachLogic _trangThaiSachLogic = new TrangThaiSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            var model = _trangThaiSachLogic.GetAllTT(id);

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(string id)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            SachLogic _SachLogic = new SachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

            Sach s = _SachLogic.GetById(id);
            s.IsDeleted = true;
            _SachLogic.Update(s);
            //_SachLogic.XoaSach(s.Id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult DeleteForever(string id)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            //SachLogic _SachLogic = new SachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

            //Sach s = _SachLogic.GetById(id);
            // todo Xoa het ca hoi lien quan
            //_SachLogic.XoaSach(s.Id);
            return RedirectToAction("Index");
        }

        public JsonResult ListName(string q)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            //if (userdata == null)
            //    return RedirectToAction("LogOff", "Account");
            if (userdata == null)
                return Json(null, JsonRequestBehavior.AllowGet);
            #endregion
            SachLogic _SachLogic = new SachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            var data = _SachLogic.ListName(q);
            return Json(new
            {
                data = data,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }

        // Popup PartialView

        /// <summary>
        /// Giao diện thêm thể loại
        /// </summary>
        /// <returns></returns>
        public ActionResult RequestThemTheLoaiGui()
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return Json(null, JsonRequestBehavior.AllowGet);
            #endregion

            TheLoaiSachLogic _TheLoaiSachLogic =
                new TheLoaiSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            
            ViewBag.ListTheLoai = _TheLoaiSachLogic.GetAllTheLoaiSach(true);
            return PartialView("_NhapLoaiSach");
        }

        public ActionResult RequestThemTheKeSach()
        {
            return PartialView("_NhapkeSach");
        }

        public ActionResult RequestLanguage()
        {
            return PartialView("_Language");
        }

        /// <summary>
        /// Giao diện thêm nhà xuất bản
        /// </summary>
        /// <returns></returns>
        public ActionResult RequestThemNhaXuatBan()
        {
            return PartialView("_NhapNhaXuatBan");
        }

        public ActionResult ThemTacGia()
        {
            return PartialView("_ThemTacGia");
        }
        public ActionResult ImportFromExcel()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ImportFromExcel(SachViewModels model)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            //if (userdata == null)
            //    return RedirectToAction("LogOff", "Account");
            if (userdata == null)
                return Json(null, JsonRequestBehavior.AllowGet);
            #endregion
            SachLogic _SachLogic = new SachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            TheLoaiSachLogic _TheLoaiSachLogic = new TheLoaiSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            KeSachLogic _keSachLogic = new KeSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            NhaXuatBanLogic _NhaXuatBanLogic = new NhaXuatBanLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            LanguageLogic _LanguageLogic = new LanguageLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            SachTacGiaLogic _SachTacGiaLogic = new SachTacGiaLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            TacGiaLogic _TacGiaLogic = new TacGiaLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);


            ExcelManager excelManager = new ExcelManager();
            List<Sach> listExcel = new List<Sach>();
            if (model.LinkExcel != null)
            {
                string uploadForder = GetUploadFolder(Helpers.UploadFolder.FileExcel);
                string physicalWebRootPath = Server.MapPath("/");

                var sourceFileName = Path.Combine(physicalWebRootPath, uploadForder, model.LinkExcel.FileName);
                string location = Path.GetDirectoryName(sourceFileName);
                if (!Directory.Exists(location))
                {
                    Directory.CreateDirectory(location);
                }
                using (FileStream fileStream = new FileStream(sourceFileName, FileMode.Create))
                {
                    model.LinkExcel.InputStream.CopyTo(fileStream);
                    var sourceDir = fileStream.Name.Replace(physicalWebRootPath, "/").Replace(@"\", @"/").Replace(@"//", @"/");
                    listExcel = excelManager.ImportSach(sourceDir);
                }

                foreach (var item in listExcel)
                {
                    #region Linh tinh
                    // Thể loại sách
                    string itemTheLoai = item.IdTheLoai.Trim();
                    var machs = System.Text.RegularExpressions.Regex.Match(itemTheLoai, @"^\d{3}$");

                    if (machs.Length > 0)
                    {
                        var theloai = _TheLoaiSachLogic.GetIdByDDC(itemTheLoai);
                        if (theloai != null)
                        {
                            item.IdTheLoai = theloai.Id;
                        }
                        else
                        {
                            // todo - ddc dictionary version 21
                            var id = _TheLoaiSachLogic.ThemTheLoaiSach(new TheLoaiSach()
                            {
                                TenTheLoai = item.IdTheLoai.Trim(),
                                MaDDC = itemTheLoai.Trim()
                            });
                            item.IdTheLoai = id;
                        }
                    }
                    else
                    {
                        var theloai = _TheLoaiSachLogic.GetByTenTheLoai(itemTheLoai);
                        if (theloai != null)
                        {
                            item.IdTheLoai = theloai.Id;
                        }
                        else
                        {
                            var id = _TheLoaiSachLogic.ThemTheLoaiSach(new TheLoaiSach() { TenTheLoai = item.IdTheLoai });
                            item.IdTheLoai = id;
                        }
                    }

                    // Kệ sách
                    var keSach = _keSachLogic.GetByTenKeSach(item.IdKeSach);
                    if (keSach != null)
                    {
                        item.IdKeSach = keSach.Id;
                    }
                    else
                    {
                        var id = _keSachLogic.Add(new KeSach() { TenKe = item.IdKeSach.Trim() });
                        item.IdKeSach = id;
                    }

                    // Nhà xuất bản
                    var nxb = _NhaXuatBanLogic.GetByTenNXB(item.IdNhaXuatBan);
                    if (nxb != null)
                    {
                        item.IdNhaXuatBan = nxb.Id;
                    }
                    else
                    {
                        var id = _NhaXuatBanLogic.ThemNXB(new NhaXuatBan() { Ten = item.IdNhaXuatBan.Trim() });
                        item.IdNhaXuatBan = id;
                    }

                    // Ngôn ngữ
                    var ngonNgu = _LanguageLogic.GetByTenNgonNgu(item.IdNgonNgu);
                    if (ngonNgu != null)
                    {
                        item.IdNgonNgu = ngonNgu.Id;
                    }
                    else
                    {
                        var id = _LanguageLogic.InsertNew(new Language() { Ten = item.IdNgonNgu.Trim() });
                        item.IdNgonNgu = id;
                    }
                    #endregion

                    var idSach = _SachLogic.ThemSach(item);
                    if (idSach.Length > 0)
                    {
                        // Tác giả 
                        foreach (var tg in item.listTacGia)
                        {
                            string idTG = null;
                            var tacGia = _TacGiaLogic.GetByTenTacGia(tg.TenTacGia);
                            if (tacGia != null)
                            {
                                idTG = tacGia.Id;
                            }
                            else
                            {
                                idTG = _TacGiaLogic.Insert(tg);
                            }
                            _SachTacGiaLogic.ThemSachTacGia(new SachTacGia() { IdTacGia = idTG, IdSach = idSach });
                        }
                        Sach sach = _SachLogic.GetBookById(idSach);
                        Sach temp = sachCommon.LuuMaVachSach(physicalWebRootPath, sach, null);
                        if (temp != null)
                        {
                            sach.QRlink = temp.QRlink;
                            sach.QRData = temp.QRData;
                            _SachLogic.Update(sach);
                        }
                    }
                }
            }
            return RedirectToAction("Index", "Sach");
            // return View();
        }
    }
}