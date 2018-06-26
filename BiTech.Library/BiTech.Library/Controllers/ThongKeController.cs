using BiTech.Library.BLL.DBLogic;
using BiTech.Library.DTO;
using BiTech.Library.Helpers;
using BiTech.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
namespace BiTech.Library.Controllers
{
    public class ThongKeController : Controller
    {
        private ThongKeLogic _thongKeLogic;
        private ChiTietPhieuMuonLogic _chiTietPhieuMuonLogic;
        public ThongKeController()
        {
            _thongKeLogic = new ThongKeLogic(Tool.GetConfiguration("ConnectionString"), Tool.GetConfiguration("DatabaseName"));
            _chiTietPhieuMuonLogic = new ChiTietPhieuMuonLogic(Tool.GetConfiguration("ConnectionString"), Tool.GetConfiguration("DatabaseName"));
        }
        // GET: ThongKe
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult DanhSachMuon(int? page, string day, int? month, int? year)
        {
            int soSachDuocMuon = 0;
            int soSachChuaTra = 0;
            List<object> listSoLuongThanhVien = new List<object>();
            var listPhieuMuon = _thongKeLogic.ListPhieuMuon();
            string idThanhVienNew = null;
            List<Sach> listSach = new List<Sach>();
            List<ThanhVien> listThanhVien = new List<ThanhVien>();
            List<PhieuMuon> listAll = new List<PhieuMuon>();
            List<PhieuMuon> listMonthSelected = new List<PhieuMuon>();
            List<PhieuMuon> listDaySelected = new List<PhieuMuon>();
            List<object> listChaCuaSach = new List<object>();
            List<int> listSoLuongSach = new List<int>();
            DateTime ngayTraNull = DateTime.ParseExact("01-01-0001", "dd-MM-yyyy", null);// Datatime NULL trong mongoDB khi xuất ra có dạng "01-01-0001"                                                                                       
            foreach (var item in listPhieuMuon)
            {
                item.TrangThai = ETinhTrangPhieuMuon.none;
                // ---------------------SẮP XẾP TRẠNG THÁI-------------------------
                // Ngày phải trả <= ngày hiện tại và ngày trả == NULL là CHƯA TRẢ   
                if (item.NgayPhaiTra <= DateTime.Now && item.NgayTra == ngayTraNull && item.NgayTra != null)
                {
                    item.TrangThai = ETinhTrangPhieuMuon.ChuaTra;
                    item.TenTrangThai = "Chưa trả";
                    soSachChuaTra++;
                }
                // Ngày phải trả > ngày hiện tại và ngày trả == NULL là GẦN TRẢ                                      
                if (item.NgayPhaiTra > DateTime.Now && item.NgayTra == ngayTraNull && item.NgayTra != null)
                {
                    item.TrangThai = ETinhTrangPhieuMuon.GanTra;
                    item.TenTrangThai = "Gần trả";
                    TimeSpan ts = item.NgayPhaiTra - DateTime.Now;
                    item.SoNgayGiaoDong = ts.Days;
                }
                // -- cong Phieu Tra vo
                // --------  
                // Ngày phải trả <= ngày hiện tại và ngày trả <= ngày phải trả là TRẢ ĐÚNG HẸN
                if (item.NgayPhaiTra <= DateTime.Now)
                {
                    if (item.NgayTra != ngayTraNull && item.NgayTra != null && item.NgayTra != null && item.NgayTra <= item.NgayPhaiTra)
                    {
                        item.TrangThai = ETinhTrangPhieuMuon.TraDungHen;
                        item.TenTrangThai = "Trả đúng hẹn";
                    }
                    // ngày trả > ngày phải trả là TRẢ TRỄ
                    if (item.NgayTra != ngayTraNull && item.NgayTra != null && item.NgayTra != null && item.NgayTra > item.NgayPhaiTra)
                    {
                        item.TrangThai = ETinhTrangPhieuMuon.TraTre;
                        item.TenTrangThai = "Trả trễ";
                        TimeSpan ts = ((DateTime)item.NgayTra) - item.NgayPhaiTra;
                        item.SoNgayGiaoDong = ts.Days;
                    }
                }
                // Ngày phải trả > ngày hiện tại và ngày trả <= ngày phải phải trả là TRẢ ĐÚNG HẸN
                if (item.NgayPhaiTra > DateTime.Now)
                {
                    if (item.NgayTra != ngayTraNull && item.NgayTra != null && item.NgayTra <= item.NgayPhaiTra)
                    {
                        item.TrangThai = ETinhTrangPhieuMuon.TraDungHen;
                        item.TenTrangThai = "Trả đúng hẹn";
                    }
                }

                // --------               
                //if (item.TrangThai == ETinhTrangPhieuMuon.ChuaTra || item.TrangThai == ETinhTrangPhieuMuon.GanTra)
                //{
                listAll.Add(item);
                if (day != null)
                {
                    if (item.NgayMuon == DateTime.Parse(day))
                    {
                        listMonthSelected.Add(item);
                    }
                }
                else
                {
                    if (item.NgayMuon.Month == month && item.NgayMuon.Year == year)
                    {
                        listMonthSelected.Add(item);
                    }
                }
                //}
            }

           // var _listPhieuMuon = listAll.OrderBy(x => x.TrangThai).ThenBy(x => x.NgayPhaiTra);
            var _listPhieuMuon = listMonthSelected.OrderBy(x => x.TrangThai).ThenBy(x => x.NgayPhaiTra);
            bool flat = false;// Kiểm tra xem mã Thành Viên đã tồn tại chưa 
            foreach (var item in _listPhieuMuon)
            {
                // Đếm số lượng người mượn sách
                idThanhVienNew = item.IdUser;
                if (listSoLuongThanhVien.Count == 0)
                {
                    listSoLuongThanhVien.Add(idThanhVienNew);
                }
                else
                {
                    foreach (var x in listSoLuongThanhVien.ToList())
                    {
                        if (x.ToString() != idThanhVienNew.ToString())
                        {
                            flat = true;
                        }
                        else
                        {
                            flat = false;
                            break;
                        }
                    }
                }
                if (flat)
                {
                    listSoLuongThanhVien.Add(idThanhVienNew);
                }
                // --------------------THÔNG TIN SÁCH,THÀNH VIÊN--------------------
                // Lấy danh sách Thành Viên
                listThanhVien.Add(_thongKeLogic.GetThanhVienById(item.IdUser));
                // Lấy danh sách Chi Tiết Phiếu Mượn từ Id Phiếu Mượn

                var listCTPM = _thongKeLogic.GetCTPMById(item.Id);
                listSach = new List<Sach>();
                foreach (var i in listCTPM)
                {
                    // Lấy danh sách Sách từ Chi Tiết Phiếu Mượn
                    var sach = _thongKeLogic.GetSachByIdDauSach(i.IdSach);
                    listSach.Add(sach);
                    // số lượng sách được mượn
                    var soLuong = i.SoLuong != 0 ? i.SoLuong : 1;
                    soSachDuocMuon += soLuong;
                    listSoLuongSach.Add(soLuong);
                }
                listChaCuaSach.Add(listSach);
            }
            // Thông tin phiếu mượn
            ViewBag.SoLuongThanhVien = listSoLuongThanhVien.Count();
            ViewBag.SoSachDuocMuon = soSachDuocMuon;
            ViewBag.SoSachDangDuocMuon = listAll.Count;
            ViewBag.SoSachChuaTra = soSachChuaTra;
            ViewBag.ListChaCuaSach = listChaCuaSach;
            ViewBag.ListSoLuongSach = listSoLuongSach;
            ViewBag.ListThanhVien = listThanhVien;
            // Phân trang
            int pageSize = 20;
            int pageNumber = (page ?? 1);
            ViewBag.pageSize = pageSize;
            ViewBag.pages = pageNumber;
            // Get ngày, tháng, năm
            ViewBag.Day = day;
            ViewBag.Month = month;//!= null ? month : 1;
            ViewBag.Year = year;// != null ? year : DateTime.Now.Year;
            return View(_listPhieuMuon.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult DanhSachTra(int? page, string day, int? month, int? year)
        {
            int soSachDuocMuon = 0;
            var listPhieuMuon = _thongKeLogic.ListPhieuMuon();
            List<Sach> listSach = new List<Sach>();
            List<ThanhVien> listThanhVien = new List<ThanhVien>();
            DateTime ngayTraNull = DateTime.ParseExact("01-01-0001", "dd-MM-yyyy", null);// Datatime NULL trong mongoDB khi xuất ra có dạng "01-01-0001"
            List<PhieuMuon> listAll = new List<PhieuMuon>();
            List<PhieuMuon> listMonthSelected = new List<PhieuMuon>();
            List<int> listSoLuongSach = new List<int>();
            List<object> listChaCuaSach = new List<object>();

            foreach (var item in listPhieuMuon)
            {
                item.TrangThai = ETinhTrangPhieuMuon.none;

                // ---------------------SẮP XẾP TRẠNG THÁI-------------------------
                // Ngày phải trả <= ngày hiện tại và ngày trả <= ngày phải trả là TRẢ ĐÚNG HẸN
                if (item.NgayPhaiTra <= DateTime.Now)
                {
                    if (item.NgayTra != ngayTraNull && item.NgayTra != null && item.NgayTra != null && item.NgayTra <= item.NgayPhaiTra)
                    {
                        item.TrangThai = ETinhTrangPhieuMuon.TraDungHen;
                        item.TenTrangThai = "Trả đúng hẹn";
                    }
                    // ngày trả > ngày phải trả là TRẢ TRỄ
                    if (item.NgayTra != ngayTraNull && item.NgayTra != null && item.NgayTra != null && item.NgayTra > item.NgayPhaiTra)
                    {
                        item.TrangThai = ETinhTrangPhieuMuon.TraTre;
                        item.TenTrangThai = "Trả trễ";
                        TimeSpan ts = ((DateTime)item.NgayTra) - item.NgayPhaiTra;
                        item.SoNgayGiaoDong = ts.Days;
                    }
                }
                // Ngày phải trả > ngày hiện tại và ngày trả <= ngày phải phải trả là TRẢ ĐÚNG HẸN
                else
                {
                    if (item.NgayTra != ngayTraNull && item.NgayTra != null && item.NgayTra <= item.NgayPhaiTra)
                    {
                        item.TrangThai = ETinhTrangPhieuMuon.TraDungHen;
                        item.TenTrangThai = "Trả đúng hẹn";
                    }
                }
                //---------------
                if (item.TrangThai == ETinhTrangPhieuMuon.TraDungHen || item.TrangThai == ETinhTrangPhieuMuon.TraTre)
                {
                    listAll.Add(item);
                    if (day != null)
                    {
                        if (item.NgayTra == DateTime.Parse(day))
                        {
                            listMonthSelected.Add(item);
                        }
                    }
                    else
                    {
                        if (((DateTime)item.NgayTra).Month == month && ((DateTime)item.NgayTra).Year == year)
                        {
                            listMonthSelected.Add(item);
                        }
                    }
                }
            }
            var _listPhieuTra = listAll.OrderBy(x => x.TrangThai).ThenBy(x => x.NgayPhaiTra);
            // var _listPhieuTra = listMonthSelected.OrderBy(x => x.TrangThai).ThenBy(x => x.NgayPhaiTra);

            foreach (var item in _listPhieuTra)
            {
                // Đếm số lượng người trả sách

                // --------------------THÔNG TIN SÁCH,THÀNH VIÊN--------------------
                // Lấy danh sách Thành Viên
                listThanhVien.Add(_thongKeLogic.GetThanhVienById(item.IdUser));
                // Lấy danh sách Chi Tiết Phiếu Mượn từ Id Phiếu Mượn
                var listCTPM = _thongKeLogic.GetCTPMById(item.Id);
                listSach = new List<Sach>();
                foreach (var i in listCTPM)
                {
                    // Lấy danh sách Sách từ Chi Tiết Phiếu Mượn
                    var sach = _thongKeLogic.GetSachByIdDauSach(i.IdSach);
                    listSach.Add(sach);
                    // Số lượng sách được mượn
                    var soLuong = i.SoLuong != 0 ? i.SoLuong : 1;
                    soSachDuocMuon += soLuong;
                    listSoLuongSach.Add(soLuong);
                }
                listChaCuaSach.Add(listSach);
            }
            // Thông tin phiếu trả
            ViewBag.SoLuongSachMuon = listSoLuongSach;
            ViewBag.ListChaCuaSach = listChaCuaSach;
            ViewBag.ListThanhVien = listThanhVien;
            // Phân trang
            int pageSize = 20;
            int pageNumber = (page ?? 1);
            ViewBag.pageSize = pageSize;
            ViewBag.pages = pageNumber;
            // Get ngày, tháng, năm
            ViewBag.Day = day;
            ViewBag.Month = month != null ? month : 1;
            ViewBag.Year = year != null ? year : DateTime.Now.Year;
            return View(_listPhieuTra.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult ThongTinDocGia()
        {
            return View();
        }

    }
}