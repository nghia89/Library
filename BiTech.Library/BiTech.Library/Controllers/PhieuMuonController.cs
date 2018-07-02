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
    //[Authorize]
    public class PhieuMuonController : BaseController
    {
        public static int SoNgayPhaiTra = 15;

        //private PhieuMuonLogic _PhieuMuonLogic;
        //private ChiTietPhieuMuonLogic _ChiTietPhieuMuonLogic;
        //private PhieuTraLogic _PhieuTraLogic;
        //private ChiTietPhieuTraLogic _ChiTietPhieuTraLogic;
        //private SachLogic _SachLogic;
        //private ThanhVienLogic _ThanhVienLogic;
        //private TrangThaiSachLogic _TrangThaiSachLogic;

        public PhieuMuonController()
        {
            //_PhieuMuonLogic = new PhieuMuonLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            //_ChiTietPhieuMuonLogic = new ChiTietPhieuMuonLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            //_PhieuTraLogic = new PhieuTraLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            //_ChiTietPhieuTraLogic = new ChiTietPhieuTraLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            //_SachLogic = new SachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            //_ThanhVienLogic = new ThanhVienLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            //_TrangThaiSachLogic = new TrangThaiSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
        }

        // GET: PhieuMuon
        public ActionResult Index(string IdUser)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            PhieuMuonLogic _PhieuMuonLogic = new PhieuMuonLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            ThanhVienLogic _ThanhVienLogic = new ThanhVienLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

            List<PhieuMuon> lstPhieuMuon = new List<PhieuMuon>();
            if (string.IsNullOrEmpty(IdUser))
            {
                lstPhieuMuon = _PhieuMuonLogic.GetAll();
            }
            else
            {
                lstPhieuMuon = _PhieuMuonLogic.GetByIdUser(IdUser);
            }

            PhieuMuonModelView _vmodel;
            List<PhieuMuonModelView> vmodel = new List<PhieuMuonModelView>();

            foreach (var item in lstPhieuMuon)
            {
                _vmodel = new PhieuMuonModelView();
                _vmodel.Id = item.Id;
                _vmodel.IdUser = item.IdUser;
                _vmodel.TenNguoiMuon = _ThanhVienLogic.GetByIdUser(item.IdUser).Ten;
                _vmodel.NgayMuon = item.NgayMuon;
                _vmodel.NgayPhaiTra = item.NgayPhaiTra;
                _vmodel.NgayTra = item.NgayTra;
                _vmodel.GiaHan = item.GiaHan;
                _vmodel.GhiChu = item.GhiChu;
                if (item.TrangThaiPhieu == EPhieuMuon.DaTra)
                {
                    _vmodel.TrangThai = "Đã trả";
                }
                else
                {
                    if (item.TrangThaiPhieu == EPhieuMuon.ChuaTra)
                    {
                        _vmodel.TrangThai = "Chưa trả";
                    }
                }
                //Add vào list 
                vmodel.Add(_vmodel);
            }
            return View(vmodel);
        }

        //public PartialViewResult _PartialPhieuMuon(string IdUser)
        //{
        //    List<PhieuMuon> lstPhieuMuon = new List<PhieuMuon>();
        //    if (String.IsNullOrEmpty(IdUser))
        //    {
        //        lstPhieuMuon = _PhieuMuonLogic.GetAll();
        //    }
        //    else
        //    {
        //        lstPhieuMuon = _PhieuMuonLogic.GetByIdUser(IdUser);
        //    }
        //    PhieuMuonModelView _vmodel;
        //    List<PhieuMuonModelView> vmodel = new List<PhieuMuonModelView>();
        //    foreach (var item in lstPhieuMuon)
        //    {
        //        _vmodel = new PhieuMuonModelView();
        //        _vmodel.Id = item.Id;
        //        _vmodel.IdUser = item.IdUser;
        //        _vmodel.TenNguoiMuon = _ThanhVienLogic.GetByIdUser(item.IdUser).Ten;
        //        _vmodel.NgayMuon = item.NgayMuon;
        //        _vmodel.NgayPhaiTra = item.NgayPhaiTra;
        //        _vmodel.NgayTra = item.NgayTra;
        //        _vmodel.GiaHan = item.GiaHan;
        //        _vmodel.GhiChu = item.GhiChu;
        //        if (item.TrangThaiPhieu == EPhieuMuon.DaTra)
        //        {
        //            _vmodel.TrangThai = "Đã trả";
        //        }
        //        else
        //        {
        //            if (item.TrangThaiPhieu == EPhieuMuon.ChuaTra)
        //            {
        //                _vmodel.TrangThai = "Chưa trả";
        //            }
        //        }
        //        //Add vào list 
        //        vmodel.Add(_vmodel);
        //    }
        //    return PartialView(vmodel);
        //}

        public ActionResult _CreatePhieuMuon()
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            ViewBag.Success = TempData["Success"];
            ViewBag.UnSuccess = TempData["UnSuccess"];
            ViewBag.SoLuong = TempData["SoLuong"];
            ViewBag.Date = TempData["Date"];

            PhieuMuonModelView model = new PhieuMuonModelView()
            {
                NgayPhaiTra = DateTime.Today.AddDays(1),
                MaSach = new List<string>()

            };
            ViewBag.MaxDate = SoNgayPhaiTra;
            return View(model);
        }

        /// <summary>
        /// Insert vào 2 bảng PhieuMuon và ChiTietPhieuMuon và cập nhật lại số lượng sách bảng Sach
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _CreatePhieuMuon(PhieuMuonModelView viewModel)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            PhieuMuonLogic _PhieuMuonLogic = new PhieuMuonLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            ChiTietPhieuMuonLogic _ChiTietPhieuMuonLogic = new ChiTietPhieuMuonLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

            try
            {
                double days = (viewModel.NgayPhaiTra - DateTime.Today).TotalDays;

                if (0 < days && days <= SoNgayPhaiTra) //ngày mượn phải nho hơn hoặc bằng số ngày được phép mượn
                {
                    PhieuMuon modelPM = new PhieuMuon()
                    {
                        IdUser = viewModel.IdUser,
                        NgayMuon = DateTime.Today, // luôn luôn đúng là ngày hôm nay. ko tính giờ
                        NgayPhaiTra = viewModel.NgayPhaiTra,
                        NgayTra = null,
                        TrangThaiPhieu = EPhieuMuon.ChuaTra, // mac dinh - Chua Tra
                        CreateDateTime = DateTime.Now
                    };

                    //Kiểm tra số lượng

                    List<string> query = new List<string>(); //List tam de so sanh
                    int soLuong = 0;
                    //lay so luong tung cuon sach
                    foreach (var idSach in viewModel.MaSach)
                    {
                        if (query.Contains(idSach))
                            continue;
                        for (int i = 0; i < viewModel.MaSach.Count; i++)
                        {
                            if (idSach == viewModel.MaSach[i])
                            {
                                ++soLuong;
                            }
                            if (!query.Contains(idSach))
                                query.Add(idSach);
                        }

                        if (!Validate(idSach, soLuong, userdata)) //kiem tra tung ma sach _ 
                        {
                            TempData["SoLuong"] = "Số lượng sách mượn không phù hợp";
                            soLuong = 0;
                            return View(viewModel);
                        }
                        soLuong = 0;
                    }

                    //Insert bảng PhieuMuon
                    string idPhieuMuon = _PhieuMuonLogic.Insert(modelPM);
                    if (!string.IsNullOrEmpty(idPhieuMuon))
                    {
                        ChiTietPhieuMuon modelCTPM = new ChiTietPhieuMuon()
                        {
                            IdPhieuMuon = idPhieuMuon,
                            //IdSach = viewModel.MaSach,
                            SoLuong = viewModel.SoLuong,
                            CreateDateTime = DateTime.Now,
                        };
                        try
                        {
                            query.Clear();
                            //lay so luong tung cuon sach
                            foreach (var idSach in viewModel.MaSach)
                            {
                                if (query.Contains(idSach))
                                    continue;
                                for (int i = 0; i < viewModel.MaSach.Count; i++)
                                {
                                    if (idSach == viewModel.MaSach[i])
                                    {
                                        ++soLuong;
                                    }
                                    if (!query.Contains(idSach))
                                        query.Add(idSach);
                                }

                                modelCTPM.SoLuong = soLuong; //so luong muon cua Ma Sach
                                modelCTPM.IdSach = idSach; // Gắn mã sách vào 1 chitietphieumuon
                                                           //Insert bảng ChiTietPhieuMuon
                                modelCTPM.Id = "";
                                string idCTPM = _ChiTietPhieuMuonLogic.Insert(modelCTPM);
                                soLuong = 0;

                                //bool result = false;

                                //#region Update bảng Sach
                                //// Update lại số lượng sách trong bảng sách
                                //Sach modelSach = _SachLogic.GetByIdBook(modelCTPM.IdSach);
                                //if (modelSach != null)
                                //{
                                //    modelSach.SoLuong -= modelCTPM.SoLuong;
                                //    result = _SachLogic.Update(modelSach);
                                //}
                                //#endregion

                                if (!string.IsNullOrEmpty(idCTPM)) //true
                                {
                                    TempData["Success"] = "Tạo phiếu mượn thành công";
                                }
                            }
                            return RedirectToAction("Index", "PhieuMuon");
                        }
                        catch (Exception ex)
                        {
                            //Xoa data trong PhieuMuon khi insert ChiTiet bi loi
                            _PhieuMuonLogic.Remove(idPhieuMuon);
                            throw ex;
                        }

                    }
                    TempData["UnSuccess"] = "Tạo phiếu mượn thất bại";
                    return View(viewModel);
                }
                else
                {
                    TempData["Date"] = "Ngày phải trả không phù hợp";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(viewModel);
        }

        //Get
        public ActionResult _EditPhieuMuon(string id)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            PhieuMuonLogic _PhieuMuonLogic = new PhieuMuonLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

            ViewBag.Success = TempData["Success"];
            ViewBag.UnSucces = TempData["UnSuccess"];
            PhieuMuon us = _PhieuMuonLogic.GetById(id);

            PhieuMuonModelView model = new PhieuMuonModelView()
            {
                IdUser = us.IdUser,
                // TenNguoiMuon = us.,
                NgayMuon = us.NgayMuon,
                NgayPhaiTra = us.NgayPhaiTra,
                TrangThaiPhieu = us.TrangThaiPhieu,
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult _EditPhieuMuon(PhieuMuonModelView viewModel, string id)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            PhieuMuonLogic _PhieuMuonLogic = new PhieuMuonLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

            var model = _PhieuMuonLogic.GetById(id); //lay 1 tai khoan 
            model.IdUser = viewModel.IdUser;
            model.NgayMuon = viewModel.NgayMuon;
            model.NgayPhaiTra = viewModel.NgayPhaiTra;
            // model.TrangThaiPhieu = viewModel.TrangThaiPhieu;

            //Cap nhat voi tai khoan moi
            bool result = _PhieuMuonLogic.Update(model);
            if (result == true)
            {
                TempData["Success"] = "Cập nhật thành công";
                return RedirectToAction("Index", "PhieuMuon");
            }
            else
                TempData["UnSuccess"] = "Cập nhật không thành công";
            return View();
        }

        public ActionResult Delete(string id)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            PhieuMuonLogic _PhieuMuonLogic = new PhieuMuonLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            ChiTietPhieuMuonLogic _ChiTietPhieuMuonLogic = new ChiTietPhieuMuonLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

            ViewBag.Success = TempData["Success"];
            var model = _PhieuMuonLogic.GetById(id);
            var trangThaiCTPM = model.TrangThaiPhieu;
            model.TrangThaiPhieu = EPhieuMuon.Deleted;

            bool result = _PhieuMuonLogic.Update(model);
            try
            {
                if (result == true)
                {
                    var lstPMChiTiet = _ChiTietPhieuMuonLogic.GetByIdPhieuMuon(id);
                    //update lại bảng chitiet - isDelete
                    foreach (var item in lstPMChiTiet)
                    {
                        result = _ChiTietPhieuMuonLogic.Remove(item.Id);
                    }
                }
            }
            catch (Exception ex)
            {
                //update k thành công
                model.TrangThaiPhieu = trangThaiCTPM;
                _PhieuMuonLogic.Update(model);
                throw ex;
            }

            if (result == true)
            {
                TempData["Success"] = "Xóa thành công";
                return RedirectToAction("Index", "PhieuMuon");
            }
            else
                TempData["Success"] = "Xóa thất bại";
            return View();
        }

        #region Angular

        /// <summary>
        /// Lấy thông tin sách thông qua idBook
        /// </summary>
        /// <param name="idBook"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult _GetBookItemById(string idBook)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return null;
            #endregion
            SachLogic _SachLogic = new SachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

            JsonResult result = new JsonResult();
            if (!String.IsNullOrWhiteSpace(idBook))
            {
                result.Data = _SachLogic.GetByIdBook(idBook);
                //lstSach.Add(idBook); // add item to list to get list idSach - inset chitietphieumuon
                result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            }
            return result;
        }

        public JsonResult GetName(string idUser)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return null;
            #endregion
            ThanhVienLogic _ThanhVienLogic = new ThanhVienLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

            JsonResult result = new JsonResult();
            if (!String.IsNullOrWhiteSpace(idUser))
            {
                var aaa = _ThanhVienLogic.GetByIdUser(idUser);
                result.Data = aaa;
                //lstSach.Add(idBook); // add item to list to get list idSach - inset chitietphieumuon
                result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            }
            return result;
        }

        /// <summary>
        /// Lấy danh sách phieumuon
        /// </summary>
        /// <param name="idPM"></param>
        /// <param name="soLuong"></param>
        /// <returns></returns>
        public JsonResult GetChiTietPhieuJSon(string idPM, int soLuong)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return null;
            #endregion
            
            SachLogic _SachLogic = new SachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            ChiTietPhieuMuonLogic _ChiTietPhieuMuonLogic = new ChiTietPhieuMuonLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

            JsonResult result = new JsonResult();
            var chiTietPM_DTO = _ChiTietPhieuMuonLogic.GetByIdPhieuMuon(idPM);
            List<ChiTietPMViewModel> lstCT = new List<ChiTietPMViewModel>();
            ChiTietPMViewModel chiTietModel;
            foreach (var item in chiTietPM_DTO)
            {
                int a = TempData["SoLuong"] == null ? soLuong : Convert.ToInt32(TempData["SoLuong"].ToString());
                if (TempData["SoLuong"] == null)
                    TempData["SoLuongView"] = null;
                chiTietModel = new ChiTietPMViewModel()
                {
                    IdPM = item.IdPhieuMuon,
                    MaSach = item.IdSach,
                    TenSach = _SachLogic.GetByIdBook(item.IdSach).TenSach,
                    SoLuong = TempData["SoLuongView"] == null ? item.SoLuong - a : (int.Parse(TempData["SoLuongView"].ToString()) - a),
                };
                if (TempData["SoLuong"] != null)
                    TempData["SoLuongView"] = item.SoLuong - a;

                if (chiTietModel.SoLuong <= 0)
                {
                    lstCT.Remove(chiTietModel);
                }
                else
                {
                    lstCT.Add(chiTietModel);
                }

                //Neu so luong  = 0 => k hien thi tren giao dien             

            }
            if (!String.IsNullOrWhiteSpace(idPM))
            {
                result.Data = lstCT;
                //lstSach.Add(idBook); // add item to list to get list idSach - inset chitietphieumuon
                result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            }
            return result;
        }

        #endregion

        #region private


        /// <summary>
        /// Hàm kiểm tra số lượng mượn _ số lượng hiện có
        /// True = Được mượn, false = ngược lại
        /// </summary>
        /// <param name="idSach"></param>
        /// <param name="soLuong"></param>
        /// <returns></returns>
        private bool Validate(string idSach, int soLuong, SSOUserDataModel userdata)
        {
            ChiTietPhieuMuonLogic _ChiTietPhieuMuonLogic = new ChiTietPhieuMuonLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            ChiTietPhieuTraLogic _ChiTietPhieuTraLogic = new ChiTietPhieuTraLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            SachLogic _SachLogic = new SachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

            Sach sach = _SachLogic.GetByIdBook(idSach);

            List<ChiTietPhieuMuon> ctpm = _ChiTietPhieuMuonLogic.GetByIdBook(idSach); // so luong sach dc muon
            int slSachMuon = 0;
            int slSachTra = 0;
            foreach (var item in ctpm) //so luong sach muon
            {
                slSachMuon += item.SoLuong;
            }
            List<ChiTietPhieuTra> ctpt = _ChiTietPhieuTraLogic.GetByIdBook(idSach); // so luong sach da tra
            foreach (var item in ctpt) //so luong sach tra
            {
                slSachTra += item.SoLuong;
            }
            //(tong - (muon -tra) = kha dung
            return soLuong <= (sach.SoLuong - (slSachMuon - slSachTra)) ? true : false;
        }

        #endregion

        public ActionResult ChiTietPhieuMuon(string id)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            SachLogic _SachLogic = new SachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            PhieuMuonLogic _PhieuMuonLogic = new PhieuMuonLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            ChiTietPhieuMuonLogic _ChiTietPhieuMuonLogic = new ChiTietPhieuMuonLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

            List<ChiTietPhieuMuon> lstChiTietPM = _ChiTietPhieuMuonLogic.GetByIdPhieuMuon(id); //Lấy danh sách chitiet thông qua id PhieuMuon
            List<ChiTietPMViewModel> modelChiTietPM = new List<ChiTietPMViewModel>();
            ChiTietPMViewModel _modelChiTietPM;

            var phieuMuon = _PhieuMuonLogic.GetById(id); //id của phiếu mượn


            foreach (var item in lstChiTietPM)
            {
                _modelChiTietPM = new ChiTietPMViewModel();
                _modelChiTietPM.NgayMuon = phieuMuon.NgayMuon;
                _modelChiTietPM.IdPM = item.IdPhieuMuon; //idPhieuMuon
                _modelChiTietPM.MaSach = item.IdSach;

                var sach = _SachLogic.GetByIdBook(item.IdSach);
                _modelChiTietPM.TenSach = sach.TenSach;

                _modelChiTietPM.SoLuong = item.SoLuong; // Số lượng sách mượn

                modelChiTietPM.Add(_modelChiTietPM); //add to list
            }

            return View(modelChiTietPM);
        }

        public ActionResult GiaHanPhieuMuon(string idPM)
        {
            return View();
        }

        [HttpGet]
        public ActionResult TaoMaBarCode_QR()
        {
            return View();
        }

        [HttpPost]
        public ActionResult TaoMaBarCode_QR(SachViewModels model)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion
            SachLogic _SachLogic = new SachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

            var sach = _SachLogic.GetByIdBook(model.IdDauSach); //Sai - hàm trả về phải là 1 list Sach. Hiện tại đang lấy 1 cuốn
            SachViewModels modelSach = new SachViewModels()
            {
                IdDauSach = sach.IdDauSach,
                TenSach = sach.TenSach
            };

            FileManager barcode = new FileManager();
            string barCodePath = barcode.CreateBarCode(modelSach.IdDauSach, modelSach.IdDauSach);

            ViewData["BarCodePath"] = barCodePath;

            return View(modelSach);
        }
    }
}