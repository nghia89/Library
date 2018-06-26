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
        private PhieuMuonLogic _PhieuMuonLogic;
        private ChiTietPhieuMuonLogic _ChiTietPhieuMuonLogic;
        private PhieuTraLogic _PhieuTraLogic;
        private ChiTietPhieuTraLogic _ChiTietPhieuTraLogic;
        private SachLogic _SachLogic;
        private ThanhVienLogic _ThanhVienLogic;
        private TrangThaiSachLogic _TrangThaiSachLogic;
        private List<String> lstSach = new List<String>();
        public PhieuMuonController()
        {
            _PhieuMuonLogic = new PhieuMuonLogic(Tool.GetConfiguration("ConnectionString"), Tool.GetConfiguration("DatabaseName"));
            _ChiTietPhieuMuonLogic = new ChiTietPhieuMuonLogic(Tool.GetConfiguration("ConnectionString"), Tool.GetConfiguration("DatabaseName"));
            _PhieuTraLogic = new PhieuTraLogic(Tool.GetConfiguration("ConnectionString"), Tool.GetConfiguration("DatabaseName"));
            _ChiTietPhieuTraLogic = new ChiTietPhieuTraLogic(Tool.GetConfiguration("ConnectionString"), Tool.GetConfiguration("DatabaseName"));
            _SachLogic = new SachLogic(Tool.GetConfiguration("ConnectionString"), Tool.GetConfiguration("DatabaseName"));
            _ThanhVienLogic = new ThanhVienLogic(Tool.GetConfiguration("ConnectionString"), Tool.GetConfiguration("DatabaseName"));
            _TrangThaiSachLogic = new TrangThaiSachLogic(Tool.GetConfiguration("ConnectionString"), Tool.GetConfiguration("DatabaseName"));
        }
        // GET: PhieuMuon
        public ActionResult Index(string IdUser)
        {
            if (String.IsNullOrEmpty(IdUser))
                IdUser = "";
            ViewBag.IdUser = IdUser;
            return View();
        }
        public PartialViewResult _PartialPhieuMuon(string IdUser)
        {
            List<PhieuMuon> lstPhieuMuon = new List<PhieuMuon>();
            if (String.IsNullOrEmpty(IdUser))
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
            return PartialView(vmodel);
        }
        public ActionResult _CreatePhieuMuon()
        {
            ViewBag.Success = TempData["Success"];
            ViewBag.UnSuccess = TempData["UnSuccess"];
            ViewBag.SoLuong = TempData["SoLuong"];
            ViewBag.Date = TempData["Date"];
            return View();
        }
        /// <summary>
        /// Hàm kiểm tra số lượng mượn _ số lượng hiện có
        /// True = Được mượn, false = ngược lại
        /// </summary>
        /// <param name="idSach"></param>
        /// <param name="soLuong"></param>
        /// <returns></returns>
        public bool Validate(string idSach, int soLuong)
        {
            Sach sach = _SachLogic.GetByIdBook(idSach);
            List<ChiTietPhieuMuon> ctpm = _ChiTietPhieuMuonLogic.GetByIdBook(idSach);
            return soLuong <= (sach.SoLuong - ctpm.Count) ? true : false;
        }
        /// <summary>
        /// Insert vào 2 bảng PhieuMuon và ChiTietPhieuMuon và cập nhật lại số lượng sách bảng Sach
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _CreatePhieuMuon(PhieuMuonModelView viewModel)
        {
            try
            {
                //Fake du lieu trang thai sach
                //TrangThaiSach tts = new TrangThaiSach()
                //{
                //    TenTT = "Còn mới",
                //};

                //var result = _TrangThaiSachLogic.Insert(tts);
                //

                if (viewModel.NgayMuon < viewModel.NgayPhaiTra) //ngày mượn phải nho hơn ngày trả
                {
                    PhieuMuon modelPM = new PhieuMuon()
                    {
                        IdUser = viewModel.IdUser,
                        NgayMuon = DateTime.Now,
                        NgayPhaiTra = viewModel.NgayPhaiTra,
                        NgayTra = null,
                        TrangThaiPhieu = EPhieuMuon.ChuaTra, // mac dinh - Chua Tra
                        CreateDateTime = DateTime.Now
                    };
                    //Kiểm tra số lượng
                    //foreach (var item in viewModel.MaSach)
                    //{
                    //    if (!Validate(item, 1)) //kiem tra tung ma sach
                    //    {
                    //        TempData["SoLuong"] = "Số lượng sách mượn không phù hợp";
                    //        return View();
                    //    }
                    //}

                    //Insert bảng PhieuMuon
                    string idPhieuMuon = _PhieuMuonLogic.Insert(modelPM);
                    if (!String.IsNullOrEmpty(idPhieuMuon))
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
                            int i = 0;
                            foreach (string lstIdSach in viewModel.MaSach)
                            {
                                for(int j = 0; j<viewModel.MaSach.Count; j++)
                                {
                                    if (lstIdSach == viewModel.MaSach[j])
                                        i++;
                                }
                                modelCTPM.SoLuong = i; //so luong muon cua Ma Sach
                                modelCTPM.IdSach = lstIdSach; // Gắn mã sách vào 1 chitietphieumuon
                                                              //Insert bảng ChiTietPhieuMuon
                                string idCTPM = _ChiTietPhieuMuonLogic.Insert(modelCTPM);


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

                                if (!String.IsNullOrEmpty(idCTPM)) //true
                                {
                                    TempData["Success"] = "Tạo phiếu mượn thành công";
                                    return RedirectToAction("Index", "PhieuMuon");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            //Xoa data trong PhieuMuon khi insert ChiTiet bi loi
                            _PhieuMuonLogic.Remove(idPhieuMuon);
                            throw ex;
                        }

                    }
                    TempData["UnSuccess"] = "Tạo phiếu mượn thất bại";
                    return View();
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
            return View();
        }

        //Get
        public ActionResult _EditPhieuMuon(string id)
        {
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
        #region _Other
        [HttpGet]
        public JsonResult _GetBookItemById(string idBook)
        {
            JsonResult result = new JsonResult();
            if (!String.IsNullOrWhiteSpace(idBook))
            {
                result.Data = _SachLogic.GetByIdBook(idBook);
                //lstSach.Add(idBook); // add item to list to get list idSach - inset chitietphieumuon
                result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            }
            return result;
        }
        
        //public JsonResult _Remove(int index)
        //{
        //    JsonResult result = new JsonResult();
        //    if (!String.IsNullOrWhiteSpace(index.ToString()))
        //    {
        //        lstSach.RemoveAt(index);
        //        result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
        //    }
        //    return result ;
        //}

        public JsonResult GetName(string idUser)
        {
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
        #endregion

        public ActionResult ChiTietPhieuMuon(string id)
        {
            List<ChiTietPhieuMuon> lstChiTietPM = _ChiTietPhieuMuonLogic.GetByIdPhieuMuon(id); //Lấy danh sách chitiet thông qua id PhieuMuon
            List<ChiTietPMViewModel> modelChiTietPM = new List<ChiTietPMViewModel>();
            ChiTietPMViewModel _modelChiTietPM = new ChiTietPMViewModel();

            var phieuMuon = _PhieuMuonLogic.GetById(id); //id của phiếu mượn
            _modelChiTietPM.NgayMuon = phieuMuon.NgayMuon;

            foreach (var item in lstChiTietPM)
            {
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
    }
}