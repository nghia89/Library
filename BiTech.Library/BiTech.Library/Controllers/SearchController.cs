﻿using BiTech.Library.BLL.DBLogic;
using BiTech.Library.Common;
using BiTech.Library.DTO;
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
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            SachLogic _SachLogic = new SachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            TheLoaiSachLogic _TheLoaiSachLogic = new TheLoaiSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            NhaXuatBanLogic _NhaXuatBanLogic = new NhaXuatBanLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            KeSachLogic _KeSachLogic = new KeSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            LanguageLogic _LanguageLogic = new LanguageLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            TacGiaLogic _TacGiaLogic = new TacGiaLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            SachTacGiaLogic _SachTacGiaLogic = new SachTacGiaLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            //var model = new ListBooksModel();

            var model = new List<BookView>();


            ViewBag.theLoaiSach = _TheLoaiSachLogic.GetAllTheLoaiSach();
            ViewBag.tacGia = _TacGiaLogic.GetAllTacGia();
            ViewBag.NXB = _NhaXuatBanLogic.GetAllNhaXuatBan();

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
                var numKhongMuonDuoc = MuonSachController.GetSoLuongSach(item.Id, userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
                item.SoLuongConLai = item.SoLuongConLai - numKhongMuonDuoc;

                BookView book = new BookView(item);

                //book.SachDTO
                book.Ten_TheLoai = _TheLoaiSachLogic.getById(item.IdTheLoai)?.TenTheLoai ?? "--";
                book.Ten_NhaXuatBan = _NhaXuatBanLogic.getById(item.IdNhaXuatBan)?.Ten ?? "--";
                book.Ten_KeSach = _KeSachLogic.getById(item.IdKeSach)?.TenKe ?? "--";
                book.Ten_NgonNgu = _LanguageLogic.GetById(item.IdNgonNgu)?.Ten ?? "--";
                book.Ten_TacGia = tenTG;
                
                model.Add(book);
            }
            
            return View(model.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult ThongTinChiTiet(string id)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("Index");
            }

            SachLogic _SachLogic = new SachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

            Sach sachDTO = _SachLogic.GetById(id);

            if (sachDTO == null)
            {
                return RedirectToAction("Index");
            }

            // cập nhật model số lượng còn lại = sl còn lại - sl trong trạng thái không mượn được
            var numKhongMuonDuoc = MuonSachController.GetSoLuongSach(sachDTO.Id, userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            sachDTO.SoLuongConLai = sachDTO.SoLuongConLai - numKhongMuonDuoc;

            SachUploadModel model = new SachUploadModel(sachDTO);
            return View(model);
        }
    }
}