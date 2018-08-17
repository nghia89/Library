using BiTech.Library.BLL.DBLogic;
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

namespace BiTech.Library.Controllers
{
    public class SachController : BaseController
    {
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


            ListBooksModel model = new ListBooksModel();

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            ViewBag.theLoaiSach = _TheLoaiSachLogic.GetAllTheLoaiSach();
            ViewBag.tacGia = _TacGiaLogic.GetAllTacGia();
            ViewBag.NXB = _NhaXuatBanLogic.GetAllNhaXuatBan();

            var list = _SachLogic.getPageSach(KeySearch);
            foreach (var item in list)
            {
                BookView book = new BookView(item);

                book.Ten_TheLoai = _TheLoaiSachLogic.getById(item.IdTheLoai)?.TenTheLoai ?? "--";
                book.Ten_NhaXuatBan = _NhaXuatBanLogic.getById(item.IdNhaXuatBan)?.Ten ?? "--";
                book.Ten_KeSach = _KeSachLogic.getById(item.IdKeSach)?.TenKe ?? "--";
                book.Ten_NgonNgu = _LanguageLogic.GetById(item.IdNgonNgu)?.Ten ?? "--";

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

                    if (failTG.Length > 0)
                    {
                        failTG = failTG.Substring(0, failTG.Length - 2);
                        TempData["ThemSachMsg"] = string.Format("Chú ý: Chọn tác giả {0} thất bại, vui lòng cập nhật sau.", failTG);
                    }

                    return RedirectToAction("Index");
                }
                TempData["ThemSachMsg"] = "Thêm sách thất bại";
            }


            LanguageLogic _LanguageLogic = new LanguageLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
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
                Sach sach = new Sach()
                {
                    Id = model.SachDTO.Id,
                    IdTheLoai = model.SachDTO.IdTheLoai,
                    IdKeSach = model.SachDTO.IdKeSach,
                    IdNhaXuatBan = model.SachDTO.IdNhaXuatBan,
                    MaKiemSoat = model.SachDTO.MaKiemSoat,
                    SoLuongTong = model.SachDTO.SoLuongTong,
                    SoTrang = model.SachDTO.SoTrang,
                    IdNgonNgu = model.SachDTO.IdNgonNgu,
                    NamXuatBan = model.SachDTO.NamXuatBan,
                    GiaBia = model.SachDTO.GiaBia,
                    TaiBan = model.SachDTO.TaiBan,
                    TenSach = model.SachDTO.TenSach,
                    TomTat = model.SachDTO.TomTat,
                    PhiMuonSach = model.SachDTO.PhiMuonSach,
                    XuatXu = model.SachDTO.XuatXu,
                    NguoiBienDich = model.SachDTO.NguoiBienDich,
                    DDC=model.SachDTO.DDC,
                    ISBN=model.SachDTO.ISBN

                    //LinkBiaSach = model.FileImageCover.ToString()
                };
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
    }
}