using BiTech.Library.BLL.DBLogic;
using BiTech.Library.Common;
using BiTech.Library.Controllers.BaseClass;
using BiTech.Library.DTO;
using BiTech.Library.Helpers;
using BiTech.Library.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BiTech.Library.Controllers
{
    public class SearchController : BaseController
    {
        // GET: Search
        public ActionResult Index(KeySearchViewModel KeySearch,int? page)
        {
            SachLogic _SachLogic = new SachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            TheLoaiSachLogic _TheLoaiSachLogic = new TheLoaiSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            NhaXuatBanLogic _NhaXuatBanLogic = new NhaXuatBanLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            KeSachLogic _KeSachLogic = new KeSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            LanguageLogic _LanguageLogic = new LanguageLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            TacGiaLogic _TacGiaLogic = new TacGiaLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            SachTacGiaLogic _SachTacGiaLogic = new SachTacGiaLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            //var model = new ListBooksModel();

            var model = new List<BookView>();


            ViewBag.theLoaiSach = _TheLoaiSachLogic.GetAllTheLoaiSach();
            ViewBag.tacGia = _TacGiaLogic.GetAllTacGia();
            ViewBag.NXB = _NhaXuatBanLogic.GetAllNhaXuatBan();

            ViewBag.theLoaiSach_selected = KeySearch.TheLoaiSach ?? " ";
            ViewBag.tacGia_selected = KeySearch.TenTacGia ?? " ";
            ViewBag.NXB_selected = KeySearch.TenNXB ?? " ";
            ViewBag.SapXep_selected = KeySearch.SapXep ?? " ";

            var list = _SachLogic.getPageSach(KeySearch);
            foreach (var item in list)
            {
                var listTG = _SachTacGiaLogic.getListById(item.Id);

                string tenTG = "";
                foreach (var item2 in listTG)
                {
                    tenTG += _TacGiaLogic.GetByIdTG(item2.IdTacGia)?.TenTacGia + ", " ?? "";
                }
                tenTG = tenTG.Length == 0 ? "--" : tenTG.Substring(0, tenTG.Length - 2);

                // cập nhật model số lượng còn lại = sl còn lại - sl trong trạng thái không mượn được
                var numKhongMuonDuoc = MuonSachController.GetSoLuongSach(item.Id, Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
                item.SoLuongConLai = item.SoLuongConLai - numKhongMuonDuoc;

                BookView book = new BookView(item);
                book.TenSach = book.SachDTO.TenSach;
                book.MaKiemSoat = book.SachDTO.MaKiemSoat;
                book.CreateDateTime = book.SachDTO.CreateDateTime;
                book.NamXuatBan = book.SachDTO.NamXuatBan;
                //book.SachDTO
                book.Ten_TheLoai = _TheLoaiSachLogic.getById(item.IdTheLoai)?.TenTheLoai ?? "--";
                book.Ten_NhaXuatBan = _NhaXuatBanLogic.getById(item.IdNhaXuatBan)?.Ten ?? "--";
                book.Ten_KeSach = _KeSachLogic.getById(item.IdKeSach)?.TenKe ?? "--";
                book.Ten_NgonNgu = _LanguageLogic.GetById(item.IdNgonNgu)?.Ten ?? "--";
                book.Ten_TacGia = tenTG;
                
                model.Add(book);
            }

            //Sắp xếp
            if (KeySearch.SapXep == "1")
                model = model.OrderBy(_ => _.TenSach).ToList();
            if (KeySearch.SapXep == "2")
                model = model.OrderBy(_ => _.MaKiemSoat).ToList();
            if (KeySearch.SapXep == "3")
                model = model.OrderBy(_ => _.CreateDateTime).ToList();
            if (KeySearch.SapXep == "4")
                model = model.OrderBy(_ => _.NamXuatBan).ToList();

            return View(model.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult ThongTinChiTiet(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("Index");
            }

            SachLogic _SachLogic = new SachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);

            Sach sachDTO = _SachLogic.GetById(id);

            if (sachDTO == null)
            {
                return RedirectToAction("Index");
            }

            // cập nhật model số lượng còn lại = sl còn lại - sl trong trạng thái không mượn được
            var numKhongMuonDuoc = MuonSachController.GetSoLuongSach(sachDTO.Id, Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            sachDTO.SoLuongConLai = sachDTO.SoLuongConLai - numKhongMuonDuoc;

            SachUploadModel model = new SachUploadModel(sachDTO);
            return View(model);
        }

        public ActionResult NewBook()
        {
            return View();
        }
    }
}