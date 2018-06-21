using BiTech.Library.BLL.DBLogic;
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
        private PhieuTraLogic _PhieuTraLogic;
        private ChiTietPhieuTraLogic _ChiTietPhieuTraLogic;
        private SachLogic _SachLogic;
        private ThanhVienLogic _ThanhVienLogic;
        private TrangThaiSachLogic _TrangThaiSachLogic;
        public PhieuTraController()
        {
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

        public ActionResult _DanhSachPhieu(PhieuTraViewModel model)
        {
            
            return RedirectToAction("Index", "PhieuMuon", new { @IdUser =  model.IdNguoiMuon});
        }
    }
}