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
    public class PhieuXuatSachController : BaseController
    {
        // GET: PhieuXuatSach
        public ActionResult Index(int? page)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            PhieuXuatSachLogic _PhieuXuatSachLogic = new PhieuXuatSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            int PageSize = 10;
            int PageNumber = (page ?? 1);
            var lst = _PhieuXuatSachLogic.Getall();
            List<PhieuXuatSachModels> lstpxs = new List<PhieuXuatSachModels>();
            foreach (var item in lst)
            {
                PhieuXuatSachModels pxs = new PhieuXuatSachModels()
                {
                    IdPhieuXuat = item.Id,
                    IdUserAdmin = item.IdUserAdmin,
                    GhiChu = item.GhiChu,
                    NgayXuat = item.CreateDateTime,
                    UserName = item.UserName
                };
                lstpxs.Add(pxs);
            }
            ViewBag.pageSize = PageSize;
            ViewBag.pages = PageNumber;
            return View(lstpxs.OrderByDescending(x => x.NgayXuat).ToPagedList(PageNumber, PageSize));
        }

        public ActionResult Details(string id)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            ChiTietXuatSachLogic _ChiTietXuatSachLogic = new ChiTietXuatSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            TrangThaiSachLogic _TrangThaiSachLogic = new TrangThaiSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            SachLogic _SachLogic =
              new SachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            LyDoXuatLogic _LyDoXuatLogic = new LyDoXuatLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            PhieuXuatSachLogic _PhieuXuatSachLogic = new PhieuXuatSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

            var model = _ChiTietXuatSachLogic.GetAllChiTietById(id);
            var phieuxuat = _PhieuXuatSachLogic.GetById(id);

            if (phieuxuat == null || model == null)
                return RedirectToAction("NotFound", "Error");

            PhieuXuatSachModels pxs = new PhieuXuatSachModels()
            {
                IdUserAdmin = phieuxuat.IdUserAdmin,
                UserName = phieuxuat.UserName,
                GhiChu = phieuxuat.GhiChu,
                NgayXuat = phieuxuat.CreateDateTime

            };

            List<ChiTietXuatSachViewModels> lst = new List<ChiTietXuatSachViewModels>();
            foreach (var item in model)
            {
                ChiTietXuatSachViewModels ctxs = new ChiTietXuatSachViewModels();
                ctxs.Id = item.Id;
                //var LyDo = _LyDoXuatLogic.GetById(ctxs.IdLydo);
                ctxs.GhiChuDon = item.GhiChu;
                ctxs.IdTinhTrang = item.IdTinhTrang;
                var TinhTrang = _TrangThaiSachLogic.getById(ctxs.IdTinhTrang);
                ctxs.tenTinhTrang = TinhTrang.TenTT;
                ctxs.IdSach = item.IdSach;
                var TenSach = _SachLogic.GetBookById(ctxs.IdSach);
                ctxs.ten = TenSach.TenSach;
                ctxs.IdPhieuXuat = item.IdPhieuXuat;
                ctxs.soLuong = item.SoLuong;
                lst.Add(ctxs);

            }
            ViewBag.lstctxuat = lst;
            return View(pxs);
        }

        public ActionResult TaoPhieuXuatSach()
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            TrangThaiSachLogic _TrangThaiSachLogic = new TrangThaiSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            LyDoXuatLogic _LyDoXuatLogic = new LyDoXuatLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

            ViewBag.listtt = _TrangThaiSachLogic.GetAll();
            ViewBag.listld = _LyDoXuatLogic.GetAll();
            return View();
        }

        [HttpPost]
        public ActionResult TaoPhieuXuatSach(PhieuXuatSachModels model)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            SachLogic _SachLogic =
               new SachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            PhieuXuatSachLogic _PhieuNhapSachLogic = new PhieuXuatSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            ChiTietXuatSachLogic _ChiTietNhapSachLogic = new ChiTietXuatSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            TrangThaiSachLogic _TrangThaiSachLogic = new TrangThaiSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            SoLuongSachTrangThaiLogic _SoLuongSachTrangThaiLogic =
             new SoLuongSachTrangThaiLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            LyDoXuatLogic _LyDoXuatLogic =
            new LyDoXuatLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            PhieuXuatSachLogic _PhieuXuatSachLogic =
                new PhieuXuatSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

            if (model.listChiTietJsonString.Count > 0)
            {
                PhieuXuatSach pxs = new PhieuXuatSach()
                {
                    GhiChu = model.GhiChu,
                    IdUserAdmin = userdata.Id,
                    UserName = userdata.UserName
                };
                string idPhieuXuat = _PhieuNhapSachLogic.XuatSach(pxs);

                if (!String.IsNullOrEmpty(idPhieuXuat))
                {
                    foreach (var json in model.listChiTietJsonString)
                    {
                        var ctModel = Newtonsoft.Json.JsonConvert.DeserializeObject<ChiTietXuatSachViewModels>(json);

                        var ctxs = new ChiTietXuatSach()
                        {
                            IdPhieuXuat = idPhieuXuat,
                            IdSach = ctModel.IdSach,
                            IdTinhTrang = ctModel.IdTinhTrang,
                            SoLuong = ctModel.soLuong,
                            GhiChu = ctModel.GhiChuDon,
                            CreateDateTime = DateTime.Now
                        };

                        _ChiTietNhapSachLogic.Insert(ctxs);
                        {
                            //update số lượng sách trạng thái
                            var sltt = _SoLuongSachTrangThaiLogic.getBy_IdSach_IdTT(ctxs.IdSach, ctxs.IdTinhTrang);
                            if (sltt != null)
                            {
                                sltt.SoLuong -= ctxs.SoLuong;
                                _SoLuongSachTrangThaiLogic.Update(sltt);
                            }
                            else
                            {
                                sltt = new SoLuongSachTrangThai();
                                sltt.IdSach = ctxs.IdSach;
                                sltt.IdTrangThai = ctxs.IdTinhTrang;
                                sltt.SoLuong = ctxs.SoLuong;
                                _SoLuongSachTrangThaiLogic.Insert(sltt);
                            }
                            //update tổng số lượng sách
                            var updatesl = _SachLogic.GetBookById(sltt.IdSach);
                            updatesl.SoLuongTong -= ctxs.SoLuong;
                            updatesl.SoLuongConLai -= ctxs.SoLuong;
                            _SachLogic.Update(updatesl);

                        }
                    }
                    return RedirectToAction("Index");
                }
            }

            ViewBag.listtt = _TrangThaiSachLogic.GetAll();
            ViewBag.listld = _LyDoXuatLogic.GetAll();
            ModelState.Clear();
            return View();
        }

        [HttpGet]
        public JsonResult _GetBookItemById(string maKiemSoat, int soLuong, string idtrangthai, string ghiChuDon)
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
            LyDoXuatLogic _LyDoXuat =
               new LyDoXuatLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

            maKiemSoat = maKiemSoat.Trim();
            ghiChuDon = ghiChuDon.Trim();

            JsonResult result = new JsonResult();
            result.Data = null;
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            if (!string.IsNullOrEmpty(maKiemSoat) && !string.IsNullOrEmpty(ghiChuDon)
                && soLuong > 0 && !string.IsNullOrEmpty(idtrangthai))
            {
                var book = _SachLogic.GetByMaMaKiemSoat(maKiemSoat);
                var tt = _TrangThaiSachLogic.getById(idtrangthai);

                ChiTietXuatSachViewModels pp = new ChiTietXuatSachViewModels()
                {
                    IdSach = book.Id,
                    ten = book.TenSach,
                    soLuong = soLuong,
                    IdTinhTrang = idtrangthai,
                    tenTinhTrang = tt.TenTT,
                    GhiChuDon = ghiChuDon,
                    MaKiemSoat = book.MaKiemSoat
                };

                result.Data = pp;
            }
            return result;
        }
    }
}