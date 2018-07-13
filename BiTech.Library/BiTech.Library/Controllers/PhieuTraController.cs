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

            var detail = _PhieuMuonLogic.GetReturnBooksTicket(idPM); // lay all thong tin phieu muon thong qua idPM - lay idUser

            if(detail == null)
                return RedirectToAction("Index");
            
            var thanhVien = _ThanhVienLogic.GetById(detail.IdUser); //ten ngupoi muon

            PhieuTraViewModel model = new PhieuTraViewModel()
            {
                IdPM = idPM,
                IdNguoiMuon = thanhVien.MaSoThanhVien,
                NguoiMuon = thanhVien.Ten,
                detail = detail
            };

            // Load danh sách trạng thái
            model.ListTrangThai = _TrangThaiSachLogic.GetAll();

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

            var _SachLogic = new SachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            var _PhieuTraLogic = new PhieuTraLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            var _ChiTietPhieuTraLogic = new ChiTietPhieuTraLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            var _PhieuMuonLogic = new PhieuMuonLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            var _ChiTietPhieuMuonLogic = new ChiTietPhieuMuonLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            var _TrangThaiSachLogic = new TrangThaiSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            bool result = true;
            bool traHet = true; //xét trả hết hay chưa
            model.detail = _PhieuMuonLogic.GetReturnBooksTicket(model.IdPM); 
            
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
                if (!string.IsNullOrEmpty(idPhieuTra))
                {
                    try
                    {
                        int i = model.listChiTietJsonString.Count();//số item trong listJson
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
                            int slSachPM = _ChiTietPhieuMuonLogic.GetByIdBook_IdPM(ctpt.IdSach, _PhieuTraLogic.GetById(ctpt.IdPhieuTra).IdPhieuMuon).SoLuong;
                            var lstSachPT = _ChiTietPhieuTraLogic.GetByIdBook(ctpt.IdSach);
                            int slSachTra = 0; //so luong sach da tra
                            foreach(var item in lstSachPT)
                            {
                                slSachTra += item.SoLuong;
                            }
                            //trả hết thì cập nhật lại ngày trả của phiếu muợn
                            if (slSachTra == slSachPM && traHet == true)
                                traHet = true; //trả hết
                            else
                                traHet = false; //trả chưa hết
                            --i;
                            if (traHet && i==0)
                            {
                                try
                                {
                                    //lấy số lượng sách trong PhieuMuon
                                    if (Validate(ctModel.SoLuong, ctModel.IdSach, model.IdPM, userdata)) //true - cập nhật ngày trả trong phiếu mượn
                                    {
                                        var phieuMuon = _PhieuMuonLogic.GetById(model.IdPM);
                                        phieuMuon.NgayTra = DateTime.Now;
                                        phieuMuon.TrangThaiPhieu = EPhieuMuon.DaTra;
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
                            }
                            //update so lượng bảng Sach
                            var sach = _SachLogic.GetById(ctpt.IdSach); //id mongo
                            sach.SoLuongConLai += ctpt.SoLuong;
                            _SachLogic.Update(sach); //update 

                            //string idUser = _PhieuMuonLogic.GetById(model.IdPM).IdUser;
                            result = true;                   
                        }
                        if(result)
                        {
                            TempData["Success"] = "Tạo phiếu trả thành công";
                            return RedirectToAction("Index", "PhieuMuon");
                        }
                         else
                        {
                            TempData["UnSuccess"] = "Tạo phiếu trả thất bại";
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

            //// Load danh sách trạng thái
            model.ListTrangThai = _TrangThaiSachLogic.GetAll();

            return View(model);
        }

        #region Angular
        /// <summary>
        /// Hàm lấy thông tin phiếu tra
        /// id : id sách (mongo id)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="soLuong"></param>
        /// <param name="idTrangThai"></param>
        /// <param name="idPM"></param>
        /// <returns></returns>
        public JsonResult GetThongTinPhieuTra(string id, int soLuong, string idTrangThai, string idPM)
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
            if (!String.IsNullOrWhiteSpace(id))
            {
                var sach = _SachLogic.GetById(id);
                var chiTiet = _ChiTietPhieuTraLogic.GetByIdBook(sach.Id);
                var trangThai = _TrangThaiSachLogic.getById(idTrangThai);
                ChiTietPhieuTraViewModel ctpt = new ChiTietPhieuTraViewModel()
                {
                    IdSach = sach.Id,
                    MaKiemSoat = sach.MaKiemSoat,
                    TenSach = sach.TenSach,
                    SoLuong = soLuong,
                    IdTrangThaiSach = idTrangThai,
                    TrangThaiSach = trangThai.TenTT,
                };
                TempData["SoLuong"] = soLuong;
                var a = _ChiTietPhieuMuonLogic.GetByIdBook_IdPM(sach.Id, idPM).SoLuong;
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