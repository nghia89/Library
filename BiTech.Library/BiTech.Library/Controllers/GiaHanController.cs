using BiTech.Library.BLL.BarCode_QR;
using BiTech.Library.BLL.DBLogic;
using BiTech.Library.Controllers.BaseClass;
using BiTech.Library.DTO;
using BiTech.Library.Helpers;
using BiTech.Library.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace BiTech.Library.Controllers
{
    [AuthorizeRoles(true, Role.CustomerAdmin, Role.CustomerUser)]
#if DEBUG
    [AllowAnonymous]
#endif
    public class GiaHanController : BaseController
    {
        static public List<MuonTraSachViewModel> list_ChuanBiTra = new List<MuonTraSachViewModel>();
        public ActionResult Index(string IdUser, string flagResult)
        {
            ThanhVienLogic _ThanhVienLogic = new ThanhVienLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            SachLogic _SachLogic = new SachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            //Danh sách
            List<ThanhVien> list_user = _ThanhVienLogic.GetAllActive();//Danh sách thành viên Active
            List<MuonTraSachViewModel> list_book = new List<MuonTraSachViewModel>(); //Danh sách book thành viên đang mượn

            //model
            ThanhVien _thanhvienmodoe = null;

            //ViewBag
            ViewBag.hasUser = false; // biến kiểm tra có tồn tại IdUser không
            ViewBag.ThongBao = false; //Có hiện thị thông báo hay không
            ViewBag.ThongBaoString = ""; //Nội dung thông báo
            ViewBag.user = null; // Giữ thông tin user nếu đăng nhập thành công
            ViewBag.flagResult = (flagResult == "true") ? true : false;
            ViewBag.MaxDate = 10;

            if (IdUser == null)
            {
                //load mặc định (chưa tìm kiếm)
            }
            else if (IdUser == "" || IdUser.Trim() == "")
            {
                //không có IdUser
                ViewBag.ThongBao = true;
                ViewBag.ThongBaoString = "Bạn hãy nhập mã thành viên";
            }
            else
            {
                #region Thành viên
                _thanhvienmodoe = list_user.Where(_ => _.MaSoThanhVien == ThanhVienCommon.GetInfo(IdUser)).SingleOrDefault(); //Thành viên
                if (_thanhvienmodoe != null)
                {
                    ViewBag.user = _thanhvienmodoe;
                    ViewBag.hasUser = true;
                }
                else
                {
                    ThanhVien user_DeActive = _ThanhVienLogic.GetByMaSoThanhVienDeActive(ThanhVienCommon.GetInfo(IdUser));//thành viên DeActive
                    if (user_DeActive != null)
                    {
                        ViewBag.ThongBao = true;
                        ViewBag.ThongBaoString = "Thành viên này đang bị khoá thẻ";
                    }
                    else
                    {
                        ViewBag.ThongBao = true;
                        ViewBag.ThongBaoString = "Thành viên này không tồn tại";
                    }
                }
                #endregion
            }
            list_book = GetByIdUser(ThanhVienCommon.GetInfo(IdUser));
            ViewBag.list_maThanhVien = list_user.Select(_ => _.MaSoThanhVien + "-" + _.Ten).ToList();
            //ViewBag.list_maSach = list_book.Select(_ => _.MaKiemSoat + "-" + _.TenSach).ToList();
            //var list = list_book.GroupBy(_ => new { _.MaKiemSoat, _.TenSach } );
            ViewBag.list_maSach = list_book.GroupBy(_ => new { _.MaKSCB, _.TenSach }).Select(group => group.Key).Select(_ => _.MaKSCB + "-" + _.TenSach).ToList();

            if (list_ChuanBiTra != null)
            {
                list_book = list_ChuanBiTra.OrderBy(_ => _.NgayMuon).ToList();
            }
            else
            {
                list_book.Clear();
            }
            return View(list_book);
        }

        // Lấy thông tin sách và cập nhật số lượng sách có thể cho mượn
        // Nơi gọi: Scripts->LibraryAngularJS.js->GiaHanCtrlr->GetBook
        // POST: GiaHan/GetBook
        [HttpPost]
        public JsonResult GetBook(string maSach, string IdUser, string NgayMuon = "", string NgayTra = "")
        {
            List<MuonTraSachViewModel> list_book = new List<MuonTraSachViewModel>();

            SachLogic _SachLogicLogic = new SachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            SachCaBietLogic _SachCaBietLogic = new SachCaBietLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);

            //Lấy danh sach những đang mượn của user id
            List<MuonTraSachViewModel> list_book_team = GetByIdUser(ThanhVienCommon.GetInfo(IdUser));

            SachCaBiet _SachCaBiet = _SachCaBietLogic.GetByMaKSCBorMaCaBienCu(new SachCommon().GetInfo(maSach));
            if(_SachCaBiet != null)
            {
                //OrderBy list theo NgayTra
                list_book = list_book_team.Where(_ => _.MaKSCB == _SachCaBiet.MaKSCB).OrderBy(_ => _.NgayTra).ToList();
                MuonTraSachViewModel Sach_ChuanBiGiaHan = (list_ChuanBiTra != null) ? list_ChuanBiTra.Find(_ => _.MaKSCB == _SachCaBiet.MaKSCB) : null;

                if (Sach_ChuanBiGiaHan != null)
                {
                    list_book.Clear();
                }
            }
            
            return Json(list_book, JsonRequestBehavior.AllowGet);

        }

        // Lấy danh sách những cuốn sách đang mượn theo IdUser
        // Nơi gọi: Scripts->LibraryAngularJS.js->GiaHanCtrlr->GetListBook
        // POST: GiaHan/GetListBook_IdUser
        [HttpPost]
        public JsonResult GetListBook_IdUser(string IdUser)
        {
            List<MuonTraSachViewModel> list_book = new List<MuonTraSachViewModel>();
            list_book = GetByIdUser(ThanhVienCommon.GetInfo(IdUser));
            return Json(list_book, JsonRequestBehavior.AllowGet);
        }

        // Lấy danh sách chuẩn bị gia hạn
        // Nơi gọi: Scripts->LibraryAngularJS.js->GiaHanCtrlr->UpdateListChuanBiTra
        // POST: /GiaHan/UpdateList_ChuanBiTra
        [HttpPost]
        public JsonResult UpdateList_ChuanBiTra(List<MuonTraSachViewModel> List_newitem)
        {
            list_ChuanBiTra = List_newitem;
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        // Gia hạn - update row table ThongTinMuonSach (dữ liệu lấy từ list sách chuẩn bị trả)
        // Nơi gọi: Scripts->LibraryAngularJS.js->GiaHanCtrlr->GiaHan
        // POST: /GiaHan/UpdateListBook
        [HttpPost]
        public JsonResult UpdateListBook(List<MuonTraSachViewModel> List_newitem)
        {
            if (List_newitem == null)
                return Json(false, JsonRequestBehavior.AllowGet);

            ThongTinMuonSachLogic _ThongTinMuonSachLogic = new ThongTinMuonSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            SachLogic _SachLogic = new SachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            SachCaBietLogic _SachCaBietLogic = new SachCaBietLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            TrangThaiSachLogic _TrangThaiSachLogic = new TrangThaiSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);

            List<MuonTraSachViewModel> list_book = new List<MuonTraSachViewModel>();
            if (List_newitem.Count > 0)
            {
                foreach (MuonTraSachViewModel item in List_newitem)
                {
                    SachCaBiet _SachCaBiet = _SachCaBietLogic.GetByMaKSCBorMaCaBienCu(item.MaKSCB);
                    ThongTinMuonSach team = new ThongTinMuonSach()
                    {
                        idUser = ThanhVienCommon.GetInfo(item.IdUser),
                        idSach = _SachCaBiet.IdSach,
                        IdSachCaBiet = _SachCaBiet.Id,
                        NgayGioMuon = DateTime.ParseExact(item.NgayMuon, "dd/MM/yyyy", null),
                        NgayPhaiTra = DateTime.ParseExact(item.NgayTra, "dd/MM/yyyy", null),
                    };
                    ThongTinMuonSach item_TTMS = _ThongTinMuonSachLogic.getByThongTinMuonSach(team);
                    //update ngày phải trả
                    item_TTMS.NgayPhaiTra = DateTime.ParseExact(item.NgayTraNew, "dd/MM/yyyy", null);
                    _ThongTinMuonSachLogic.Update(item_TTMS);
                }
                list_book = GetByIdUser(ThanhVienCommon.GetInfo(List_newitem[0].IdUser));
                return Json(list_book, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        // Lấy list trạng thái sách
        // Nơi gọi: Scripts->LibraryAngularJS.js->GiaHanCtrlr->GetAllTrangThaiSach
        // POST: /GiaHan/GetAllTrangThaiSach
        [HttpPost]
        public JsonResult GetAllTrangThaiSach()
        {
            TrangThaiSachLogic _TrangThaiSachLogic = new TrangThaiSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            List<TrangThaiSach> list_TT = _TrangThaiSachLogic.GetAll();
            return Json(list_TT, JsonRequestBehavior.AllowGet);
        }

        #region Function

        /// <summary>
        /// Convert ThongTinMuonSach to MuonTraSachViewModel
        /// </summary>
        /// <param name="item"></param>
        /// <param name="_sach"></param>
        /// <returns></returns>
        internal static MuonTraSachViewModel ToMuonTraSachViewModel(ThongTinMuonSach item, Sach _sach, SachCaBiet _SachCaBiet)
        {
            MuonTraSachViewModel kq = new MuonTraSachViewModel();
            kq.Id = item.Id;
            kq.IdUser = item.idUser;
            kq.MaKiemSoat = _SachCaBiet.MaKSCB.Replace(".","_");
            kq.MaKSCB = _SachCaBiet.MaKSCB;
            kq.TenSach = _sach.TenSach;
            kq.SoLuong = "1";
            kq.NgayMuon = item.NgayGioMuon.ToString("dd/MM/yyyy");
            kq.NgayTra = item.NgayPhaiTra.ToString("dd/MM/yyyy");
            long ngaytra = DateTime.ParseExact(kq.NgayTra, "dd/MM/yyyy", CultureInfo.InvariantCulture).Date.Ticks;
            long ngayhientai = DateTime.Now.Date.Ticks;
            kq.TinhTrang = ngaytra - ngayhientai < 0;
            return kq;
        }

        /// <summary>
        /// Lấy danh sách những sách đang mượn bằng idUser
        /// </summary>
        /// <param name="IdUser"></param>
        /// <returns></returns>
        private List<MuonTraSachViewModel> GetByIdUser(string IdUser)
        {
            List<MuonTraSachViewModel> list_book = new List<MuonTraSachViewModel>();

            SachLogic _SachLogic = new SachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            ThongTinMuonSachLogic _ThongTinMuonSachLogic = new ThongTinMuonSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            SachCaBietLogic _SachCaBietLogic = new SachCaBietLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            TrangThaiSachLogic _TrangThaiSachLogic = new TrangThaiSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);

            List<ThongTinMuonSach> list_TTMS = _ThongTinMuonSachLogic.GetAllIdUser_ChuaTra(IdUser); //Thông tin  mượn sách với IdUser (những sách chưa trả)
            List<MuonTraSachCheckViewTable> list_maSach = new List<MuonTraSachCheckViewTable>(); //Danh sách mã sách đã được thêm vào list_book

            foreach (ThongTinMuonSach item in list_TTMS)
            {
                SachCaBiet _SachCaBiet = _SachCaBietLogic.getById(item.IdSachCaBiet); //lấy thông tin sách cá biệt bằng idSach
                if (_SachCaBiet == null)
                    continue;
                Sach _Sach = _SachLogic.GetByID_IsDeleteFalse(_SachCaBiet.IdSach); //Lấy thông tin đầu sách
                MuonTraSachViewModel mtsach = ToMuonTraSachViewModel(item, _Sach, _SachCaBiet); // convert ThongTinMuonSach to MuonTraSachViewModel
                list_book.Add(mtsach);
            }
            return list_book;
        }

        #endregion

    }
}