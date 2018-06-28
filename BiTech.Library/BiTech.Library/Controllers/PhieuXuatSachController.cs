using BiTech.Library.BLL.DBLogic;
using BiTech.Library.DTO;
using BiTech.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BiTech.Library.Controllers
{
    public class PhieuXuatSachController : BaseController
    {
        // GET: PhieuXuatSach
        public ActionResult Index()
        {
            return View();
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


            if (model.listChiTietJsonString.Count > 0)
            {
                PhieuXuatSach pxs = new PhieuXuatSach()
                {
                    NgayNhap = DateTime.Now,
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
                            IdPhieuNhap = idPhieuXuat,
                            IdSach = ctModel.IdSach,
                            IdTinhtrang = ctModel.IdTinhTrang,
                            IdLyDo = ctModel.IdLydo,
                            soLuong = ctModel.soLuong,
                            CreateDateTime = DateTime.Now
                        };

                        _ChiTietNhapSachLogic.Insert(ctxs);
                        {
                            //update số lượng sách trạng thái
                            var sltt = _SoLuongSachTrangThaiLogic.getBy_IdSach_IdTT(ctxs.IdSach, ctxs.IdTinhtrang);
                            sltt.SoLuong -= ctxs.soLuong;
                            _SoLuongSachTrangThaiLogic.Update(sltt);
                            //update tổng số lượng sách
                            var updatesl = _SachLogic.GetBookById(sltt.IdSach);
                            updatesl.SoLuong -= ctxs.soLuong;
                            _SachLogic.Update(updatesl);
                        }
                    }
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