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
                    IdPhieuXuat=item.Id,
                    IdUserAdmin=item.IdUserAdmin,
                    GhiChu=item.GhiChu,
                    NgayXuat=item.NgayXuat

                };
                lstpxs.Add(pxs);
            }
            
            return View(lstpxs.OrderByDescending(x=>x.NgayXuat).ToPagedList(PageNumber,PageSize));
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
            PhieuXuatSachModels pxs = new PhieuXuatSachModels()
            {

                IdUserAdmin = phieuxuat.IdUserAdmin,
                GhiChu = phieuxuat.GhiChu,
                NgayXuat = phieuxuat.NgayXuat

            };
            List<ChiTietXuatSachViewModels> lst = new List<ChiTietXuatSachViewModels>();
            foreach(var item in model)
            {
               
                ChiTietXuatSachViewModels ctxs = new ChiTietXuatSachViewModels();
                ctxs.Id = item.Id;
                ctxs.IdLydo = item.IdLyDo;
                var LyDo = _LyDoXuatLogic.GetById(ctxs.IdLydo);
                ctxs.lyDo = LyDo.LyDo;
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
                    NgayXuat = DateTime.Now,
                    GhiChu = model.GhiChu,
                    IdUserAdmin = "",

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
                            IdLyDo = ctModel.IdLydo,
                            SoLuong = ctModel.soLuong,
                            CreateDateTime = DateTime.Now
                        };

                        _ChiTietNhapSachLogic.Insert(ctxs);
                        {
                            //update số lượng sách trạng thái
                            var sltt = _SoLuongSachTrangThaiLogic.getBy_IdSach_IdTT(ctxs.IdSach, ctxs.IdTinhTrang);
                            sltt.SoLuong -= ctxs.SoLuong;
                            _SoLuongSachTrangThaiLogic.Update(sltt);
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
        public JsonResult _GetBookItemById(string idBook, int soLuong, string idtrangthai, string idlydo)
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

            JsonResult result = new JsonResult();
            if (!String.IsNullOrWhiteSpace(idBook))
            {
                var book = _SachLogic.GetByIdBook(idBook);
                var tt = _TrangThaiSachLogic.getById(idtrangthai);
                var ld = _LyDoXuat.GetById(idlydo);
                ChiTietXuatSachViewModels pp = new ChiTietXuatSachViewModels();
                {
                    pp.IdSach = book.Id;
                    pp.IdDauSach = book.IdDauSach;
                    pp.ten = book.TenSach;
                    pp.soLuong = soLuong;
                    pp.IdTinhTrang = idtrangthai;
                    pp.tenTinhTrang = tt.TenTT;
                    pp.IdLydo = idlydo;
                    pp.lyDo = ld.LyDo;
                };

                result.Data = pp;
                //lstSach.Add(idBook); // add item to list to get list idSach - inset chitietphieumuon
                result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            }
            return result;
        }
    }
}