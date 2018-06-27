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
        private SachLogic _SachLogic;
        private ThanhVienLogic _ThanhVienLogic;
        private TrangThaiSachLogic _TrangThaiSachLogic;
        public PhieuTraController()
        {
            _PhieuMuonLogic = new PhieuMuonLogic(Tool.GetConfiguration("ConnectionString"), Tool.GetConfiguration("DatabaseName"));
            _PhieuTraLogic = new PhieuTraLogic(Tool.GetConfiguration("ConnectionString"), Tool.GetConfiguration("DatabaseName"));
            _ChiTietPhieuTraLogic = new ChiTietPhieuTraLogic(Tool.GetConfiguration("ConnectionString"), Tool.GetConfiguration("DatabaseName"));
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
            return RedirectToAction("Index", "PhieuMuon", new { @IdUser =  model.IdNguoiMuon});
        }

        /// <summary>
        /// Tạo phiếu trả sách
        /// HttpGet
        /// </summary>
        /// <returns></returns>
        public ActionResult TaoPhieuTra(string idPM)
        {
            PhieuMuon pm = _PhieuMuonLogic.GetById(idPM); // lay phieumuon thong qua idPM - lay idUser
            ThanhVien nguoiMuon = _ThanhVienLogic.GetByIdUser(pm.IdUser); //ten ngupoi muon
         
            PhieuTraViewModel model = new PhieuTraViewModel()
            {
                IdPM = idPM,
                IdNguoiMuon = nguoiMuon.MaSoThanhVien,
                NguoiMuon = nguoiMuon.Ten,
            };
            return View(model);
        }

        public ActionResult TraSach(string idPM, string idSach)
        {
            ViewBag.ListTinhTrangSach = _TrangThaiSachLogic.GetAll();
            PhieuMuon pm = _PhieuMuonLogic.GetById(idPM); // lay phieumuon thong qua idPM - lay idUser
            ThanhVien nguoiMuon = _ThanhVienLogic.GetByIdUser(pm.IdUser); //ten ngupoi muon

            Sach sach = _SachLogic.GetByIdBook(idSach); //lay ten sach
            PhieuTraViewModel model = new PhieuTraViewModel()
            {
                IdSach = idSach,
                IdPM = idPM,
                IdNguoiMuon = nguoiMuon.MaSoThanhVien,
                NguoiMuon = nguoiMuon.Ten,
                TenSach = sach.TenSach,                
            };
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult TraSach(PhieuTraViewModel model, string idPM, string idSach)
        {
            try
            {
                PhieuTra ptModel = new PhieuTra()
                {
                    IdPhieuMuon = idPM,
                    CreateDateTime = DateTime.Now,
                    NgayTra = DateTime.Now
                };
                // insert phieu tra
                string idPT = _PhieuTraLogic.Insert(ptModel);

                if(!String.IsNullOrEmpty(idPT))
                {
                    // insert chitietPT
                    try
                    {
                        ChiTietPhieuTra ctPTModel = new ChiTietPhieuTra()
                        {
                            IdPhieuTra = idPT,
                            IdSach = idSach,
                        };
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("ChiTietPhieuMuon", "PhieuMuon", new { @id = idPM });
        }
    }
}