using BiTech.Library.BLL.DBLogic;
using BiTech.Library.BLL.BarCode_QR;
using BiTech.Library.Models;
using BiTech.Library.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using BiTech.Library.Controllers.BaseClass;
using static BiTech.Library.Helpers.Tool;
using System.IO;

namespace BiTech.Library.Controllers
{
    public class PhieuNhapSachController : BaseController
    {
        public ActionResult Index(int? page)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            PhieuNhapSachLogic _PhieuNhapSachLogic = new PhieuNhapSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            int PageSize = 10;
            int PageNumber = (page ?? 1);
            var lst = _PhieuNhapSachLogic.Getall();
            List<PhieuNhapSachModels> lstpns = new List<PhieuNhapSachModels>();
            foreach (var item in lst)
            {
                PhieuNhapSachModels pns = new PhieuNhapSachModels()
                {
                    IdPhieuNhap = item.Id,
                    IdUserAdmin = item.IdUserAdmin,
                    GhiChu = item.GhiChu,
                    NgayNhap = item.NgayNhap,
                    UserName = item.UserName

                };
                lstpns.Add(pns);
            }

            ViewBag.pageSize = PageSize;
            ViewBag.pages = PageNumber;
            return View(lstpns.OrderByDescending(x => x.NgayNhap).ToPagedList(PageNumber, PageSize));
        }

        public ActionResult NhapSach()
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            PhieuNhapSachLogic _NhapSachLogic = new PhieuNhapSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            SachLogic _SachLogic = new SachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

            var list = _SachLogic.getAllSach();

