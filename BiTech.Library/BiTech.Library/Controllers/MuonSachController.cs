using BiTech.Library.BLL.BarCode_QR;
using BiTech.Library.BLL.DBLogic;
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
    public class MuonSachController : BaseController
    {
        public ActionResult Index(string IdUser)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            ThanhVienLogic _ThanhVienLogic = new ThanhVienLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            SachLogic _SachLogic = new SachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            //Danh sách
            List<ThanhVien> list_user = _ThanhVienLogic.GetAll();//Danh sách thành viên
            List<Sach> list_Sach = _SachLogic.getPageSach(new Common.KeySearchViewModel()); //Danh sách trong kho
            List<MuonTraSachViewModel> list_book = new List<MuonTraSachViewModel>(); //Danh sách book thành viên đang mượn

            //model
            ThanhVien _thanhvienmodoe = null;

            //ViewBag
            ViewBag.hasUser = false; // biến kiểm tra có tồn tại IdUser không
            ViewBag.ThongBao = false; //Có hiện thị thông báo hay không
            ViewBag.ThongBaoString = ""; //Nội dung thông báo
            ViewBag.user = null; // Giữ thông tin user nếu đăng nhập thành công
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
                _thanhvienmodoe = list_user.Where(_ => _.MaSoThanhVien == IdUser).SingleOrDefault(); //Thành viên
                if (_thanhvienmodoe != null)
                {
                    ViewBag.user = _thanhvienmodoe;
                    ViewBag.hasUser = true;
                }
                else
                {
                    ViewBag.ThongBao = true;
                    ViewBag.ThongBaoString = "Thành viên này không tồn tại";
                }
                #endregion
            }
            ViewBag.list_maThanhVien = list_user.Select(_ => _.MaSoThanhVien).Take(20).ToList();
            ViewBag.list_maSach = list_Sach.Select(_ => _.MaKiemSoat).Take(20).ToList();
            return View(list_book);
        }

        /// <summary>
        /// Lấy thông tin sách và cập nhật số lượng sách có thể cho mượn
        /// </summary>
        /// <param name="maSach"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetBook(string maSach)
        {
            List<MuonTraSachViewModel> list_book = new List<MuonTraSachViewModel>();

            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return Json(null, JsonRequestBehavior.AllowGet); //RedirectToAction("LogOff", "Account");
            #endregion
            SachLogic _SachLogicLogic = new SachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

            Sach _sach = _SachLogicLogic.GetByMaMaKiemSoat(maSach);
            if(_sach != null)
            {
                list_book.Add(new MuonTraSachViewModel()
                {
                    MaKiemSoat = _sach.MaKiemSoat,
                    TenSach = _sach.TenSach,
                    SoLuong = GetSoLuongSach(_sach.Id)
                });
            }

            return Json(list_book, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// Lấy danh sách những cuốn sách đang mượn theo IdUser
        /// </summary>
        /// <param name="IdUser"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetListBook_IdUser(string IdUser)
        {
            List<MuonTraSachViewModel> list_book = new List<MuonTraSachViewModel>();
            list_book = GetByIdUser(IdUser);
            return Json(list_book, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// insert row table ThongTinMuonSach (dữ liệu lấy từ list sách chuẩn bị mượn)
        /// </summary>
        /// <param name="List_newitem"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdateListBook(List<MuonTraSachViewModel> List_newitem)
        {
            if(List_newitem == null)
                return Json(false, JsonRequestBehavior.AllowGet);

            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return Json(false, JsonRequestBehavior.AllowGet);
            #endregion

            ThongTinMuonSachLogic _ThongTinMuonSachLogic = new ThongTinMuonSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            SachLogic _SachLogic = new SachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

            List<MuonTraSachViewModel> list_book = new List<MuonTraSachViewModel>();
            if (List_newitem.Count > 0)
            {
                foreach (MuonTraSachViewModel item in List_newitem)
                {
                    Sach _sach = _SachLogic.GetByMaMaKiemSoat(item.MaKiemSoat); //Lấy thông tin sách
                    //string a = DateTime.ParseExact(item.NgayTra, "dd/MM/yyyy", CultureInfo.InvariantCulture).Date.Ticks.ToString();
                    ThongTinMuonSach team = new ThongTinMuonSach()
                    {
                        idUser = item.IdUser,
                        idSach = _sach.Id,
                        NgayGioMuon = DateTime.Now,
                        NgayPhaiTra = DateTime.ParseExact(item.NgayTra, "dd/MM/yyyy", CultureInfo.InvariantCulture),//item.NgayTra,
                        DaTra = false,
                        NgayTraThucTe = DateTime.ParseExact("01/01/0001", "dd/MM/yyyy", null),
                    };

                    //mỗi cuốn sách thì thêm vào table ThongTinMuonSach 1 row
                    for (int i = 0; i< int.Parse(item.SoLuong); i++)
                    {
                        _ThongTinMuonSachLogic.Insert(team); //Thêm sách (Insert to database)
                    }
                }
                list_book = GetByIdUser(List_newitem[0].IdUser);
                return Json(list_book, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        #region Function

        /// <summary>
        /// Convert ThongTinMuonSach to MuonTraSachViewModel
        /// </summary>
        /// <param name="item"></param>
        /// <param name="_sach"></param>
        /// <returns></returns>
        public MuonTraSachViewModel toMuonTraSachViewModel(ThongTinMuonSach item, Sach _sach)
        {
            MuonTraSachViewModel kq = new MuonTraSachViewModel();
            kq.Id = item.Id;
            kq.IdUser = item.idUser;
            kq.MaKiemSoat = _sach.MaKiemSoat;
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
        public List<MuonTraSachViewModel> GetByIdUser(string IdUser)
        {
            List<MuonTraSachViewModel> list_book = new List<MuonTraSachViewModel>();

            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            #endregion

            SachLogic _SachLogic = new SachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            ThongTinMuonSachLogic _ThongTinMuonSachLogic = new ThongTinMuonSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            SoLuongSachTrangThaiLogic _SoLuongSachTrangThaiLogic = new SoLuongSachTrangThaiLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            TrangThaiSachLogic _TrangThaiSachLogic = new TrangThaiSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

            
            List<ThongTinMuonSach> list_TTMS = _ThongTinMuonSachLogic.GetAllIdUser_ChuaTra(IdUser); //Thông tin  mượn sách với IdUser (những sách chưa trả)
            List<MuonTraSachCheckViewTable> list_maSach = new List<MuonTraSachCheckViewTable>(); //Danh sách mã sách đã được thêm vào list_book

            foreach (ThongTinMuonSach item in list_TTMS)
            {
                Sach _Sach = _SachLogic.GetBookById(item.idSach); //lấy thông tin sách bằng idSach
                //Tạo đối tượng dùng kiểm tra 
                MuonTraSachCheckViewTable _itemcheck = new MuonTraSachCheckViewTable()
                {
                    MaKiemSoat = _Sach.MaKiemSoat,
                    NgayMuon = item.NgayGioMuon.ToString("dd/MM/yyyy"),
                    NgayTra = item.NgayPhaiTra.ToString("dd/MM/yyyy"),
                };

                //kiểm tra đối tượng đã tồn tại trong list_maSach chưa
                if (list_maSach.FindIndex(_=>_.MaKiemSoat == _itemcheck.MaKiemSoat && _.NgayMuon == _itemcheck.NgayMuon && _.NgayTra == _itemcheck.NgayTra) > -1 )
                {
                    //đã tồn tại update số lượng
                    MuonTraSachViewModel team = list_book.Where(_ => _.MaKiemSoat == _itemcheck.MaKiemSoat && _.NgayMuon == _itemcheck.NgayMuon && _.NgayTra == _itemcheck.NgayTra).SingleOrDefault();
                    team.SoLuong = (int.Parse(team.SoLuong) + 1).ToString();
                }
                else
                {
                    //Chưa tồn tại thì thêm vào list
                    list_maSach.Add(_itemcheck);
                    MuonTraSachViewModel mtsach = toMuonTraSachViewModel(item, _Sach);
                    mtsach.SoLuongMax = GetSoLuongSach(_Sach.Id);
                    list_book.Add(mtsach);
                }
            }
            return list_book;
        }

        /// <summary>
        /// Lấy số lượng sách có thể cho mượn theo idSach (SoLuongMax)
        /// </summary>
        /// <param name="idSach"></param>
        /// <returns></returns>
        public string GetSoLuongSach(string idSach)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return "";
            #endregion

            SachLogic _SachLogic = new SachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            SoLuongSachTrangThaiLogic _SoLuongSachTrangThaiLogic = new SoLuongSachTrangThaiLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            TrangThaiSachLogic _TrangThaiSachLogic = new TrangThaiSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            ThongTinMuonSachLogic _ThongTinMuonSachLogic = new ThongTinMuonSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

            Sach _Sach = _SachLogic.GetBookById(idSach);
            int soluongsach = 0;
            int soluongsach_true = 0;
            int soluongsach_false = 0;

            //Tính tổng trạng thái sách có thể cho mượn
            List<TrangThaiSach> _ListTrangThai_true = _TrangThaiSachLogic.GetAllTT_True(); //Lấy những trạng thái sách có thể cho mượn
            foreach (TrangThaiSach item_trangthai in _ListTrangThai_true)
            {
                SoLuongSachTrangThai sl_sach = _SoLuongSachTrangThaiLogic.getBy_IdSach_IdTT(_Sach.Id, item_trangthai.Id);
                if(sl_sach != null)
                    soluongsach_true = soluongsach_true + sl_sach.SoLuong;
            }
            //Tính tổng trạng thái sách không thể cho mượn
            List<TrangThaiSach> _ListTrangThai_false = _TrangThaiSachLogic.GetAllTT_False(); //Lấy những trạng thái sách có thể cho mượn
            foreach (TrangThaiSach item_trangthai in _ListTrangThai_false)
            {
                SoLuongSachTrangThai sl_sach = _SoLuongSachTrangThaiLogic.getBy_IdSach_IdTT(_Sach.Id, item_trangthai.Id);
                if (sl_sach != null)
                    soluongsach_false = soluongsach_false + sl_sach.SoLuong;
            }
            //Số lượng sách còn có thể cho mượn (chưa tính những sách đang cho mượn)
            //soluongsach = soluongsach_true - soluongsach_false;
            //Số lượng sách còn lại có thể cho mượn thực tế = tổng sách có thế cho mượn - số sách hiện đang cho mượn
            soluongsach = soluongsach_true - _ThongTinMuonSachLogic.GetAll_ChuaTra_byIdSach(_Sach.Id);
            return soluongsach.ToString();
        } 

        #endregion
    }
}