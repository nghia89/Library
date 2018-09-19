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
    public class PhieuXuatSachController : BaseController
    {
        // GET: PhieuXuatSach
        public ActionResult Index(int? page)
        {
            PhieuXuatSachLogic _PhieuXuatSachLogic = new PhieuXuatSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
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
            ChiTietXuatSachLogic _ChiTietXuatSachLogic = new ChiTietXuatSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            TrangThaiSachLogic _TrangThaiSachLogic = new TrangThaiSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            SachLogic _SachLogic =
              new SachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
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
                ctxs.MaKiemSoat = _SachLogic.GetById(item.IdSach).MaKiemSoat;
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
            var _PhieuNhapSachLogic = new PhieuXuatSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            var _ChiTietNhapSachLogic = new ChiTietXuatSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            var _TrangThaiSachLogic = new TrangThaiSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            var _SoLuongSachTrangThaiLogic = new SoLuongSachTrangThaiLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            var _PhieuXuatSachLogic = new PhieuXuatSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);

            if (model.listChiTietJsonString.Count > 0)
            {
                PhieuXuatSach pxs = new PhieuXuatSach()
                {
                    GhiChu = model.GhiChu,
                    IdUserAdmin = _UserAccessInfo.Id,
                    UserName = _UserAccessInfo.UserName
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

                        // update số lượng sách trạng thái
                        var sltt = _SoLuongSachTrangThaiLogic.getBy_IdSach_IdTT(ctxs.IdSach, ctxs.IdTinhTrang);

                        // Nếu có và sl hiện tại lớn hơn hoặc = sl xuất
                        if (sltt != null && sltt.SoLuong >= ctxs.SoLuong)
                        {
                            sltt.SoLuong -= ctxs.SoLuong;
                            _SoLuongSachTrangThaiLogic.Update(sltt);

                            _ChiTietNhapSachLogic.Insert(ctxs);

                            // update tổng số lượng sách
                            var updatesl = _SachLogic.GetBookById(sltt.IdSach);
                            updatesl.SoLuongTong -= ctxs.SoLuong;

                            // ktr neu cho muon moi cong them
                            //var tinhTrang = _TrangThaiSachLogic.getById(ctModel.IdTinhTrang);
                            //if (tinhTrang.TrangThai == true)
                            updatesl.SoLuongConLai -= ctxs.SoLuong;

                            _SachLogic.Update(updatesl);
                        }

                        //else
                        //{
                        //    sltt = new SoLuongSachTrangThai();
                        //    sltt.IdSach = ctxs.IdSach;
                        //    sltt.IdTrangThai = ctxs.IdTinhTrang;
                        //    sltt.SoLuong = ctxs.SoLuong;
                        //    _SoLuongSachTrangThaiLogic.Insert(sltt);
                        //}
                    }
                    return RedirectToAction("Index");
                }
            }

            ViewBag.listtt = _TrangThaiSachLogic.GetAll();
            ModelState.Clear();
            return View();
        }

        // Ajax ----

        [HttpGet]
        public JsonResult _GetBookItemById(string maKiemSoat, int soLuong, string idtrangthai, string ghiChuDon, int soLuongHienThi)
        {
            var _SachLogic = new SachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            var _TrangThaiSachLogic = new TrangThaiSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            var _SoLuongSachTrangThaiLogic = new SoLuongSachTrangThaiLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);

            //Lấy số lượng tồn theo mã kiểm soát
            JsonResult result = new JsonResult();
            maKiemSoat = maKiemSoat.Trim();
            ghiChuDon = ghiChuDon.Trim();

            result.Data = null;
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            var ttModel = Newtonsoft.Json.JsonConvert.DeserializeObject<SoLuongTrangThaiSachVM>(idtrangthai); //convert idtrangthai json sang model
            string idTT = ttModel.IdTrangThai;
            //Kiểm tra số lượng tồn có lớn hơn đang có hay không
            var slSachTon = _SoLuongSachTrangThaiLogic.getBy_IdSach_IdTT(_SachLogic.GetByMaMaKiemSoat(maKiemSoat).Id, idTT).SoLuong;
            //Kiểm tra số lượng cần xuất có phù hợp hay không ( sl cần xuất >= số lần dc mượn (số lần dc mượn + sl còn = sl tổng)
            var slConLai = _SachLogic.GetByMaMaKiemSoat(maKiemSoat).SoLuongConLai;
            if (soLuong <= (slSachTon - soLuongHienThi) && (slConLai - soLuong) >= 0)
            {
                if (!string.IsNullOrEmpty(maKiemSoat) && !string.IsNullOrEmpty(ghiChuDon)
                    && soLuong > 0 && !string.IsNullOrEmpty(idTT))
                {
                    var book = _SachLogic.GetByMaMaKiemSoat(maKiemSoat);
                    var tt = _TrangThaiSachLogic.getById(idTT);

                    ChiTietXuatSachViewModels pp = new ChiTietXuatSachViewModels()
                    {
                        IdSach = book.Id,
                        ten = book.TenSach,
                        soLuong = soLuong,
                        IdTinhTrang = idTT,
                        tenTinhTrang = tt.TenTT,
                        GhiChuDon = ghiChuDon,
                        MaKiemSoat = book.MaKiemSoat
                    };

                    result.Data = pp;
                }
            }
            else
            {
                return null;
            }
            return result;
        }

        public JsonResult GetStatusByIdBook(string idBook)
        {
            if (!string.IsNullOrEmpty(idBook))
            {
                SachLogic _SachLogicLogic = new SachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
                TrangThaiSachLogic _TrangThaiSach = new TrangThaiSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
                SoLuongSachTrangThaiLogic _SLSachTTLogic = new SoLuongSachTrangThaiLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);

                Sach _sach = _SachLogicLogic.GetByMaMaKiemSoat(new SachCommon().GetInfo(idBook)); //Lấy sách theo input 
                if (_sach != null)
                {
                    var slSachTT = _SLSachTTLogic.GetByIdSach(_sach.Id); // Lấy đc IdTrangThai
                    List<TrangThaiSach> lstTTSach = new List<TrangThaiSach>();
                    foreach (var item in slSachTT)
                    {
                        if (item.SoLuong > 0)
                        {
                            var tt = _TrangThaiSach.getById(item.IdTrangThai);
                            lstTTSach.Add(tt); //lấy đc list trạng thái
                        }
                    }
                    //chuyển về listTT viewmodel
                    SoLuongTrangThaiSachVM slTTSachVM;
                    List<SoLuongTrangThaiSachVM> lstTTSachVM = new List<SoLuongTrangThaiSachVM>();
                    foreach (var item in lstTTSach)
                    {
                        slTTSachVM = new SoLuongTrangThaiSachVM()
                        {
                            IdTrangThai = item.Id,
                            TrangThai = item.TenTT,
                            IdSach = _sach.Id
                        };
                        lstTTSachVM.Add(slTTSachVM);
                    }
                    return Json(lstTTSachVM, JsonRequestBehavior.AllowGet);
                }
            }
            return null;
        }
    }
}