            List<SachViewModels> ListdsSach = new List<SachViewModels>();
            foreach (var item in list)
            {
                SachViewModels model = new SachViewModels
                {
                    Id = item.Id,
                    TenSach = item.TenSach,
                    IdTheLoai = item.IdTheLoai,
                    IdKeSach = item.IdKeSach,
                    IdNhaXuatBan = item.IdNhaXuatBan,
                    MaKiemSoat = item.MaKiemSoat,
                    NgonNgu = item.IdNgonNgu,
                    NamSanXuat = item.NamXuatBan,
                    GiaBia = item.GiaBia,
                    LinkBiaSach = item.LinkBiaSach,
                    SoLuong = item.SoLuongTong
                };
                ListdsSach.Add(model);
            }
            ViewBag.DSSach = ListdsSach;
            return View();
        }

        [HttpPost]
        public ActionResult NhapSach(ChiTietNhapSachViewModels model)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            PhieuNhapSachLogic _NhapSachLogic = new PhieuNhapSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

            PhieuNhapSach NS = new PhieuNhapSach()
            {
                Id = model.Id,

            };
            _NhapSachLogic.NhapSach(NS);
            return View();
        }

        public ActionResult TaoPhieuNhapSach()
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            TrangThaiSachLogic _TrangThaiSachLogic = new TrangThaiSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

            ViewBag.listtt = _TrangThaiSachLogic.GetAll();
            return View();
        }

        [HttpPost]
        public ActionResult TaoPhieuNhapSach(PhieuNhapSachModels model)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            SachLogic _SachLogic = new SachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            PhieuNhapSachLogic _PhieuNhapSachLogic = new PhieuNhapSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            ChiTietNhapSachLogic _ChiTietNhapSachLogic = new ChiTietNhapSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            TrangThaiSachLogic _TrangThaiSachLogic = new TrangThaiSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            SoLuongSachTrangThaiLogic _SoLuongSachTrangThaiLogic = new SoLuongSachTrangThaiLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

            if (model.listChiTietJsonString.Count > 0)
            {
                PhieuNhapSach pns = new PhieuNhapSach()
                {
                    NgayNhap = DateTime.Now,
                    GhiChu = model.GhiChu,
                    IdUserAdmin = userdata.Id,
                    UserName = userdata.UserName
                };

                string idPhieuNhap = _PhieuNhapSachLogic.NhapSach(pns);

                if (!String.IsNullOrEmpty(idPhieuNhap))
                {
                    foreach (var json in model.listChiTietJsonString)
                    {
                        var ctModel = Newtonsoft.Json.JsonConvert.DeserializeObject<ChiTietNhapSachViewModels>(json);

                        var ctns = new ChiTietNhapSach()
                        {
                            IdPhieuNhap = idPhieuNhap,
                            IdSach = ctModel.IdSach,
                            //tenTinhTrang = ctModel.tenTinhTrang,
                            SoLuong = ctModel.soLuong,
                            CreateDateTime = DateTime.Now,
                            IdTinhtrang = ctModel.IdTinhTrang,
                            GhiChu = ctModel.GhiChu
                        };

                        _ChiTietNhapSachLogic.Insert(ctns);
                        {
                            var sltt = _SoLuongSachTrangThaiLogic.getBy_IdSach_IdTT(ctns.IdSach, ctModel.IdTinhTrang);
                            if (sltt != null)
                            {
                                sltt.SoLuong += ctns.SoLuong;
                                _SoLuongSachTrangThaiLogic.Update(sltt);
                            }
                            else
                            {
                                sltt = new SoLuongSachTrangThai();
                                sltt.IdSach = ctns.IdSach;
                                sltt.IdTrangThai = ctModel.IdTinhTrang;
                                sltt.SoLuong = ctns.SoLuong;
                                _SoLuongSachTrangThaiLogic.Insert(sltt);
                            }

                            var updatesl = _SachLogic.GetBookById(sltt.IdSach);
                            updatesl.SoLuongTong += ctns.SoLuong;
                            // ktr neu cho muon moi cong them
                            var tinhTrang = _TrangThaiSachLogic.getById(ctModel.IdTinhTrang);
                            if (tinhTrang.TrangThai == true)
                                updatesl.SoLuongConLai += ctns.SoLuong;
                            _SachLogic.Update(updatesl);
                        }
                    }
                    return RedirectToAction("Index");
                }
            }

            ViewBag.listtt = _TrangThaiSachLogic.GetAll();
            ModelState.Clear();

            return View();
        }

        public ActionResult Details(string id)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            ChiTietNhapSachLogic _ChiTietNhapSachLogic = new ChiTietNhapSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            TrangThaiSachLogic _TrangThaiSachLogic = new TrangThaiSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            SachLogic _SachLogic =
              new SachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            PhieuNhapSachLogic _PhieuNhapSachLogic =
             new PhieuNhapSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

            var model = _ChiTietNhapSachLogic.GetAllChiTietById(id);
            var phieunhap = _PhieuNhapSachLogic.GetById(id);

            if (phieunhap == null || model == null)
                return RedirectToAction("NotFound", "Error");

            PhieuNhapSachModels pns = new PhieuNhapSachModels()
            {

                IdUserAdmin = phieunhap.IdUserAdmin,
                GhiChu = phieunhap.GhiChu,
                NgayNhap = phieunhap.NgayNhap

            };
            List<ChiTietNhapSachViewModels> lst = new List<ChiTietNhapSachViewModels>();
            foreach (var item in model)
            {

                ChiTietNhapSachViewModels ctns = new ChiTietNhapSachViewModels();
                ctns.Id = item.Id;
                ctns.IdTinhTrang = item.IdTinhtrang;
                var TinhTrang = _TrangThaiSachLogic.getById(ctns.IdTinhTrang);
                ctns.tenTinhTrang = TinhTrang.TenTT;
                ctns.IdSach = item.IdSach;
                var TenSach = _SachLogic.GetBookById(ctns.IdSach);
                ctns.ten = TenSach.TenSach;
                //ctns.tenTinhTrang = item.IdPhieuNhap;
                ctns.soLuong = item.SoLuong;
                //ctns.tenTinhTrang = item.tenTinhTrang;
                lst.Add(ctns);

            }
            ViewBag.lstctnhap = lst;

            return View(pns);
        }
        [HttpGet]
        public JsonResult _GetBookItemById(string maKS, int soLuong, string idtrangthai, string GhiChu)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return Json(null, JsonRequestBehavior.AllowGet); //RedirectToAction("LogOff", "Account");
            #endregion
            SachLogic _SachLogic =
                new SachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            TrangThaiSachLogic _TrangThaiSachLogic =
                new TrangThaiSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

            JsonResult result = new JsonResult();
            if (!String.IsNullOrWhiteSpace(maKS))
            {
                var book = _SachLogic.GetByMaMaKiemSoat(maKS);
                var tt = _TrangThaiSachLogic.getById(idtrangthai);
                ChiTietNhapSachViewModels pp = new ChiTietNhapSachViewModels()
                {
                    IdSach = book.Id,
                    ten = book.TenSach,
                    soLuong = soLuong,
                    IdTinhTrang = idtrangthai,
                    //tenTinhTrang = tt.TenTT,
                    MaKiemSoat = book.MaKiemSoat,
                    GhiChu = GhiChu
                };

                result.Data = pp;
                //lstSach.Add(idBook); // add item to list to get list idSach - inset chitietphieumuon
                result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            }
            return result;
        }

        //code	   
        [HttpPost]
        public ActionResult Autocomplete(string a)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return Json(null, JsonRequestBehavior.AllowGet); //RedirectToAction("LogOff", "Account");
            #endregion
            SachLogic _SachLogic =
               new SachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

            var ListTD = (from N in _SachLogic.getAll()
                          where N.MaKiemSoat.StartsWith(a)
                          select new { N.MaKiemSoat });


            return Json(ListTD, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ImportFromExcel()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ImportFromExcel(PhieuNhapSachModels model)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion
            PhieuNhapSachLogic _PhieuNhapSachLogic = new PhieuNhapSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            ChiTietNhapSachLogic _ChiTietNhapSachLogic = new ChiTietNhapSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            SachLogic _SachLogic = new SachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            TrangThaiSachLogic _TrangThaiSachLogic = new TrangThaiSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            SoLuongSachTrangThaiLogic _SoLuongSachTrangThaiLogic = new SoLuongSachTrangThaiLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            ExcelManager excelManager = new ExcelManager();
            List<ChiTietNhapSach> list = new List<ChiTietNhapSach>();
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
                    list = excelManager.ImportPhieuNhapSach(sourceDir);
                }
                PhieuNhapSach pns = new PhieuNhapSach()
                {
                    NgayNhap = DateTime.Now,
                    GhiChu = model.GhiChu,
                    IdUserAdmin = userdata.Id,
                    UserName = userdata.UserName
                };
                string idPhieuNhap = _PhieuNhapSachLogic.NhapSach(pns);
                var listAllTTS = _TrangThaiSachLogic.GetAll();

                foreach (var item in list)
                {
                    var sach = _SachLogic.GetByMaMaKiemSoat(item.IdSach);
                    int index = Int32.Parse(item.IdTinhtrang);
                    TrangThaiSach trangThaiSach = null;
                    if (index <= listAllTTS.Count())
                        trangThaiSach = listAllTTS[index - 1];
                    if (trangThaiSach != null && sach != null)
                    {
                        item.IdSach = sach.Id;
                        item.IdPhieuNhap = idPhieuNhap;
                        item.IdTinhtrang = trangThaiSach.Id;

                        _ChiTietNhapSachLogic.Insert(item);
                        //todo
                        {
                            var sltt = _SoLuongSachTrangThaiLogic.getBy_IdSach_IdTT(sach.Id, trangThaiSach.Id);
                            if (sltt != null)
                            {
                                sltt.SoLuong += item.SoLuong;
                                _SoLuongSachTrangThaiLogic.Update(sltt);
                            }
                            else
                            {
                                sltt = new SoLuongSachTrangThai();
                                sltt.IdSach = sach.Id;
                                sltt.IdTrangThai = trangThaiSach.Id;
                                sltt.SoLuong = item.SoLuong;
                                _SoLuongSachTrangThaiLogic.Insert(sltt);
                            }

                            var updateSach = _SachLogic.GetBookById(sltt.IdSach);
                            updateSach.SoLuongTong += item.SoLuong;
                            if (trangThaiSach.TrangThai == true)
                                updateSach.SoLuongConLai += item.SoLuong;
                            _SachLogic.Update(updateSach);
                        }
                    }
                }
            }
            //return View();
            return RedirectToAction("Index", "PhieuNhapSach");
        }


    }

}