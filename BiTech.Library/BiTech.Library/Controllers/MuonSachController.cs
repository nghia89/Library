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
    public class MuonSachController : BaseController
    {
        static public List<MuonTraSachViewModel> list_ChuanBiMuon = new List<MuonTraSachViewModel>();

        public ActionResult Index(string IdUser, string flagResult)
        {
            ThanhVienLogic _ThanhVienLogic = new ThanhVienLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            SachLogic _SachLogic = new SachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            //Danh sách
            List<ThanhVien> list_user = _ThanhVienLogic.GetAllActive();//Danh sách thành viên Active
            List<Sach> list_Sach = _SachLogic.getPageSach(new Common.KeySearchViewModel()); //Danh sách trong kho
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
                    ThanhVien user_DeActive = _ThanhVienLogic.GetByMaSoThanhVienDeActive(ThanhVienCommon.GetInfo(IdUser));//Thành viên DeActive
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

            ViewBag.list_maThanhVien = list_user.Select(_ => _.MaSoThanhVien + "-" + _.Ten).ToList();
            //ViewBag.list_maSach = list_Sach.Select(_ => _.MaKiemSoat + "-" + _.TenSach).ToList();

            if (list_ChuanBiMuon != null)
            {
                list_book = list_ChuanBiMuon.OrderBy(_ => _.NgayMuon).ToList();
            }
            return View(list_book);
        }

        // Lấy thông tin sách và cập nhật số lượng sách có thể cho mượn
        // Nơi gọi: Scripts->LibraryAngularJS.js->MuonSachCtrlr->GetBook
        // POST: MuonSach/GetBook
        [HttpPost]
        public JsonResult GetBook(string maSach)
        {
            List<MuonTraSachViewModel> list_book = new List<MuonTraSachViewModel>();

            SachLogic _SachLogicLogic = new SachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            SachCaBietLogic _SachCaBietLogic = new SachCaBietLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            ThongTinMuonSachLogic _ThongTinMuonSachLogic = new ThongTinMuonSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            TrangThaiSachLogic _TrangThaiSachLogic = new TrangThaiSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            SachCaBiet _SachCaBiet = _SachCaBietLogic.GetByMaKSCBorMaCaBienCu(new SachCommon().GetInfo(maSach));
            

            if (_SachCaBiet != null)
            {
                TrangThaiSach _tt = _TrangThaiSachLogic.getById(_SachCaBiet.IdTrangThai);
                if(_tt != null)
                {
                    if (_tt.TrangThai)
                    {
                        Sach _Sach = _SachLogicLogic.GetBookById(_SachCaBiet.IdSach);
                        list_book.Add(new MuonTraSachViewModel()
                        {
                            MaKiemSoat = _SachCaBiet.MaKSCB.Replace(".", "_"),
                            MaKSCB = _SachCaBiet.MaKSCB,
                            TenSach = _Sach.TenSach
                        });
                    }
                }                

                List<ThongTinMuonSach> List_SachDuocMuon = _ThongTinMuonSachLogic.GetAllbyIdSachCaBiet_ChuaTra(_SachCaBiet.Id);
                
                MuonTraSachViewModel Sach_ChuanBiMuon = (list_ChuanBiMuon != null)?list_ChuanBiMuon.Find(_=>_.MaKSCB == _SachCaBiet.MaKSCB):null; //sách đã tồn tại trong danh sách chuẩn bị mượn chưa

                if (List_SachDuocMuon.Count > 0 || Sach_ChuanBiMuon != null)
                {
                    list_book.Clear();
                }
            }

            return Json(list_book, JsonRequestBehavior.AllowGet);
        }

        // Lấy danh sách những cuốn sách đang mượn theo IdUser
        // Nơi gọi: Scripts->LibraryAngularJS.js->MuonSachCtrlr->GetListBook
        // POST: MuonSach/GetListBook_IdUser
        [HttpPost]
        public JsonResult GetListBook_IdUser(string IdUser)
        {
            List<MuonTraSachViewModel> list_book = new List<MuonTraSachViewModel>();

            list_book = GetByIdUser(ThanhVienCommon.GetInfo(IdUser), Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);

            return Json(list_book, JsonRequestBehavior.AllowGet);
        }

        // Lấy danh sách những cuốn sách cá biệt bằng keyword
        // Nơi gọi: Views->MuonSach->index.cshtml->AddAutoComplete
        // POST: MuonSach/GetListBook_IdUser
        [HttpPost]
        public JsonResult GetAllListById(string maSach)
        {
            SachLogic _SachLogic = new SachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            SachCaBietLogic _SachCaBietLogic = new SachCaBietLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            List<SachCaBiet> list_Sach_CaBiet = _SachCaBietLogic.GetAllByMaKSCBorMaCaBienCu(maSach);
            foreach(SachCaBiet item in list_Sach_CaBiet)
            {
                item.TenSach = _SachLogic.GetBookById(item.IdSach).TenSach;
            }
            var list = list_Sach_CaBiet.Select(_ => _.MaKSCB + "-" + _.TenSach).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        // insert row table ThongTinMuonSach (dữ liệu lấy từ list sách chuẩn bị mượn)
        // Nơi gọi: Scripts->LibraryAngularJS.js->MuonSachCtrlr->MuonSach
        // POST: MuonSach/UpdateListBook
        [HttpPost]
        public JsonResult UpdateListBook(List<MuonTraSachViewModel> List_newitem)
        {
            if (List_newitem == null)
                return Json(false, JsonRequestBehavior.AllowGet);

            ThongTinMuonSachLogic _ThongTinMuonSachLogic = new ThongTinMuonSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            SachLogic _SachLogic = new SachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            SachCaBietLogic _SachCaBietLogic = new SachCaBietLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            List<MuonTraSachViewModel> list_book = new List<MuonTraSachViewModel>();
            if (List_newitem.Count > 0)
            {
                foreach (MuonTraSachViewModel item in List_newitem)
                {
                    SachCaBiet _SachCaBiet = _SachCaBietLogic.GetByMaKSCBorMaCaBienCu(item.MaKSCB);
                    Sach _sach = _SachLogic.GetByMaMaKiemSoat(item.MaKiemSoat.Split('_')[0]); //Lấy thông tin sách
                    if(_sach != null)
                        _sach.SoLuongConLai--; 
                    if (_SachCaBiet != null)
                    {
                        ThongTinMuonSach team = new ThongTinMuonSach()
                        {
                            idUser = ThanhVienCommon.GetInfo(item.IdUser),
                            idSach = _SachCaBiet.IdSach,
                            IdSachCaBiet = _SachCaBiet.Id,
                            NgayGioMuon = DateTime.Now,
                            NgayPhaiTra = DateTime.ParseExact(item.NgayTra, "dd/MM/yyyy", CultureInfo.InvariantCulture),//item.NgayTra,
                            DaTra = false,
                            NgayTraThucTe = DateTime.ParseExact("01/01/0001", "dd/MM/yyyy", null),
                        };
                        _ThongTinMuonSachLogic.Insert(team); //Thêm sách (Insert to database)
                        _SachLogic.Update(_sach);
                    }
                }
                list_book = GetByIdUser(ThanhVienCommon.GetInfo(List_newitem[0].IdUser), Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
                return Json(list_book, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        // ChuanBiTra
        // Nơi gọi: Scripts->LibraryAngularJS.js->MuonSachCtrlr->UpdateList_ChuanBiMuon
        // POST: /MuonSach/UpdateList_ChuanBiMuon
        [HttpPost]
        public JsonResult UpdateList_ChuanBiMuon(List<MuonTraSachViewModel> List_newitem)
        {
            list_ChuanBiMuon = List_newitem;
            return Json(true, JsonRequestBehavior.AllowGet);
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
            kq.MaKiemSoat = _SachCaBiet.MaKSCB;
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
        internal static List<MuonTraSachViewModel> GetByIdUser(string IdUser, string connectionString, string databaseName)
        {
            List<MuonTraSachViewModel> list_book = new List<MuonTraSachViewModel>();

            SachLogic _SachLogic = new SachLogic(connectionString, databaseName);
            ThongTinMuonSachLogic _ThongTinMuonSachLogic = new ThongTinMuonSachLogic(connectionString, databaseName);
            SachCaBietLogic _SachCaBietLogic = new SachCaBietLogic(connectionString, databaseName);
            TrangThaiSachLogic _TrangThaiSachLogic = new TrangThaiSachLogic(connectionString, databaseName);

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