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
                _thanhvienmodoe = list_user.Where(_ => _.MaSoThanhVien == new ThanhVienCommon().GetInfo(IdUser)).SingleOrDefault(); //Thành viên
                if (_thanhvienmodoe != null)
                {
                    ViewBag.user = _thanhvienmodoe;
                    ViewBag.hasUser = true;
                }
                else
                {
                    ThanhVien user_DeActive = _ThanhVienLogic.GetByMaSoThanhVienDeActive(new ThanhVienCommon().GetInfo(IdUser));//thành viên DeActive
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
            list_book = GetByIdUser(new ThanhVienCommon().GetInfo(IdUser));
            ViewBag.list_maThanhVien = list_user.Select(_ => _.MaSoThanhVien).ToList();
            //ViewBag.list_maSach = list_book.Select(_ => _.MaKiemSoat + "-" + _.TenSach).ToList();
            //var list = list_book.GroupBy(_ => new { _.MaKiemSoat, _.TenSach } );
            ViewBag.list_maSach = list_book.GroupBy(_ => new { _.MaKiemSoat, _.TenSach }).Select(group => group.Key).Select(_ => _.MaKiemSoat + "-" + _.TenSach).ToList();

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

        /// <summary>
        /// Lấy thông tin sách và cập nhật số lượng sách có thể cho mượn
        /// </summary>
        /// <param name="maSach"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetBook(string maSach, string IdUser, string NgayMuon = "", string NgayTra = "")
        {
            List<MuonTraSachViewModel> list_book = new List<MuonTraSachViewModel>();

            SachLogic _SachLogicLogic = new SachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);

            //Lấy danh sach những đang mượn của user id
            List<MuonTraSachViewModel> list_book_team = GetByIdUser(new ThanhVienCommon().GetInfo(IdUser));

            //Nếu ngày mượn và ngày trả là ""
            if (NgayMuon == "" && NgayTra == "")
            {
                //nhập mã sách qua khung search
                list_book_team = GetDanhSachCoTheTra(list_book_team);
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

        // Lấy danh sách những cuốn sách đang mượn theo IdUser
        // POST: GiaHan/GetListBook_IdUser
        [HttpPost]
        public JsonResult GetListBook_IdUser(string IdUser)
        {
            List<MuonTraSachViewModel> list_book = new List<MuonTraSachViewModel>();
            list_book = GetByIdUser(new ThanhVienCommon().GetInfo(IdUser));
            return Json(list_book, JsonRequestBehavior.AllowGet);
        }

        // ChuanBiTra
        // POST: /GiaHan/UpdateList_ChuanBiTra
        [HttpPost]
        public JsonResult UpdateList_ChuanBiTra(List<MuonTraSachViewModel> List_newitem)
        {
            list_ChuanBiTra = List_newitem;
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        // Gia hạn - update row table ThongTinMuonSach (dữ liệu lấy từ list sách chuẩn bị trả)
        // POST: /TraSach/UpdateListBook
        [HttpPost]
        public JsonResult UpdateListBook(List<MuonTraSachViewModel> List_newitem)
        {
            if (List_newitem == null)
                return Json(false, JsonRequestBehavior.AllowGet);

            ThongTinMuonSachLogic _ThongTinMuonSachLogic = new ThongTinMuonSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            SachLogic _SachLogic = new SachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            TrangThaiSachLogic _TrangThaiSachLogic = new TrangThaiSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            SoLuongSachTrangThaiLogic _SoLuongSachTrangThaiLogic = new SoLuongSachTrangThaiLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);

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
                        ThongTinMuonSach item_TTMS = _ThongTinMuonSachLogic.getByThongTinMuonSach(team);
                        if (item_TTMS != null)
                        {
                            //update ngày phải trả
                            item_TTMS.NgayPhaiTra = DateTime.ParseExact(item.NgayTraNew, "dd/MM/yyyy", null);
                            _ThongTinMuonSachLogic.Update(item_TTMS);
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
            TrangThaiSachLogic _TrangThaiSachLogic = new TrangThaiSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            List<TrangThaiSach> list_TT = _TrangThaiSachLogic.GetAll();
            return Json(list_TT, JsonRequestBehavior.AllowGet);
        }

        #region Function

        /// <summary>
        /// Danh sách còn sách để trả
        /// </summary>
        /// <param name="List_item">list đang mượn</param>
        /// <returns>List result = list đang mượn - list chuẩn bị trả</returns>
        private List<MuonTraSachViewModel> GetDanhSachCoTheTra(List<MuonTraSachViewModel> List_item)
        {
            if (list_ChuanBiTra == null)
            {
                return List_item;
            }

            List<MuonTraSachViewModel> list_chuanbixoa = new List<MuonTraSachViewModel>(); //Danh sách item thoả điều kiện để xoá khỏi List_item
            //Kiểm tra item đã chọn hết sách để trả chưa
            foreach (MuonTraSachViewModel item in List_item)
            {
                MuonTraSachViewModel item_chuanbitra = list_ChuanBiTra.Where(_ => _.MaKiemSoat == item.MaKiemSoat
                                                                               && _.NgayMuon == item.NgayMuon
                                                                               && _.NgayTra == item.NgayTra).SingleOrDefault();
                if (item_chuanbitra != null)
                {
                    if (item.SoLuong == item_chuanbitra.SoLuong)
                    {
                        list_chuanbixoa.Add(item);
                    }
                }
            }
            foreach (MuonTraSachViewModel item in list_chuanbixoa)
            {
                List_item.Remove(item);
            }
            return List_item;
        }

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

            SachLogic _SachLogic = new SachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            ThongTinMuonSachLogic _ThongTinMuonSachLogic = new ThongTinMuonSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            SoLuongSachTrangThaiLogic _SoLuongSachTrangThaiLogic = new SoLuongSachTrangThaiLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            TrangThaiSachLogic _TrangThaiSachLogic = new TrangThaiSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);

            List<ThongTinMuonSach> list_TTMS = _ThongTinMuonSachLogic.GetAllIdUser_ChuaTra(IdUser); //Thông tin  mượn sách với IdUser (những sách chưa trả)
            List<MuonTraSachCheckViewTable> list_maSach = new List<MuonTraSachCheckViewTable>(); //Danh sách mã sách đã được thêm vào list_book

            foreach (ThongTinMuonSach item in list_TTMS)
            {
                Sach _Sach = _SachLogic.GetByID_IsDeleteFalse(item.idSach); //lấy thông tin sách bằng idSach
                if (_Sach == null)
                    continue;
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
            SachLogic _SachLogic = new SachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            SoLuongSachTrangThaiLogic _SoLuongSachTrangThaiLogic = new SoLuongSachTrangThaiLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            TrangThaiSachLogic _TrangThaiSachLogic = new TrangThaiSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            ThongTinMuonSachLogic _ThongTinMuonSachLogic = new ThongTinMuonSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);

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
            soluongsach = soluongsach - _ThongTinMuonSachLogic.Count_ChuaTra_byIdSach(_Sach.Id);
            return soluongsach.ToString();
        }

        #endregion

    }
}