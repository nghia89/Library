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
        private PhieuMuonLogic _PhieuMuonLogic;
        private PhieuTraLogic _PhieuTraLogic;
        private ChiTietPhieuTraLogic _ChiTietPhieuTraLogic;
        private ChiTietPhieuMuonLogic _ChiTietPhieuMuonLogic;
        private SachLogic _SachLogic;
        private ThanhVienLogic _ThanhVienLogic;
        private TrangThaiSachLogic _TrangThaiSachLogic;
        public PhieuTraController()
        {
            _PhieuMuonLogic = new PhieuMuonLogic(Tool.GetConfiguration("ConnectionString"), Tool.GetConfiguration("DatabaseName"));
            _PhieuTraLogic = new PhieuTraLogic(Tool.GetConfiguration("ConnectionString"), Tool.GetConfiguration("DatabaseName"));
            _ChiTietPhieuTraLogic = new ChiTietPhieuTraLogic(Tool.GetConfiguration("ConnectionString"), Tool.GetConfiguration("DatabaseName"));
            _ChiTietPhieuMuonLogic = new ChiTietPhieuMuonLogic(Tool.GetConfiguration("ConnectionString"), Tool.GetConfiguration("DatabaseName"));
            _SachLogic = new SachLogic(Tool.GetConfiguration("ConnectionString"), Tool.GetConfiguration("DatabaseName"));
            _ThanhVienLogic = new ThanhVienLogic(Tool.GetConfiguration("ConnectionString"), Tool.GetConfiguration("DatabaseName"));
            _TrangThaiSachLogic = new TrangThaiSachLogic(Tool.GetConfiguration("ConnectionString"), Tool.GetConfiguration("DatabaseName"));
        }
        // GET: PhieuTra
        public ActionResult Index()
        {
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
            return RedirectToAction("Index", "PhieuMuon", new { @IdUser = model.IdNguoiMuon });
        }

        /// <summary>
        /// Tạo phiếu trả sách
        /// HttpGet
        /// </summary>
        /// <returns></returns>
        public ActionResult TaoPhieuTra(string idPM)
        {
            ViewBag.Success = TempData["Success"];
            ViewBag.UnSuccess = TempData["UnSuccess"];
            ViewBag.SoLuong = TempData["SoLuong"];
            PhieuMuon pm = _PhieuMuonLogic.GetById(idPM); // lay phieumuon thong qua idPM - lay idUser
            ThanhVien nguoiMuon = _ThanhVienLogic.GetByIdUser(pm.IdUser); //ten ngupoi muon

            PhieuTraViewModel model = new PhieuTraViewModel()
            {
                IdPM = idPM,
                IdNguoiMuon = nguoiMuon.MaSoThanhVien,
                NguoiMuon = nguoiMuon.Ten,
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
                                if (Validate(ctModel.SoLuong, ctModel.IdSach, model.IdPM)) //true - cập nhật ngày trả trong phiếu mượn
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
        /// <summary>
        /// Kiểm tra số lượng sách đã trả hêt hay chưa
        /// True = đóng phieumuon
        /// False = chưa đóng
        /// </summary>
        /// <param name="idSach"></param>
        /// <param name="soLuong"></param>
        /// <returns></returns>
        public bool Validate(int soLuong, string idSach, string idPM)
        {
            int soLuongMuon = _ChiTietPhieuMuonLogic.GetByIdBook_IdPM(idSach, idPM).SoLuong;
            return soLuong == soLuongMuon ? true : false;
        }

        public JsonResult GetThongTinPhieuTra(string idBook, int soLuong, string idTrangThai)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return Json(null, JsonRequestBehavior.AllowGet); //RedirectToAction("LogOff", "Account");
            #endregion

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
                var b = _PhieuTraLogic.GetById(chiTiet[0].Id).Id;
                var a = _ChiTietPhieuMuonLogic.GetByIdBook_IdPM(idBook, b).SoLuong;
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

        //public ActionResult TraSach(string idPM, string idSach)
        //{
        //    ViewBag.ListTinhTrangSach = _TrangThaiSachLogic.GetAll();
        //    PhieuMuon pm = _PhieuMuonLogic.GetById(idPM); // lay phieumuon thong qua idPM - lay idUser  
        //    ThanhVien nguoiMuon = _ThanhVienLogic.GetByIdUser(pm.IdUser); //ten ngupoi muon

        //    Sach sach = _SachLogic.GetByIdBook(idSach); //lay ten sach
        //    PhieuTraViewModel model = new PhieuTraViewModel()
        //    {
        //        IdSach = idSach,
        //        IdPM = idPM,
        //        IdNguoiMuon = nguoiMuon.MaSoThanhVien,
        //        NguoiMuon = nguoiMuon.Ten,
        //        TenSach = sach.TenSach,
        //    };
        //    return PartialView(model);
        //}

        //[HttpPost]
        //public ActionResult TraSach(PhieuTraViewModel model, string idPM, string idSach)
        //{
        //    try
        //    {
        //        PhieuTra ptModel = new PhieuTra()
        //        {
        //            IdPhieuMuon = idPM,
        //            CreateDateTime = DateTime.Now,
        //            NgayTra = DateTime.Now
        //        };
        //        // insert phieu tra
        //        string idPT = _PhieuTraLogic.Insert(ptModel);

        //        if (!String.IsNullOrEmpty(idPT))
        //        {
        //            // insert chitietPT
        //            try
        //            {
        //                ChiTietPhieuTra ctPTModel = new ChiTietPhieuTra()
        //                {
        //                    IdPhieuTra = idPT,
        //                    IdSach = idSach,
        //                };
        //            }
        //            catch (Exception ex)
        //            {
        //                throw ex;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return RedirectToAction("ChiTietPhieuMuon", "PhieuMuon", new { @id = idPM });
        //}
    }
}