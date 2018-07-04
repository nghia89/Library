using BiTech.Library.BLL.DBLogic;
using BiTech.Library.DTO;
using BiTech.Library.Helpers;
using BiTech.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BiTech.Library.Controllers
{
    public class PhieuTraController : BaseController
    {
        // GET: PhieuTra
        public ActionResult Index()
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            var _TrangThaiSachLogic = new TrangThaiSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

            ViewBag.ListTinhTrangSach = _TrangThaiSachLogic.GetAll();
            return View();
        }

        /// <summary>
        /// Danh sách phiếu mượn của thành viên
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult _DanhSachPhieu(PhieuTraViewModel model)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            return RedirectToAction("Index", "PhieuMuon", new { @IdUser = model.IdNguoiMuon });
        }

        /// <summary>
        /// Tạo phiếu trả sách
        /// HttpGet
        /// </summary>
        /// <returns></returns>
        public ActionResult TaoPhieuTra(string idPM)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            #region Logics

            var _ThanhVienLogic = new ThanhVienLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

            var _TrangThaiSachLogic = new TrangThaiSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

            var _PhieuMuonLogic = new PhieuMuonLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            
            #endregion Logics

            ViewBag.Success = TempData["Success"] = "";
            ViewBag.UnSuccess = TempData["UnSuccess"] = "";
            ViewBag.SoLuong = TempData["SoLuong"] = "";

            var phieuMuon = _PhieuMuonLogic.GetById(idPM); // lay phieumuon thong qua idPM - lay idUser

            var thanhVien = _ThanhVienLogic.GetByIdUser(phieuMuon.IdUser); //ten ngupoi muon
            
            PhieuTraViewModel model = new PhieuTraViewModel()
            {
                IdPM = idPM,
                IdNguoiMuon = thanhVien.MaSoThanhVien,
                NguoiMuon = thanhVien.Ten,
            };

            var listTTSach = _TrangThaiSachLogic.GetAll();

            // Load danh sách trạng thái
            ViewBag.ListTinhTrangSach = _TrangThaiSachLogic.GetAll();

            foreach (var item in listTTSach)
            {
                model.ListTrangThai.Add(new TrangThai()
                {
                    Id = item.Id,
                    TenTT = item.TenTT
                });
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult TaoPhieuTra(PhieuTraViewModel model)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            var _PhieuTraLogic = new PhieuTraLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            var _ChiTietPhieuTraLogic = new ChiTietPhieuTraLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            var _PhieuMuonLogic = new PhieuMuonLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            var _TrangThaiSachLogic = new TrangThaiSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

            if (model.listChiTietJsonString.Count > 0)
            {
                PhieuTra phieuTra = new PhieuTra()
                {
                    IdPhieuMuon = model.IdPM,
                    NgayTra = DateTime.Now,
                    CreateDateTime = DateTime.Now
                };
                //insert phieutra
                string idPhieuTra = _PhieuTraLogic.Insert(phieuTra);
                if (!String.IsNullOrEmpty(idPhieuTra))
                {
                    try
                    {
                        foreach (var json in model.listChiTietJsonString)
                        {
                            var ctModel = Newtonsoft.Json.JsonConvert.DeserializeObject<ChiTietPhieuTraViewModel>(json);

                            var ctpt = new ChiTietPhieuTra()
                            {
                                IdPhieuTra = idPhieuTra,
                                IdSach = ctModel.IdSach,
                                IdTrangThai = ctModel.IdTrangThaiSach,
                                SoLuong = ctModel.SoLuong,
                                CreateDateTime = DateTime.Now
                            };
                            //Insert chitiet phieutra
                            string idCtpt = _ChiTietPhieuTraLogic.Insert(ctpt);

                            // update ngày trả PhieuMuon nếu như trả hết

                            try
                            {
                                //lấy số lượng sách trong PhieuMuon
                                if (Validate(ctModel.SoLuong, ctModel.IdSach, model.IdPM, userdata)) //true - cập nhật ngày trả trong phiếu mượn
                                {
                                    var phieuMuon = _PhieuMuonLogic.GetById(model.IdPM);
                                    phieuMuon.NgayTra = DateTime.Now;
                                    _PhieuMuonLogic.Update(phieuMuon);
                                }
                            }
                            catch (Exception ex)
                            {
                                //nếu update không thành công thì undo
                                _PhieuTraLogic.Remove(idPhieuTra);
                                _ChiTietPhieuTraLogic.Remove(idCtpt);
                                throw ex;
                            }
                            string idUser = _PhieuMuonLogic.GetById(model.IdPM).IdUser;
                            if (!String.IsNullOrEmpty(idCtpt)) //true
                            {
                                TempData["Success"] = "Tạo phiếu mượn thành công";
                                return RedirectToAction("Index", "PhieuMuon", new { @IdUser = idUser });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //insert chitietphieutra khong thành công- xoa phieutra
                        _PhieuTraLogic.Remove(idPhieuTra);
                        throw ex;
                    }
                }
            }
            var listTTSach = _TrangThaiSachLogic.GetAll();
            // Load danh sách trạng thái
            ViewBag.ListTinhTrangSach = _TrangThaiSachLogic.GetAll();

            foreach (var item in listTTSach)
            {
                model.ListTrangThai.Add(new TrangThai()
                {
                    Id = item.Id,
                    TenTT = item.TenTT
                });
            }

            return View(model);
        }

        #region Angular

        public JsonResult GetThongTinPhieuTra(string idBook, int soLuong, string idTrangThai, string idPM)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return Json(null, JsonRequestBehavior.AllowGet); //RedirectToAction("LogOff", "Account");
            #endregion

            var _ChiTietPhieuTraLogic = new ChiTietPhieuTraLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            var _ChiTietPhieuMuonLogic = new ChiTietPhieuMuonLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            var _SachLogic = new SachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            var _TrangThaiSachLogic = new TrangThaiSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

            JsonResult result = new JsonResult();
            if (!String.IsNullOrWhiteSpace(idBook))
            {
                var sach = _SachLogic.GetByIdBook(idBook);
                var chiTiet = _ChiTietPhieuTraLogic.GetByIdBook(idBook);
                var trangThai = _TrangThaiSachLogic.getById(idTrangThai);
                ChiTietPhieuTraViewModel ctpt = new ChiTietPhieuTraViewModel();
                {
                    ctpt.IdSach = sach.IdDauSach;
                    ctpt.TenSach = sach.TenSach;
                    ctpt.SoLuong = soLuong;
                    ctpt.IdTrangThaiSach = idTrangThai;
                    ctpt.TrangThaiSach = trangThai.TenTT;
                };
                TempData["SoLuong"] = soLuong;
                var a = _ChiTietPhieuMuonLogic.GetByIdBook_IdPM(idBook, idPM).SoLuong;
                if (soLuong > a)
                {
                    result.Data = null;
                }
                else
                {
                    result.Data = ctpt;
                }
                //lstSach.Add(idBook); // add item to list to get list idSach - inset chitietphieumuon
                result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            }
            return result;
        }

        #endregion

        #region private

        /// <summary>
        /// Kiểm tra số lượng sách đã trả hêt hay chưa
        /// True = đóng phieumuon
        /// False = chưa đóng
        /// </summary>
        /// <param name="idSach"></param>
        /// <param name="soLuong"></param>
        /// <returns></returns>
        private bool Validate(int soLuong, string idSach, string idPM, SSOUserDataModel userdata)
        {
            var _ChiTietPhieuMuonLogic = new ChiTietPhieuMuonLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

            int soLuongMuon = _ChiTietPhieuMuonLogic.GetByIdBook_IdPM(idSach, idPM).SoLuong;
            return soLuong == soLuongMuon ? true : false;
        }

        #endregion
    }
}