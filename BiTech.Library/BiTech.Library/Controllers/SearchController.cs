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
        public ActionResult Index(KeySearchViewModel KeySearch, int? page)
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
            ViewBag.pageSize = pageSize;
            ViewBag.pages = pageNumber;
            //var model = new ListBooksModel();

            var model = new List<BookView>();


            ViewBag.theLoaiSach = _TheLoaiSachLogic.GetAllTheLoaiSach();
            ViewBag.tacGia = _TacGiaLogic.GetAllTacGia();
            ViewBag.NXB = _NhaXuatBanLogic.GetAllNhaXuatBan();

            ViewBag.theLoaiSach_selected = KeySearch.TheLoaiSach ?? " ";
            ViewBag.tacGia_selected = KeySearch.TenTacGia ?? " ";
            ViewBag.NXB_selected = KeySearch.TenNXB ?? " ";
            ViewBag.SapXep_selected = KeySearch.SapXep ?? " ";

            KeySearch.Keyword = new SachCommon().GetInfo(KeySearch.Keyword);
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
            if (KeySearch.SapXep == "11")
                model = model.OrderByDescending(_ => _.TenSach).ToList();
            if (KeySearch.SapXep == "2")
                model = model.OrderBy(_ => _.MaKiemSoat).ToList();
            if (KeySearch.SapXep == "22")
                model = model.OrderByDescending(_ => _.MaKiemSoat).ToList();
            if (KeySearch.SapXep == "3")
                model = model.OrderBy(_ => _.CreateDateTime).ToList();
            if (KeySearch.SapXep == "33")
                model = model.OrderByDescending(_ => _.CreateDateTime).ToList();
            if (KeySearch.SapXep == "4")
                model = model.OrderBy(_ => _.NamXuatBan).ToList();
            if (KeySearch.SapXep == "44")
                model = model.OrderByDescending(_ => _.NamXuatBan).ToList();

            return View(model.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult SearchDetail(KeySearchViewModel KeySearch, int? page)
        {
            SachLogic _SachLogic = new SachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            TheLoaiSachLogic _TheLoaiSachLogic = new TheLoaiSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            NhaXuatBanLogic _NhaXuatBanLogic = new NhaXuatBanLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            KeSachLogic _KeSachLogic = new KeSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            LanguageLogic _LanguageLogic = new LanguageLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            TacGiaLogic _TacGiaLogic = new TacGiaLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            SachTacGiaLogic _SachTacGiaLogic = new SachTacGiaLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            BoSuuTapLogic _BoSuuTapLogic = new BoSuuTapLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);

            ListBooksModel model = new ListBooksModel();

            TempData["KeSach"] = KeySearch.KeSach;      
            TempData["BoSuuTap"] = KeySearch.BoSuuTap;      
            TempData["FrmSearchHigh"] = KeySearch.FrmSearchHigh;

            #region //Keyword
            TempData["Keyword"] = KeySearch.Keyword;
            TempData["Keyword1"] = KeySearch.Keyword1;
            TempData["Keyword2"] = KeySearch.Keyword2;
            TempData["Keyword3"] = KeySearch.Keyword3;
            TempData["Keyword4"] = KeySearch.Keyword4;
            TempData["KeywordBasic"] = KeySearch.KeywordBasic;
            #endregion

            #region //ddlLoaiTimKiem
            if (KeySearch.ddlLoaiTimKiem0 != null)
                TempData["lLoaiTimKiem0"] = KeySearch.ddlLoaiTimKiem0;
            else
                TempData["lLoaiTimKiem0"] = "";

            if (KeySearch.ddlLoaiTimKiem1 != null)
                TempData["lLoaiTimKiem1"] = KeySearch.ddlLoaiTimKiem1;
            else
                TempData["lLoaiTimKiem1"] = "";

            if (KeySearch.ddlLoaiTimKiem2 != null)
                TempData["lLoaiTimKiem2"] = KeySearch.ddlLoaiTimKiem2;
            else
                TempData["lLoaiTimKiem2"] = "";

            if (KeySearch.ddlLoaiTimKiem3 != null)
                TempData["lLoaiTimKiem3"] = KeySearch.ddlLoaiTimKiem3;
            else
                TempData["lLoaiTimKiem3"] = "";

            if (KeySearch.ddlLoaiTimKiem4 != null)
                TempData["lLoaiTimKiem4"] = KeySearch.ddlLoaiTimKiem4;
            else
                TempData["lLoaiTimKiem4"] = "";
            #endregion

            #region //Condition (điều kiện)
            if (KeySearch.Condition != null)
                TempData["Condition"] = KeySearch.Condition;
            else
                TempData["Condition"] = "";

            if (KeySearch.Condition1 != null)
                TempData["Condition1"] = KeySearch.Condition1;
            else
                TempData["Condition1"] = "";

            if (KeySearch.Condition2 != null)
                TempData["Condition2"] = KeySearch.Condition2;
            else
                TempData["Condition2"] = "";

            if (KeySearch.Condition3 != null)
                TempData["Condition3"] = KeySearch.Condition3;
            else
                TempData["Condition3"] = "";

            if (KeySearch.Condition4 != null)
                TempData["Condition4"] = KeySearch.Condition4;
            else
                TempData["Condition4"] = "";
            #endregion

            #region //Operator (toán tử)
            if (KeySearch.dlOperator1 != null)
                TempData["dlOperator1"] = KeySearch.dlOperator1;
            else
                TempData["dlOperator1"] = "";

            if (KeySearch.dlOperator2 != null)
                TempData["dlOperator2"] = KeySearch.dlOperator2;
            else
                TempData["dlOperator2"] = "";

            if (KeySearch.dlOperator3 != null)
                TempData["dlOperator3"] = KeySearch.dlOperator3;
            else
                TempData["dlOperator3"] = "";

            if (KeySearch.dlOperator4 != null)
                TempData["dlOperator4"] = KeySearch.dlOperator4;
            else
                TempData["dlOperator4"] = "";
            #endregion


            int pageSize = 30;
            int pageNumber = (page ?? 1);
            ViewBag.pageSize = pageSize;
            ViewBag.pages = pageNumber;

            ViewBag.theLoaiSach = _TheLoaiSachLogic.GetAllTheLoaiSach();
            ViewBag.BoSuuTap = _BoSuuTapLogic.GetAll();
            ViewBag.NXB = _NhaXuatBanLogic.GetAllNhaXuatBan();
            ViewBag.KeSach = _KeSachLogic.GetAll();

            ViewBag.theLoaiSach_selected = KeySearch.TheLoaiSach ?? " ";
            ViewBag.tacGia_selected = KeySearch.TenTacGia ?? " ";
            ViewBag.NXB_selected = KeySearch.TenNXB ?? " ";
            ViewBag.SapXep_selected = KeySearch.SapXep ?? " ";

            var list = _SachLogic.getPageSachHighPerformance(KeySearch);
            ViewBag.number = list.Count();

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
                //var numKhongMuonDuoc =  MuonSachController.GetSoLuongSach(item.Id, userdata.MyApps[_AppCode].ConnectionString, userdata.MyApps[_AppCode].DatabaseName);
                //item.SoLuongConLai = item.SoLuongConLai - numKhongMuonDuoc;

                BookView book = new BookView(item);
                book.TenSach = book.SachDTO.TenSach;
                book.MaKiemSoat = book.SachDTO.MaKiemSoat;
                book.CreateDateTime = book.SachDTO.CreateDateTime;
                book.NamXuatBan = book.SachDTO.NamXuatBan;
                book.Ten_TheLoai = _TheLoaiSachLogic.getById(item.IdTheLoai)?.TenTheLoai ?? "--";
                book.Ten_NhaXuatBan = _NhaXuatBanLogic.getById(item.IdNhaXuatBan)?.Ten ?? "--";
                book.Ten_KeSach = _KeSachLogic.getById(item.IdKeSach)?.TenKe ?? "--";
                book.Ten_NgonNgu = _LanguageLogic.GetById(item.IdNgonNgu)?.Ten ?? "--";
                book.Ten_TacGia = tenTG;

                model.Books.Add(book);
            }

            //Sắp xếp
            if (KeySearch.SapXep == "1")
                model.Books = model.Books.OrderBy(_ => _.TenSach).ToList();
            if (KeySearch.SapXep == "11")
                model.Books = model.Books.OrderByDescending(_ => _.TenSach).ToList();
            if (KeySearch.SapXep == "2")
                model.Books = model.Books.OrderBy(_ => _.MaKiemSoat).ToList();
            if (KeySearch.SapXep == "22")
                model.Books = model.Books.OrderByDescending(_ => _.MaKiemSoat).ToList();
            if (KeySearch.SapXep == "3")
                model.Books = model.Books.OrderBy(_ => _.CreateDateTime).ToList();
            if (KeySearch.SapXep == "33")
                model.Books = model.Books.OrderByDescending(_ => _.CreateDateTime).ToList();
            if (KeySearch.SapXep == "4")
                model.Books = model.Books.OrderBy(_ => _.NamXuatBan).ToList();
            if (KeySearch.SapXep == "44")
                model.Books = model.Books.OrderByDescending(_ => _.NamXuatBan).ToList();

            return View(model.Books.ToPagedList(pageNumber, pageSize));
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

            SachUploadModel model = new SachUploadModel(sachDTO);
            return View(model);
        }

        public ActionResult NewBook()
        {
            return View();
        }
    }
}