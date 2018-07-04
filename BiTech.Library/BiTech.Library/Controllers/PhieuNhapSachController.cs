using BiTech.Library.BLL.DBLogic;
using BiTech.Library.Models;
using BiTech.Library.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;

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
                    NgayNhap = item.NgayNhap

                };
                lstpns.Add(pns);
            }

           
            return View(lstpns.OrderByDescending(x=>x.NgayNhap).ToPagedList(PageNumber,PageSize));
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
            SachLogic _SachLogic =
               new SachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            PhieuNhapSachLogic _PhieuNhapSachLogic = new PhieuNhapSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            ChiTietNhapSachLogic _ChiTietNhapSachLogic = new ChiTietNhapSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            TrangThaiSachLogic _TrangThaiSachLogic = new TrangThaiSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            SoLuongSachTrangThaiLogic _SoLuongSachTrangThaiLogic =
             new SoLuongSachTrangThaiLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);


            if (model.listChiTietJsonString.Count > 0)
            {
                PhieuNhapSach pns = new PhieuNhapSach()
                {
                    NgayNhap = DateTime.Now,
                    GhiChu = model.GhiChu,
                    IdUserAdmin = "",

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
                            IdTinhtrang = ctModel.IdTinhTrang,
                            soLuong = ctModel.soLuong,
                            CreateDateTime = DateTime.Now
                        };

                        _ChiTietNhapSachLogic.Insert(ctns);
                        {
                            var sltt = _SoLuongSachTrangThaiLogic.getBy_IdSach_IdTT(ctns.IdSach, ctns.IdTinhtrang);
                            if (sltt != null)
                            {
                                sltt.SoLuong += ctns.soLuong;
                                _SoLuongSachTrangThaiLogic.Update(sltt);
                            }
                            else
                            {
                                sltt = new SoLuongSachTrangThai();
                                sltt.IdSach = ctns.IdSach;
                                sltt.IdTrangThai = ctns.IdTinhtrang;
                                sltt.SoLuong = ctns.soLuong;
                                _SoLuongSachTrangThaiLogic.Insert(sltt);
                            }

                            var updatesl = _SachLogic.GetBookById(sltt.IdSach);
                            updatesl.SoLuong += ctns.soLuong;
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
                ctns.IdDauSach = TenSach.IdDauSach;
                ctns.IdPhieuNhap = item.IdPhieuNhap;
                ctns.soLuong = item.soLuong;

                lst.Add(ctns);

            }
            ViewBag.lstctnhap = lst;

            return View(pns);
        }
        [HttpGet]
        public JsonResult _GetBookItemById(string idBook, int soLuong, string idtrangthai)
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
            if (!String.IsNullOrWhiteSpace(idBook))
            {
                var book = _SachLogic.GetByIdBook(idBook);
                var tt = _TrangThaiSachLogic.getById(idtrangthai);
                ChiTietNhapSachViewModels pp = new ChiTietNhapSachViewModels();
                {
                    pp.IdSach = book.Id;
                    pp.IdDauSach = book.IdDauSach;
                    pp.ten = book.TenSach;
                    pp.soLuong = soLuong;
                    pp.IdTinhTrang = idtrangthai;
                    pp.tenTinhTrang = tt.TenTT;
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

            var ListTD = (from N in _SachLogic.getAllSach()
                          where N.IdDauSach.StartsWith(a)
                          select new { N.IdDauSach });
           

            return Json(ListTD, JsonRequestBehavior.AllowGet);
        }
    } 

}