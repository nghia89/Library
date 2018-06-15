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
    public class PhieuMuonController : Controller
    {
        private PhieuMuonLogic _PhieuMuonLogic;
        private ChiTietPhieuMuonLogic _ChiTietPhieuMuonLogic;
        private SachLogic _SachLogic;
        public PhieuMuonController()
        {
            _PhieuMuonLogic = new PhieuMuonLogic(Tool.GetConfiguration("ConnectionString"), Tool.GetConfiguration("DatabaseName"));
            _ChiTietPhieuMuonLogic = new ChiTietPhieuMuonLogic(Tool.GetConfiguration("ConnectionString"), Tool.GetConfiguration("DatabaseName"));
            _SachLogic = new SachLogic(Tool.GetConfiguration("ConnectionString"), Tool.GetConfiguration("DatabaseName"));
        }
        // GET: PhieuMuon
        public ActionResult Index()
        {
            return View();
        }
        public PartialViewResult _PartialPhieuMuon()
        {
            List<PhieuMuon> lstPhieuMuon = _PhieuMuonLogic.GetAll();
            return PartialView(lstPhieuMuon);
        }

        public ActionResult _CreatePhieuMuon()
        {
            ViewBag.Success = TempData["Success"];
            ViewBag.UnSuccess = TempData["UnSuccess"];
            ViewBag.SoLuong = TempData["SoLuong"];
            return View();
        }
        /// <summary>
        /// Hàm kiểm tra số lượng mượn _ số lượng hiện có
        /// True = Được mượn, false = ngược lại
        /// </summary>
        /// <param name="idSach"></param>
        /// <param name="soLuong"></param>
        /// <returns></returns>
        public bool Validate (string idSach, int soLuong)
        {
            //Sach sach = _SachLogic.GetById(idSach);
            //return sach.SoLuong >= soLuong ? true : false;

            return true;
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
                if (Validate(viewModel.MaSach, viewModel.SoLuong))
                {
                    PhieuMuon modelPM = new PhieuMuon()
                    {
                        IdUser = viewModel.IdUser,
                        NgayMuon = DateTime.Now,
                        NgayPhaiTra = viewModel.NgayPhaiTra,
                        TrangThaiPhieu = EPhieuMuon.ChuaTra, // mac dinh - Chua Tra
                        CreateDateTime = DateTime.Now
                    };
                    //Insert bảng PhieuMuon
                    string idPhieuMuon = _PhieuMuonLogic.Insert(modelPM);
                    if (!String.IsNullOrEmpty(idPhieuMuon))
                    {
                        ChiTietPhieuMuon modelCTPM = new ChiTietPhieuMuon()
                        {
                            IdPhieuMuon = idPhieuMuon,
                            IdSach = viewModel.MaSach,
                            SoLuong = viewModel.SoLuong,
                            CreateDateTime = DateTime.Now,
                        };
                        //Insert bảng ChiTietPhieuMuon
                        string idCTPM = _ChiTietPhieuMuonLogic.Insert(modelCTPM);
                        bool result = false;

                        #region Update bảng Sach
                        //// Update lại số lượng sách trong bảng sách
                        //Sach modelSach = _SachLogic.GetById(modelCTPM.IdSach);
                        //modelSach.SoLuong -= modelCTPM.SoLuong;
                        //result = _SachLogic.Update(modelSach); 

                        #endregion

                        if (!result)
                        {
                            //Fail              
                            TempData["UnSuccess"] = "Thêm mới thất bại";
                            return View();
                        }
                        else
                        {
                            TempData["Success"] = "Thêm mới thành công";
                            return RedirectToAction("Index", "PhieuMuon");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            TempData["SoLuong"] = "Số lượng sách mượn không phù hợp";
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
            model.TrangThaiPhieu = viewModel.TrangThaiPhieu;

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
            model.TrangThaiPhieu = EPhieuMuon.Deleted;
            bool result = _PhieuMuonLogic.Update(model);
            if (result == true)
            {
                TempData["Success"] = "Xóa thành công";
                return RedirectToAction("Index", "PhieuMuon");
            }
            else
                TempData["Success"] = "Xóa thất bại";
            return View();
        }
    }
}