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
    public class TraSachController : BaseController
    {
        static public List<MuonTraSachViewModel> list_ChuanBiTra = new List<MuonTraSachViewModel>();
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
                _thanhvienmodoe = list_user.Where(_ => _.MaSoThanhVien == new ThanhVienCommon().GetInfo(IdUser)).SingleOrDefault(); //Thành viên
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
            list_book = GetByIdUser(new ThanhVienCommon().GetInfo(IdUser));
            ViewBag.list_maThanhVien = list_user.Select(_ => _.MaSoThanhVien).Take(20).ToList();
            ViewBag.list_maSach = list_book.Select(_ => _.MaKiemSoat).Take(20).ToList();
            return View(list_book);
        }

        /// <summary>
        /// Lấy thông tin sách và cập nhật số lượng sách có thể cho mượn
        /// </summary>
        /// <param name="maSach"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetBook(string maSach, string IdUser, string NgayMuon = "", string NgayTra = "")
        {
            List<MuonTraSachViewModel> list_book = new List<MuonTraSachViewModel>();

            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return Json(null, JsonRequestBehavior.AllowGet); //RedirectToAction("LogOff", "Account");
            #endregion

            SachLogic _SachLogicLogic = new SachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

            //Lấy danh sach những đang mượn của user id
            List<MuonTraSachViewModel> list_book_team = GetByIdUser(new ThanhVienCommon().GetInfo(IdUser));

            //Nếu ngày mượn và ngày trả là ""
            if (NgayMuon == "" && NgayTra == "")
            {
                //nhập mã sách qua khung search
            }
            else
            {
                //Lấy đối tượng trong danh sách đang chuẩn bị trả theo ngày mượn và ngày trả
                list_book_team = list_book_team.Where(_ => _.NgayMuon == NgayMuon && _.NgayTra == NgayTra).ToList();
            }
            
            //Lấy item có ngày trả nhỏ nhất
            //OrderBy list theo NgayTra
            list_book = list_book_team.Where(_ => _.MaKiemSoat == new SachCommon().GetInfo(maSach)).OrderBy(_ => _.NgayTra).ToList();

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
               
        // 
        // POST: /TraSach/UpdateList_ChuanBiTra
        //
        [HttpPost]
        public JsonResult UpdateList_ChuanBiTra(List<MuonTraSachViewModel> List_newitem)
        {
            list_ChuanBiTra = List_newitem;
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Trả Sách - update row table ThongTinMuonSach (dữ liệu lấy từ list sách chuẩn bị trả)
        /// </summary>
        /// <param name="List_newitem"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdateListBook(List<MuonTraSachViewModel> List_newitem)
        {
            if (List_newitem == null)
                return Json(false, JsonRequestBehavior.AllowGet);

            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return Json(false, JsonRequestBehavior.AllowGet);
            #endregion

            ThongTinMuonSachLogic _ThongTinMuonSachLogic = new ThongTinMuonSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            SachLogic _SachLogic = new SachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            TrangThaiSachLogic _TrangThaiSachLogic = new TrangThaiSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            SoLuongSachTrangThaiLogic _SoLuongSachTrangThaiLogic = new SoLuongSachTrangThaiLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

            List<MuonTraSachViewModel> list_book = new List<MuonTraSachViewModel>();
            if (List_newitem.Count > 0)
            {
                foreach (MuonTraSachViewModel item in List_newitem)
                {
                    Sach _sach = _SachLogic.GetByMaMaKiemSoat(item.MaKiemSoat); //Lấy thông tin sách
                    ThongTinMuonSach team = new ThongTinMuonSach()
                    {
                        idUser = item.IdUser,
                        idSach = _sach.Id,
                        NgayTraThucTe = DateTime.Now,
                        NgayGioMuon = DateTime.ParseExact(item.NgayMuon, "dd/MM/yyyy", null),
                        NgayPhaiTra = DateTime.ParseExact(item.NgayTra, "dd/MM/yyyy", null),
                    };

                    //mỗi cuốn sách thì update vào table ThongTinMuonSach 1 row
                    for (int i = 0; i < int.Parse(item.SoLuong); i++)
                    {
                        ThongTinMuonSach item_TT = _ThongTinMuonSachLogic.getByThongTinMuonSach(team);
                        if (item_TT != null)
                        {
                            item_TT.DaTra = true;
                            item_TT.TrangThaiTra = item.TinhTrangSach;
                            item_TT.NgayTraThucTe = DateTime.Now;
                            //update or insert SoLuongSachTrangThai
                            TrangThaiSach _trangthai = _TrangThaiSachLogic.getById(item_TT.TrangThaiTra);
                            if (_trangthai != null)
                            {
                                //Trạng thái sách trả không thể cho mượn tiếp(hư hại)
                                //Thêm số lượng sách hư hại vào SoLuongSachTrangThai
                                //=>Số lượng sách có thể cho mượn = số lượng sách không hư - số lượng sách hư hại
                                if (_trangthai.TrangThai == false)
                                {
                                    SoLuongSachTrangThai sl_sach = _SoLuongSachTrangThaiLogic.getBy_IdSach_IdTT(item_TT.idSach, item_TT.TrangThaiTra);
                                    if (sl_sach == null)
                                    {
                                        //inset
                                        SoLuongSachTrangThai sl_sach_new = new SoLuongSachTrangThai()
                                        {
                                            IdSach = item_TT.idSach,
                                            IdTrangThai = item_TT.TrangThaiTra,
                                            SoLuong = 1
                                        };
                                        _SoLuongSachTrangThaiLogic.Insert(sl_sach_new);
                                    }
                                    else
                                    {
                                        //update
                                        sl_sach.SoLuong = sl_sach.SoLuong + 1;
                                        _SoLuongSachTrangThaiLogic.Update(sl_sach);
                                    }

                                    //Update lại số lượng sách có thể mượn
                                    //Lấy danh sách trạng thái true
                                    List<TrangThaiSach> _trangthai_true = _TrangThaiSachLogic.GetAllTT_True();
                                    foreach(TrangThaiSach _item_TTS_true in _trangthai_true)
                                    {
                                        //lấy SoLuongSachTrangThai
                                        SoLuongSachTrangThai sl_sach_true = _SoLuongSachTrangThaiLogic.getBy_IdSach_IdTT(item_TT.idSach, _item_TTS_true.Id);
                                        if(sl_sach_true != null)
                                        {
                                            //Số số lượng sách của trạng thái > 0 thì số lượng sách giảm 1
                                            if(sl_sach_true.SoLuong > 0)
                                            {
                                                sl_sach_true.SoLuong = sl_sach_true.SoLuong - 1;
                                                _SoLuongSachTrangThaiLogic.Update(sl_sach_true);
                                                break;
                                            }
                                        }
                                    }

                                }
                            }
                            _ThongTinMuonSachLogic.SuaTrangThai(item_TT);
                        }
                    }
                }
                list_book = GetByIdUser(List_newitem[0].IdUser);
                return Json(list_book, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Lấy list trạng thái sách
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetAllTrangThaiSach()
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return Json(false, JsonRequestBehavior.AllowGet);
            #endregion

            TrangThaiSachLogic _TrangThaiSachLogic = new TrangThaiSachLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

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
        private MuonTraSachViewModel toMuonTraSachViewModel(ThongTinMuonSach item, Sach _sach)
        {
            MuonTraSachViewModel kq = new MuonTraSachViewModel();
            kq.Id = item.Id;
            kq.IdUser = item.idUser;
            kq.MaKiemSoat = _sach.MaKiemSoat;
            kq.TenSach = _sach.TenSach;
            kq.SoLuong = "1";
            kq.NgayMuon = item.NgayGioMuon.ToString("dd/MM/yyyy");
            kq.NgayTra = item.NgayPhaiTra.ToString("dd/MM/yyyy");

            long ngaytra = item.NgayPhaiTra.Date.Ticks;
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
                if (list_maSach.FindIndex(_ => _.MaKiemSoat == _itemcheck.MaKiemSoat && _.NgayMuon == _itemcheck.NgayMuon && _.NgayTra == _itemcheck.NgayTra) > -1)
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
        private string GetSoLuongSach(string idSach)
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
            List<TrangThaiSach> _ListTrangThai_true = _TrangThaiSachLogic.GetAllTT_True(); //Lấy những trạng thái sách có thể cho mượn
            int soluongsach = 0;
            //Tính tổng trạng thái sách có thể cho mượn
            foreach (TrangThaiSach item_trangthai in _ListTrangThai_true)
            {
                SoLuongSachTrangThai sl_sach = _SoLuongSachTrangThaiLogic.getBy_IdSach_IdTT(_Sach.Id, item_trangthai.Id);
                if (sl_sach != null)
                    soluongsach = soluongsach + sl_sach.SoLuong;
            }
            //Số lượng sách còn lại có thể cho mượn = tổng sách có thế cho mượn - số sách hiện đang cho mượn
            soluongsach = soluongsach - _ThongTinMuonSachLogic.GetAll_ChuaTra_byIdSach(_Sach.Id);
            return soluongsach.ToString();
        }

        #endregion

    }
}