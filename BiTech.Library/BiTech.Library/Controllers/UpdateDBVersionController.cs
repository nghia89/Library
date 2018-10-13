using BiTech.Library.BLL.DBLogic;
using BiTech.Library.Controllers.BaseClass;
using BiTech.Library.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BiTech.Library.Controllers
{
    public class UpdateDBVersionController : BaseController
    {
        // GET: UpdateDBVersion
        public ActionResult Index()
        {
            ViewBag.Successs = TempData["Successs"];
            ViewBag.UnSuccesss = TempData["UnSuccesss"];
            return View();                
        }
      
        public ActionResult UpdateVersion()
        {
            ThanhVienLogic _ThanhVienLogic = new ThanhVienLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            SachLogic _SachLogic = new SachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            SachTacGiaLogic _SachTacGiaLogic = new SachTacGiaLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            TacGiaLogic _TacGiaLogic = new TacGiaLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            TheLoaiSachLogic _TheLoaiSachLogic = new TheLoaiSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            SachTheLoaiLogic _SachTheLoaiLogic = new SachTheLoaiLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            PhieuNhapSachLogic _PhieuNhapSachLogic = new PhieuNhapSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            ChiTietNhapSachLogic _ChiTietNhapSachLogic = new ChiTietNhapSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            NhaXuatBanLogic _NhaXuatBanLogic = new NhaXuatBanLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            BoSuuTapLogic _BoSuuTapLogic = new BoSuuTapLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            SachCaBietLogic _SachCaBietLogic = new SachCaBietLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            ThongTinMuonSachLogic _ThongTinMuonSachLogic = new ThongTinMuonSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            TrangThaiSachLogic _TrangThaiSachLogic = new TrangThaiSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            DDCLogic _DDCLogic = new DDCLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            KeSachLogic _keSachLogic = new KeSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            LanguageLogic _LanguageLogic = new LanguageLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            PhieuXuatSachLogic _PhieuXuatSachLogic = new PhieuXuatSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            ThongTinThuVienLogic _thongTinThuVienLogic = new ThongTinThuVienLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            ChiTietXuatSachLogic _ChiTietXuatSachLogic = new ChiTietXuatSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            ChucVuLogic _chucVuLogic = new ChucVuLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);           
            SoLuongSachTrangThaiLogic _soLuongSachTrangThaiLogic= new SoLuongSachTrangThaiLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            try
            {
                _TacGiaLogic.UpdateDBVersion();
                _TheLoaiSachLogic.UpdateDBVersion();
                _NhaXuatBanLogic.UpdateDBVersion();
                _LanguageLogic.UpdateDBVersion();
                _PhieuNhapSachLogic.UpdateDBVersion();
                _PhieuXuatSachLogic.UpdateDBVersion();
                _ChiTietXuatSachLogic.UpdateDBVersion();
                _ChiTietNhapSachLogic.UpdateDBVersion();
                _BoSuuTapLogic.UpdateDBVersion();
                _keSachLogic.UpdateDBVersion();
                _SachTheLoaiLogic.UpdateDBVersion();
                _SachTacGiaLogic.UpdateDBVersion();
                _SachLogic.UpdateDBVersion();
                _SachCaBietLogic.UpdateDBVersion();
                _ThongTinMuonSachLogic.UpdateDBVersion();
                _TrangThaiSachLogic.UpdateDBVersion();
                _thongTinThuVienLogic.UpdateDBVersion();
                _ThanhVienLogic.UpdateDBVersion();
                _chucVuLogic.UpdateDBVersion();
                _soLuongSachTrangThaiLogic.UpdateDBVersion();
                _DDCLogic.UpdateDBVersion();
                TempData["Successs"] = "Cập nhật thành công!";
            }
            catch
            {
                TempData["UnSuccesss"] = "Cập nhật không thành công!";
                throw;
            }
            
            return RedirectToAction("Index");
        }

    }
}