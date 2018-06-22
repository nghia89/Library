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
            int soSachChuaTra = 0;
            var listPhieuMuon = _thongKeLogic.ListPhieuMuon();

            List<Sach> listSach = new List<Sach>();
            List<ThanhVien> listThanhVien = new List<ThanhVien>();
            List<PhieuMuon> listAll = new List<PhieuMuon>();
            List<PhieuMuon> listMonthSelected = new List<PhieuMuon>();
            List<PhieuMuon> listDaySelected = new List<PhieuMuon>();
            List<object> listChaCuaSach = new List<object>();
            DateTime ngayTraNull = DateTime.ParseExact("01-01-0001", "dd-MM-yyyy", null);// Datatime NULL trong mongoDB khi xuất ra có dạng "01-01-0001"
            var taii = listPhieuMuon.OrderBy(x => x.TrangThai).ThenBy(x => x.NgayPhaiTra);
            foreach (var item in listPhieuMuon)

            {
                item.TrangThai = ETinhTrangPhieuMuon.none;
                // ---------------------SẮP XẾP TRẠNG THÁI-------------------------
                // Ngày phải trả <= ngày hiện tại và ngày trả == NULL là CHƯA TRẢ   
                if (item.NgayPhaiTra <= DateTime.Now && item.NgayTra == ngayTraNull)
                {
                    item.TrangThai = ETinhTrangPhieuMuon.ChuaTra;
                    item.TenTrangThai = "Chưa trả";
                    soSachChuaTra++;
                }
                // Ngày phải trả > ngày hiện tại và ngày trả == NULL là GẦN TRẢ                                      
                if (item.NgayPhaiTra > DateTime.Now && item.NgayTra == ngayTraNull)
                {
                    item.TrangThai = ETinhTrangPhieuMuon.GanTra;
                    item.TenTrangThai = "Gần trả";
                    TimeSpan ts = item.NgayPhaiTra - DateTime.Now;
                    item.SoNgayGiaoDong = ts.Days;
                }
                // --------               
                if (item.TrangThai == ETinhTrangPhieuMuon.ChuaTra || item.TrangThai == ETinhTrangPhieuMuon.GanTra)
                {
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
                }
            }

            var _listPhieuMuon = listAll.OrderBy(x => x.TrangThai).ThenBy(x => x.NgayPhaiTra);
            //var _listPhieuMuon = listMonthSelected.OrderBy(x => x.TrangThai).ThenBy(x => x.NgayPhaiTra);
            List<int> listSoLuong = new List<int>();
            foreach (var item in _listPhieuMuon)
            {
                // --------------------THÔNG TIN SÁCH,THÀNH VIÊN--------------------
                // Lấy danh sách Thành Viên
                listThanhVien.Add(_thongKeLogic.GetThanhVienById(item.IdUser));
                // Lấy danh sách Chi Tiết Phiếu Mượn từ Id Phiếu Mượn

                var listCTPM = _thongKeLogic.GetCTPMById(item.Id);
                listSach = new List<Sach>();
                foreach (var i in listCTPM)
                {

                    // Lấy danh sách Sách từ Chi Tiết Phiếu Mượn
                    var sach = _thongKeLogic.GetSachById(i.IdSach);
                    listSach.Add(sach);
                    var soLuong = i.SoLuong != 0 ? i.SoLuong : 1;
                    listSoLuong.Add(soLuong);
                }
                listChaCuaSach.Add(listSach);
            }
            ViewBag.ListChaCuaSach = listChaCuaSach;

            ViewBag.ListSoLuong = listSoLuong;
            ViewBag.listSach = listSach;
            ViewBag.listThanhVien = listThanhVien;
            ViewBag.SoSachDangDuocMuon = listAll.Count;
            ViewBag.SoSachChuaTra = soSachChuaTra;
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            ViewBag.pageSize = pageSize;
            ViewBag.pages = pageNumber;
            ViewBag.Day = day;
            ViewBag.Month = month != null ? month : 1;
            ViewBag.Year = year != null ? year : DateTime.Now.Year;
            return View(_listPhieuMuon.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult DanhSachTra(int? page, string day, int? month, int? year)
        {
            var listPhieuMuon = _thongKeLogic.ListPhieuMuon();
            List<Sach> listSach = new List<Sach>();
            List<ThanhVien> listThanhVien = new List<ThanhVien>();
            DateTime ngayTraNull = DateTime.ParseExact("01-01-0001", "dd-MM-yyyy", null);// Datatime NULL trong mongoDB khi xuất ra có dạng "01-01-0001"
            List<PhieuMuon> listAll = new List<PhieuMuon>();
            List<PhieuMuon> listMonthSelected = new List<PhieuMuon>();
            foreach (var item in listPhieuMuon)
            {
                item.TrangThai = ETinhTrangPhieuMuon.none;
                // --------------------THÔNG TIN SÁCH,THÀNH VIÊN--------------------
                // Lấy danh sách Thành Viên
                listThanhVien.Add(_thongKeLogic.GetThanhVienById(item.IdUser));
                // Lấy danh sách Chi Tiết Phiếu Mượn từ Id Phiếu Mượn
                var listCTPM = _thongKeLogic.GetCTPMById(item.Id);
                foreach (var i in listCTPM)
                {
                    // Lấy danh sách Sách từ Chi Tiết Phiếu Mượn
                    var sach = _thongKeLogic.GetSachById(i.IdSach);
                    listSach.Add(sach);
                }
                // ---------------------SẮP XẾP TRẠNG THÁI-------------------------
                // Ngày phải trả <= ngày hiện tại và ngày trả <= ngày phải trả là TRẢ ĐÚNG HẸN
                if (item.NgayPhaiTra <= DateTime.Now)
                {
                    if (item.NgayTra != ngayTraNull && item.NgayTra <= item.NgayPhaiTra)
                    {
                        item.TrangThai = ETinhTrangPhieuMuon.TraDungHen;
                        item.TenTrangThai = "Trả đúng hẹn";
                    }
                    // ngày trả > ngày phải trả là TRẢ TRỄ
                    if (item.NgayTra != ngayTraNull && item.NgayTra > item.NgayPhaiTra)
                    {
                        item.TrangThai = ETinhTrangPhieuMuon.TraTre;
                        item.TenTrangThai = "Trả trễ";
                        TimeSpan ts = item.NgayTra - item.NgayPhaiTra;
                        item.SoNgayGiaoDong = ts.Days;
                    }
                }
                // Ngày phải trả > ngày hiện tại và ngày trả <= ngày phải phải trả là TRẢ ĐÚNG HẸN
                else
                {
                    if (item.NgayTra != ngayTraNull && item.NgayTra <= item.NgayPhaiTra)
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
                        if (item.NgayTra.Month == month && item.NgayTra.Year == year)
                        {
                            listMonthSelected.Add(item);
                        }
                    }
                }
            }
            var _listPhieuTra = listAll.OrderBy(x => x.TrangThai).ThenBy(x => x.NgayPhaiTra);
           // var _listPhieuTra = listMonthSelected.OrderBy(x => x.TrangThai).ThenBy(x => x.NgayPhaiTra);
            ViewBag.listSach = listSach;
            ViewBag.listThanhVien = listThanhVien;
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            ViewBag.pageSize = pageSize;
            ViewBag.pages = pageNumber;
            ViewBag.Day = day;
            ViewBag.Month = month != null ? month : 1;
            ViewBag.Year = year != null ? year : DateTime.Now.Year;
            return View(_listPhieuTra.ToPagedList(pageNumber, pageSize));
        }

    }
}