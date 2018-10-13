using BiTech.Library.BLL.DBLogic;
using BiTech.Library.Controllers.BaseClass;
using BiTech.Library.DTO;
using BiTech.Library.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BiTech.Library.Helpers;

namespace BiTech.Library.Controllers
{
    [AuthorizeRoles(true, Role.CustomerAdmin, Role.CustomerUser)]
#if DEBUG
    [AllowAnonymous]
#endif
    public class PhieuXuatSachController : BaseController
    {
        // GET: PhieuXuatSach
        public ActionResult Index(int? page)
        {
            PhieuXuatSachLogic _PhieuXuatSachLogic = new PhieuXuatSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            int PageSize = 20;
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
            ViewBag.number = lst.Count;
            return View(lstpxs.OrderByDescending(x => x.NgayXuat).ToPagedList(PageNumber, PageSize));
        }

        public ActionResult Details(string id)
        {
            ChiTietXuatSachLogic _ChiTietXuatSachLogic = new ChiTietXuatSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            TrangThaiSachLogic _TrangThaiSachLogic = new TrangThaiSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            SachLogic _SachLogic = new SachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            PhieuXuatSachLogic _PhieuXuatSachLogic = new PhieuXuatSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);

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
                ctxs.IdTinhTrang = item.IdTinhTrang;
                var TinhTrang = _TrangThaiSachLogic.getById(ctxs.IdTinhTrang);
                ctxs.tenTinhTrang = TinhTrang.TenTT;
                ctxs.IdSach = item.IdSach;
                var TenSach = _SachLogic.GetBookById(ctxs.IdSach);
                ctxs.ten = TenSach.TenSach;
                ctxs.IdPhieuXuat = item.IdPhieuXuat;
                ctxs.MaKiemSoat = _SachLogic.GetById(item.IdSach).MaKiemSoat;
                ctxs.MaCaBiet = item.MaCaBiet;
                lst.Add(ctxs);

            }
            ViewBag.lstctxuat = lst;
            return View(pxs);
        }

        public ActionResult TaoPhieuXuatSach()
        {
            TrangThaiSachLogic _TrangThaiSachLogic = new TrangThaiSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            ViewBag.listtt = _TrangThaiSachLogic.GetAll();
            return View();
        }

        [HttpPost]
        public ActionResult TaoPhieuXuatSach(PhieuXuatSachModels model)
        {
            var _SachLogic = new SachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            var _ChiTietNhapSachLogic = new ChiTietXuatSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            var _TrangThaiSachLogic = new TrangThaiSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            var _PhieuXuatSachLogic = new PhieuXuatSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            var _SachCaBietLogic = new SachCaBietLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);

            if (model.listChiTietJsonString.Count > 0)
            {
                PhieuXuatSach pxs = new PhieuXuatSach()
                {
                    GhiChu = model.GhiChu,
                    IdUserAdmin = _UserAccessInfo.Id,
                    UserName = _UserAccessInfo.UserName
                };
                string idPhieuXuat = _PhieuXuatSachLogic.XuatSach(pxs);
                SachCaBiet scDTO = new SachCaBiet();
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
                            MaCaBiet = ctModel.MaCaBiet,
                            CreateDateTime = DateTime.Now
                        };
                        _ChiTietNhapSachLogic.Insert(ctxs);
                        ;
                        _SachCaBietLogic.Remove(_SachCaBietLogic.GetByMaKSCBorMaCaBienCu(ctModel.MaCaBiet).Id);

                        //Update bảng Sach 
                        //Cập nhật số lượng cho mỗi đầu sách  
                        var modelSach = _SachLogic.GetByID_IsDeleteFalse(ctModel.IdSach);
                        modelSach.SoLuongTong--;
                        modelSach.SoLuongConLai--;
                        _SachLogic.Update(modelSach);
                    }
                    return RedirectToAction("Index");
                }
            }
            ViewBag.listtt = _TrangThaiSachLogic.GetAll();
            ModelState.Clear();
            return View();
        }

        public JsonResult AddBookToList(string maCaBiet)
        {
            var _SachCaBietLogic = new SachCaBietLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            var _SachLogic = new SachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            var _TrangThaiLogic = new TrangThaiSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            JsonResult result = new JsonResult();
            if (maCaBiet.Length > 25 || maCaBiet.Contains("BLibBook"))
            {
                //get ma kiem soat tu QR
                SachCommon s = new SachCommon();
                maCaBiet = s.GetInfo(maCaBiet);
            }
            var cabiet = _SachCaBietLogic.GetByMaKSCBorMaCaBienCu(maCaBiet);
            if (cabiet != null)
            {
                var book = _SachLogic.GetByID_IsDeleteFalse(cabiet.IdSach);
                result.Data = null;
                result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                var tt = _TrangThaiLogic.getById(cabiet.IdTrangThai);
                ChiTietXuatModels cnpx = new ChiTietXuatModels()
                {
                    Id = book.Id,
                    IdSach = cabiet.IdSach,
                    IdTinhTrang = tt.Id,
                    TenSach = book.TenSach,
                    MaCaBiet = maCaBiet,
                    MaKiemSoat = book.MaKiemSoat,
                    TrangThai = tt.TenTT
                };
                result.Data = cnpx;
            }
            return result;
        }

        /// <summary>
        /// Add vào list queue - maSach( MKS || maQR || ISBN)
        /// </summary>
        /// <param name="maSach"></param>
        /// <returns></returns>
        public JsonResult AddBookToListQueue(string maSach)
        {
            JsonResult result = new JsonResult();
            if (!string.IsNullOrEmpty(maSach))
            {
                var _SachLogic = new SachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
                var _TrangThaiLogic = new TrangThaiSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
                if (maSach.Contains("BLibBook")) //QR
                {
                    //get ma kiem soat tu QR
                    Sach _sach = _SachLogic.GetByMaKiemSoatorISBN(new SachCommon().GetInfo(maSach));
                    maSach = _sach.MaKiemSoat;
                }
                var idDauSach = _SachLogic.GetByMaKiemSoatorISBN(maSach);
                if (idDauSach != null)
                {
                    //Lay danh sách sách cá biệt từ IdDauSach
                    var _SachCaBietLogic = new SachCaBietLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
                    var lstSachCB = _SachCaBietLogic.GetListCaBietFromIdSach(idDauSach.Id);
                    result.Data = null;
                    result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                    List<ChiTietXuatModels> lstCtxs = new List<ChiTietXuatModels>();

                    foreach (var item in lstSachCB)
                    {
                        var tt = _TrangThaiLogic.getById(item.IdTrangThai);
                        ChiTietXuatModels cnpxQueue = new ChiTietXuatModels()
                        {
                            IdSach = item.IdSach,
                            IdTinhTrang = tt.Id,
                            TenSach = idDauSach.TenSach,
                            TrangThai = tt.TenTT
                        };
                        if (item.MaKSCB == "" && item.MaCaBienCu != "")
                            cnpxQueue.MaCaBiet = item.MaCaBienCu;
                        else
                            cnpxQueue.MaCaBiet = item.MaKSCB;
                        lstCtxs.Add(cnpxQueue);
                    }

                    result.Data = lstCtxs;
                }
            }
            return result;
        }
    }
}