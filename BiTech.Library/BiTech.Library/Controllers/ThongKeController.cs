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
using BiTech.Library.Controllers.BaseClass;

namespace BiTech.Library.Controllers
{
    public class ThongKeController : BaseController
    {
        private ThongKeLogic _thongKeLogic;
        private ChiTietPhieuMuonLogic _chiTietPhieuMuonLogic;
        NghiepVuThongKeController nghiepVu;
        public ThongKeController()
        {
            _thongKeLogic = new ThongKeLogic(Tool.GetConfiguration("ConnectionString"), Tool.GetConfiguration("DatabaseName"));
            _chiTietPhieuMuonLogic = new ChiTietPhieuMuonLogic(Tool.GetConfiguration("ConnectionString"), Tool.GetConfiguration("DatabaseName"));
            nghiepVu = new NghiepVuThongKeController();
        }
        // GET: ThongKe
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DanhSachMuon(int? page, string day, int? month, int? year)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            var _thongKeLogic = new ThongKeLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            var _chiTietPhieuMuonLogic = new ChiTietPhieuMuonLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            int soSachTong = 0;
            int soSachChuaTra = 0;

            List<object> listSoLuongThanhVien = new List<object>();
            List<PhieuMuon> listPhieuMuon = _thongKeLogic.GetAllPhieuMuon();
            List<int> listSoSachTong = new List<int>();
            List<Sach> listSach = new List<Sach>();
            List<ThanhVien> listThanhVien = new List<ThanhVien>();
            List<PhieuMuon> listAll = new List<PhieuMuon>();
            List<PhieuMuon> listMonthSelected = new List<PhieuMuon>();
            List<PhieuMuon> listYearSelected = new List<PhieuMuon>();
            List<PhieuMuon> listPhieuMuonTrongNgay = new List<PhieuMuon>();
            DateTime ngayTraNull = DateTime.ParseExact("01-01-0001", "dd-MM-yyyy", null);// Datatime NULL trong mongoDB khi xuất ra có dạng "01-01-0001"                                                                                                 
            foreach (var item in listPhieuMuon)
            {
                item.TrangThai = ETinhTrangPhieuMuon.none;
                // ---------------------SẮP XẾP TRẠNG THÁI-------------------------
                // Ngày phải trả < ngày hiện tại và ngày trả == NULL là CHƯA TRẢ   
                if (item.NgayPhaiTra < DateTime.Today && item.NgayTra == ngayTraNull && item.NgayTra != null)
                {
                    item.TrangThai = ETinhTrangPhieuMuon.ChuaTra;
                    item.TenTrangThai = "Chưa trả";
                    soSachChuaTra++;
                }
                // Ngày phải trả > ngày hiện tại và ngày trả == NULL là GẦN TRẢ                                      
                if (item.NgayPhaiTra >= DateTime.Today && item.NgayTra == ngayTraNull && item.NgayTra != null)
                {
                    item.TrangThai = ETinhTrangPhieuMuon.GanTra;
                    item.TenTrangThai = "Gần trả";
                    TimeSpan ts = item.NgayPhaiTra - DateTime.Today;
                    item.SoNgayGiaoDong = ts.Days;
                }
                // --------  
                // Ngày phải trả <= ngày hiện tại và ngày trả <= ngày phải trả là TRẢ ĐÚNG HẸN
                if (item.NgayPhaiTra <= DateTime.Today)
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
                if (item.NgayPhaiTra > DateTime.Today)
                {
                    if (item.NgayTra != ngayTraNull && item.NgayTra != null && item.NgayTra <= item.NgayPhaiTra)
                    {
                        item.TrangThai = ETinhTrangPhieuMuon.TraDungHen;
                        item.TenTrangThai = "Trả đúng hẹn";
                    }
                }
                // --Sử lý theo Tháng Năm hoặc theo Ngày                          
                listAll.Add(item);
                if (day != null && day != "0")
                {
                    if (item.NgayMuon == DateTime.Parse(day))
                    {
                        listMonthSelected.Add(item);
                    }
                }
                else
                {
                    if (item.NgayMuon.Year == year)
                    {
                        listYearSelected.Add(item);
                        if (item.NgayMuon.Month == month)
                            listMonthSelected.Add(item);
                    }
                }
            }

            // var _listPhieuMuon = listAll.OrderBy(x => x.TrangThai).ThenBy(x => x.NgayPhaiTra);
            List<PhieuMuon> _listPhieuMuon = listMonthSelected.OrderBy(x => x.TrangThai).ThenBy(x => x.NgayPhaiTra).ToList();
            // --------------------THÔNG TIN PHIẾU MƯỢN--------------------           
            foreach (var item in _listPhieuMuon)
            {
                // --------------------THÔNG TIN SÁCH,THÀNH VIÊN--------------------
                // Lấy danh sách Thành Viên
                listThanhVien.Add(_thongKeLogic.GetThanhVienById(item.IdUser));
                // Lấy danh sách Sách từ Chi Tiết Phiếu Mượn
                var listCTPM = _thongKeLogic.GetCTPMById(item.Id);
                // listSach = new List<Sach>();
                soSachTong = 0;
                foreach (var i in listCTPM)
                {
                    var sach = _thongKeLogic.GetSachByIdDauSach(i.IdSach);
                    listSach.Add(sach);
                    // số lượng sách được mượn
                    var soLuong = i.SoLuong != 0 ? i.SoLuong : 1;
                    soSachTong += soLuong;     // tổng số sách được mượn trong 1 phiếu mượn          
                }
                listSoSachTong.Add(soSachTong);
            }
            // Dữ liệu thống kê
            ViewBag.SoPhieuMuonSach = listAll.Count;
            ViewBag.SoSachChuaTra = soSachChuaTra;
            ViewBag.SoSachDuocMuon = nghiepVu.DemSoSachDuocMuon(_listPhieuMuon);
            ViewBag.SoLuongThanhVien = nghiepVu.DemSoNguoiMuonSach(_listPhieuMuon);
            // Thông tin phiếu mượn
            ViewBag.ListSoSachTong = listSoSachTong;
            ViewBag.ListSach = listSach;
            ViewBag.ListThanhVien = listThanhVien;
            // Phân trang
            int pageSize = 20;
            int pageNumber = (page ?? 1);
            ViewBag.pageSize = pageSize;
            ViewBag.pages = pageNumber;
            // Get ngày, tháng, năm
            ViewBag.Page = page;
            ViewBag.Day = day;
            ViewBag.Month = month;//!= null ? month : 1;
            ViewBag.Year = year;// != null ? year : DateTime.Now.Year;
            return View(_listPhieuMuon.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult BieuDoPhieuMuon(int? month, int? year,BieuDoPhieuMuonViewModel model)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            var _thongKeLogic = new ThongKeLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            var _chiTietPhieuMuonLogic = new ChiTietPhieuMuonLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            // 1 tháng có 31 ngày
            int[] soNguoiMuonSachTrongThang = new int[31];
            int[] soNguoiTraTreTrongThang = new int[31];
            int[] soNguoiKhongTraTrongThang = new int[31];
            int[] soPhieuMuonTrongThang = new int[31];
            int[] soSachDuocMuonTrongThang = new int[31];
            // 1 năm có 4 quý
            int[] soNguoiMuonSachTrongQuy = new int[4];
            int[] soNguoiTraTreTrongQuy = new int[4];
            int[] soNguoiKhongTraTrongQuy = new int[4];
            int[] soPhieuMuonTrongQuy = new int[4];
            int[] soSachDuocMuonTrongQuy = new int[4];
            // 1 năm có 12 tháng
            int[] soNguoiMuonSachTrongNam = new int[12];
            int[] soNguoiTraTreTrongNam = new int[12];
            int[] soNguoiKhongTraTrongNam = new int[12];
            int[] soPhieuMuonTrongNam = new int[12];
            int[] soSachDuocMuonTrongNam = new int[12];

            List<PhieuMuon> listPhieuMuon = _thongKeLogic.GetAllPhieuMuon();
            List<PhieuMuon> listMonthSelected = new List<PhieuMuon>();
            List<PhieuMuon> listYearSelected = new List<PhieuMuon>();
            List<PhieuMuon> listPhieuMuonTrongNgay = new List<PhieuMuon>();

            DateTime ngayTraNull = DateTime.ParseExact("01-01-0001", "dd-MM-yyyy", null);// Datatime NULL trong mongoDB khi xuất ra có dạng "01-01-0001"                                                                                       
            List<PhieuMuon> listQuy1 = new List<PhieuMuon>();
            List<PhieuMuon> listQuy2 = new List<PhieuMuon>();
            List<PhieuMuon> listQuy3 = new List<PhieuMuon>();
            List<PhieuMuon> listQuy4 = new List<PhieuMuon>();
            List<PhieuMuon> listMonth1 = new List<PhieuMuon>();
            List<PhieuMuon> listMonth2 = new List<PhieuMuon>();
            List<PhieuMuon> listMonth3 = new List<PhieuMuon>();
            List<PhieuMuon> listMonth4 = new List<PhieuMuon>();
            List<PhieuMuon> listMonth5 = new List<PhieuMuon>();
            List<PhieuMuon> listMonth6 = new List<PhieuMuon>();
            List<PhieuMuon> listMonth7 = new List<PhieuMuon>();
            List<PhieuMuon> listMonth8 = new List<PhieuMuon>();
            List<PhieuMuon> listMonth9 = new List<PhieuMuon>();
            List<PhieuMuon> listMonth10 = new List<PhieuMuon>();
            List<PhieuMuon> listMonth11 = new List<PhieuMuon>();
            List<PhieuMuon> listMonth12 = new List<PhieuMuon>();
            // lấy danh sách phiếu mượn trong năm 
            foreach (var item in listPhieuMuon)
            {
                if (item.NgayMuon.Year == year)
                {
                    listYearSelected.Add(item);
                    if (item.NgayMuon.Month == month)
                        listMonthSelected.Add(item);
                }
            }
            List<PhieuMuon> _listPhieuMuon = listMonthSelected.ToList();
            // -----------------------THÔNG TIN THỐNG KÊ-----------------------  
            foreach (var item in _listPhieuMuon)
            {
                // danh sách phiếu mượn trong ngày (ghi tắt DSPMTN)
                listPhieuMuonTrongNgay = _thongKeLogic.GetPMByNgayMuon(item.NgayMuon);
                // từ DSPMTN lấy ra 5 loại dữ liệu để thống kê
                soPhieuMuonTrongThang[item.NgayMuon.Day] = listPhieuMuonTrongNgay.Count != 0 ? listPhieuMuonTrongNgay.Count : -1;
                soNguoiMuonSachTrongThang[item.NgayMuon.Day] = nghiepVu.DemSoNguoiMuonSach(listPhieuMuonTrongNgay) != 0 ? nghiepVu.DemSoNguoiMuonSach(listPhieuMuonTrongNgay) : -1;
                soSachDuocMuonTrongThang[item.NgayMuon.Day] = nghiepVu.DemSoSachDuocMuon(listPhieuMuonTrongNgay) != 0 ? nghiepVu.DemSoSachDuocMuon(listPhieuMuonTrongNgay) : -1;
                soNguoiKhongTraTrongThang[item.NgayMuon.Day] = nghiepVu.DemSoNguoiKhongTra(listPhieuMuonTrongNgay) != 0 ? nghiepVu.DemSoNguoiKhongTra(listPhieuMuonTrongNgay) : -1;
                soNguoiTraTreTrongThang[item.NgayMuon.Day] = nghiepVu.DemSoNguoiTraTre(listPhieuMuonTrongNgay) != 0 ? nghiepVu.DemSoNguoiTraTre(listPhieuMuonTrongNgay) : -1;
            }
            foreach (var item in listYearSelected)
            {
                // chia danh sách phiếu mượn trong năm thành 4 (ứng với 4 quý)
                if (item.NgayMuon.Month == 1 || item.NgayMuon.Month == 2 || item.NgayMuon.Month == 3)
                {
                    listQuy1.Add(item);
                }
                if (item.NgayMuon.Month == 4 || item.NgayMuon.Month == 5 || item.NgayMuon.Month == 6)
                {
                    listQuy2.Add(item);
                }
                if (item.NgayMuon.Month == 7 || item.NgayMuon.Month == 8 || item.NgayMuon.Month == 9)
                {
                    listQuy3.Add(item);
                }
                if (item.NgayMuon.Month == 10 || item.NgayMuon.Month == 11 || item.NgayMuon.Month == 12)
                {
                    listQuy4.Add(item);
                }
                #region --------12 thang
                // chia danh sách phiếu mượn trong năm thành 12 (ứng với 12 tháng)
                switch (item.NgayMuon.Month)
                {
                    case 1:
                        {
                            listMonth1.Add(item);
                            break;
                        }
                    case 2:
                        {
                            listMonth2.Add(item);
                            break;
                        }
                    case 3:
                        {
                            listMonth3.Add(item);
                            break;
                        }
                    case 4:
                        {
                            listMonth4.Add(item);
                            break;
                        }
                    case 5:
                        {
                            listMonth5.Add(item);
                            break;
                        }
                    case 6:
                        {
                            listMonth6.Add(item);
                            break;
                        }
                    case 7:
                        {
                            listMonth7.Add(item);
                            break;
                        }
                    case 8:
                        {
                            listMonth8.Add(item);
                            break;
                        }
                    case 9:
                        {
                            listMonth9.Add(item);
                            break;
                        }
                    case 10:
                        {
                            listMonth10.Add(item);
                            break;
                        }
                    case 11:
                        {
                            listMonth11.Add(item);
                            break;
                        }
                    case 12:
                        {
                            listMonth12.Add(item);
                            break;
                        }
                }
                #endregion                
            }
            // truyền dữ liệu thống kê vào từng tháng trong năm
            for (int i = 0; i < 12; i++)
            {
                List<PhieuMuon> list = new List<PhieuMuon>();
                #region -------- 12 thang
                switch (i)
                {
                    case 0:
                        {
                            list = listMonth1;
                            break;
                        }
                    case 1:
                        {
                            list = listMonth2;
                            break;
                        }
                    case 2:
                        {
                            list = listMonth3;
                            break;
                        }
                    case 3:
                        {
                            list = listMonth4;
                            break;
                        }
                    case 4:
                        {
                            list = listMonth5;
                            break;
                        }
                    case 5:
                        {
                            list = listMonth6;
                            break;
                        }
                    case 6:
                        {
                            list = listMonth7;
                            break;
                        }
                    case 7:
                        {
                            list = listMonth8;
                            break;
                        }
                    case 8:
                        {
                            list = listMonth9;
                            break;
                        }
                    case 9:
                        {
                            list = listMonth10;
                            break;
                        }
                    case 10:
                        {
                            list = listMonth11;
                            break;
                        }
                    case 11:
                        {
                            list = listMonth12;
                            break;
                        }
                }
                #endregion
                // dữ liệu mỗi tháng ứng mới 1 phần tử trong mảng (mảng có 12 phần tử ứng với 12 tháng)
                soNguoiMuonSachTrongNam[i] = nghiepVu.DemSoNguoiMuonSach(list);
                soNguoiTraTreTrongNam[i] = nghiepVu.DemSoNguoiTraTre(list);
                soNguoiKhongTraTrongNam[i] = nghiepVu.DemSoNguoiKhongTra(list);
                soPhieuMuonTrongNam[i] = nghiepVu.DemSoPhieuMuon(list);
                soSachDuocMuonTrongNam[i] = nghiepVu.DemSoSachDuocMuon(list);
            }
            // truyền dữ liệu thống kê vào từng quý trong năm
            for (int i = 0; i < 4; i++)
            {
                List<PhieuMuon> list = new List<PhieuMuon>();
                switch (i)
                {
                    case 0:
                        {
                            list = listQuy1;
                            break;
                        }
                    case 1:
                        {
                            list = listQuy2;
                            break;
                        }
                    case 2:
                        {
                            list = listQuy3;
                            break;
                        }
                    case 3:
                        {
                            list = listQuy4;
                            break;
                        }
                }
                // dữ liệu mỗi quý ứng với từng phần tử trong mảng (mảng có 4 phần tử ứng với 4 quý)
                soNguoiMuonSachTrongQuy[i] = nghiepVu.DemSoNguoiMuonSach(list);
                soNguoiTraTreTrongQuy[i] = nghiepVu.DemSoNguoiTraTre(list);
                soNguoiKhongTraTrongQuy[i] = nghiepVu.DemSoNguoiKhongTra(list);
                soPhieuMuonTrongQuy[i] = nghiepVu.DemSoPhieuMuon(list);
                soSachDuocMuonTrongQuy[i] = nghiepVu.DemSoSachDuocMuon(list);
            }
            // Khai báo list chứa dữ liệu thống kê của từng ngày trong tháng 
            List<int> lsoNgayTrongThang = new List<int>();
            List<int> lsoPMTrongNgay = new List<int>();
            List<int> lsoNguoiMuonTrongNgay = new List<int>();
            List<int> lsoSachDuocMuonTrongNgay = new List<int>();
            List<int> lsoNguoiKhongTraTrongNgay = new List<int>();
            List<int> lsoNguoiTraTreTrongNgay = new List<int>();
            // chọn ra những ngày nào có phiếu mượn để gắn vào list 
            for (int i = 0; i < 31; i++)
            {
                if (soPhieuMuonTrongThang[i] != 0)
                {
                    lsoNgayTrongThang.Add(i);
                    lsoPMTrongNgay.Add(soPhieuMuonTrongThang[i]);
                }
                if (soNguoiMuonSachTrongThang[i] != 0)
                {
                    lsoNguoiMuonTrongNgay.Add(soNguoiMuonSachTrongThang[i]);
                }
                if (soSachDuocMuonTrongThang[i] != 0)
                {
                    lsoSachDuocMuonTrongNgay.Add(soSachDuocMuonTrongThang[i]);
                }
                if (soNguoiKhongTraTrongThang[i] != 0)
                {
                    lsoNguoiKhongTraTrongNgay.Add(soNguoiKhongTraTrongThang[i]);
                }
                if (soNguoiTraTreTrongThang[i] != 0)
                {
                    lsoNguoiTraTreTrongNgay.Add(soNguoiTraTreTrongThang[i]);
                }
            }

            JsonResult result = new JsonResult();
            // Chuyền dữ liệu vào Model
            model = new BieuDoPhieuMuonViewModel
            {
                // Thống kê trong Năm (chia ra 12 tháng)
                lsoPhieuMuonTrongNam = soPhieuMuonTrongNam,

                lsoNguoiMuonSachTrongNam = soNguoiMuonSachTrongNam,
                lsoSachDuocMuonTrongNam = soSachDuocMuonTrongNam,
                lsoNguoiKhongTraTrongNam = soNguoiKhongTraTrongNam,
                lsoNguoiTraTreTrongNam = soNguoiTraTreTrongNam,
                // Thống kê trong Quý (chia ra 4 Quý)          
                lsoPhieuMuonTrongQuy = soPhieuMuonTrongQuy,
                lsoNguoiMuonSachTrongQuy = soNguoiMuonSachTrongQuy,
                lsoSachDuocMuonTrongQuy = soSachDuocMuonTrongQuy,
                lsoNguoiKhongTraTrongQuy = soNguoiKhongTraTrongQuy,
                lsoNguoiTraTreTrongQuy = soNguoiTraTreTrongQuy,
                // Thống kê trong Tháng (chia ra 31 ngày)
                lsoNgayTrongThang = lsoNgayTrongThang,
                lsoPMTrongNgay = lsoPMTrongNgay,
                lsoNguoiMuonTrongNgay = lsoNguoiMuonTrongNgay,
                lsoSachDuocMuonTrongNgay = lsoSachDuocMuonTrongNgay,
                lsoNguoiKhongTraTrongNgay = lsoNguoiKhongTraTrongNgay,
                lsoNguoiTraTreTrongNgay = lsoNguoiTraTreTrongNgay
            };
            // Tháng, năm       
            ViewBag.Month = month;//!= null ? month : 1;
            ViewBag.Year = year;// != null ? year : DateTime.Now.Year;
            return View(model);
        }
        public ActionResult DanhSachTra(int? page, string day, int? month, int? year)
        {
            int soSachDuocMuon = 0;
            var listPhieuMuon = _thongKeLogic.GetAllPhieuMuon();
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
                if (item.NgayPhaiTra <= DateTime.Today)
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
            ViewBag.Year = year != null ? year : DateTime.Today.Year;
            return View(_listPhieuTra.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult LichSuMuonSach(int? page, string id, int? pagePM, string day, int? month, int? year)
        {
            int soSachDuocMuon = 0;
            List<Sach> listSach = new List<Sach>();
            // List<ChiTietPhieuMuon> listCTPM = new List<ChiTietPhieuMuon>();
            List<object> listChaCuaSach = new List<object>();
            List<int> listSoLuongSach = new List<int>();
            ViewBag.TenThanhVien = _thongKeLogic.GetThanhVienById(id).Ten;

            var listPhieuMuon = _thongKeLogic.GetPMByIdThanhVien(id);
            // Danh sách phiếu mượn           
            DateTime ngayTraNull = DateTime.ParseExact("01-01-0001", "dd-MM-yyyy", null);
            foreach (var item in listPhieuMuon)
            {
                item.TrangThai = ETinhTrangPhieuMuon.none;
                // ---------------------SẮP XẾP TRẠNG THÁI-------------------------
                // Ngày phải trả <= ngày hiện tại và ngày trả == NULL là CHƯA TRẢ   
                if (item.NgayPhaiTra <= DateTime.Today && item.NgayTra == ngayTraNull && item.NgayTra != null)
                {
                    item.TrangThai = ETinhTrangPhieuMuon.ChuaTra;
                    item.TenTrangThai = "Chưa trả";
                }
                // Ngày phải trả > ngày hiện tại và ngày trả == NULL là GẦN TRẢ                                      
                if (item.NgayPhaiTra > DateTime.Today && item.NgayTra == ngayTraNull && item.NgayTra != null)
                {
                    item.TrangThai = ETinhTrangPhieuMuon.GanTra;
                    item.TenTrangThai = "Gần trả";
                    TimeSpan ts = item.NgayPhaiTra - DateTime.Today;
                    item.SoNgayGiaoDong = ts.Days;
                }
                // -- cong Phieu Tra vo
                // --------  
                // Ngày phải trả <= ngày hiện tại và ngày trả <= ngày phải trả là TRẢ ĐÚNG HẸN
                if (item.NgayPhaiTra <= DateTime.Today)
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
                if (item.NgayPhaiTra > DateTime.Today)
                {
                    if (item.NgayTra != ngayTraNull && item.NgayTra != null && item.NgayTra <= item.NgayPhaiTra)
                    {
                        item.TrangThai = ETinhTrangPhieuMuon.TraDungHen;
                        item.TenTrangThai = "Trả đúng hẹn";
                    }
                }
            }
            // --------            
            var _listPhieuMuon = listPhieuMuon.OrderBy(x => x.TrangThai).ThenBy(x => x.NgayMuon);
            // Duyệt dữ liệu từ danh sách đã được sắp xếp lại
            foreach (var item in _listPhieuMuon)
            {
                var listCTPM = _thongKeLogic.GetCTPMById(item.Id);
                listSach = new List<Sach>();
                foreach (var ctpm in listCTPM)
                {
                    // Lấy danh sách Sách từ Chi Tiết Phiếu Mượn
                    var sach = _thongKeLogic.GetSachByIdDauSach(ctpm.IdSach);
                    listSach.Add(sach);
                    // số lượng sách được mượn
                    var soLuong = ctpm.SoLuong != 0 ? ctpm.SoLuong : 1;
                    soSachDuocMuon += soLuong;
                    listSoLuongSach.Add(soLuong);
                }
                listChaCuaSach.Add(listSach);
            }
            ViewBag.ListChaCuaSach = listChaCuaSach;
            ViewBag.SoLuongSachMuon = listSoLuongSach;
            // Phân trang
            int pageSize = 20;
            int pageNumber = (page ?? 1);
            ViewBag.pageSize = pageSize;
            ViewBag.pages = pageNumber;
            ViewBag.Id = id;
            // ngay thang nam
            ViewBag.pagePM = pagePM;
            ViewBag.Day = day;
            ViewBag.Month = month;//!= null ? month : 1;
            ViewBag.Year = year;
            return View(_listPhieuMuon.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult DanhSachChuaTra(int? page, int? pagePM, string day, int? month, int? year)
        {
            int soSachDuocMuon = 0;
            List<Sach> listSach = new List<Sach>();
            // List<ChiTietPhieuMuon> listCTPM = new List<ChiTietPhieuMuon>();
            List<object> listChaCuaSach = new List<object>();
            List<int> listSoLuongSach = new List<int>();
            List<PhieuMuon> listChuaTra = new List<PhieuMuon>();
            List<ThanhVien> listThanhVien = new List<ThanhVien>();
            List<string> listIdThanhVien = new List<string>();

            List<PhieuMuon> listPMChuaTra = new List<PhieuMuon>();
            bool testName = false;
            var listPhieuMuon = _thongKeLogic.GetAllPhieuMuon();
            // Danh sách phiếu mượn           
            DateTime ngayTraNull = DateTime.ParseExact("01-01-0001", "dd-MM-yyyy", null);
            foreach (var item in listPhieuMuon)
            {
                item.TrangThai = ETinhTrangPhieuMuon.none;
                // ---------------------SẮP XẾP TRẠNG THÁI-------------------------
                if (item.NgayTra == ngayTraNull && item.NgayTra != null)
                {
                    // Ngày phải trả <= ngày hiện tại và ngày trả == NULL là CHƯA TRẢ   
                    if (item.NgayPhaiTra <= DateTime.Today)
                    {
                        item.TrangThai = ETinhTrangPhieuMuon.ChuaTra;
                        item.TenTrangThai = "Chưa trả";
                    }
                    // Ngày phải trả > ngày hiện tại và ngày trả == NULL là GẦN TRẢ                                      
                    else
                    {
                        item.TrangThai = ETinhTrangPhieuMuon.GanTra;
                        item.TenTrangThai = "Gần trả";
                        TimeSpan ts = item.NgayPhaiTra - DateTime.Today;
                        item.SoNgayGiaoDong = ts.Days;
                    }
                    listChuaTra.Add(item);
                }
                //--- Kiểm tra tên trùng
                if (listPMChuaTra.Count == 0)
                {
                    listPMChuaTra.Add(item);
                }
                else
                {
                    foreach (var id in listPMChuaTra)
                    {
                        if (id.IdUser.Equals(item.IdUser) == true)
                        {
                            testName = false;
                            break;
                        }
                        else
                        {
                            testName = true;
                        }
                    }
                }
                if (testName)
                {
                    listPMChuaTra.Add(item);
                }
            }
            // Sắp xếp lại phiếu mượn         
            // var _listPhieuMuon = listPMChuaTra.OrderBy(x => x.TrangThai).ThenBy(x => x.NgayPhaiTra);
            var _listPhieuMuon = listChuaTra.OrderBy(x => x.TrangThai).ThenBy(x => x.NgayPhaiTra);
            foreach (var item in _listPhieuMuon)
            {
                // Lấy danh sách Thành Viên
                listThanhVien.Add(_thongKeLogic.GetThanhVienById(item.IdUser));

                listIdThanhVien.Add(item.IdUser);
                // Lấy danh sách CTPM
                var listCTPM = _thongKeLogic.GetCTPMById(item.Id);
                // listSach = new List<Sach>();
                foreach (var ctpm in listCTPM)
                {
                    // Lấy danh sách Sách từ Chi Tiết Phiếu Mượn
                    var sach = _thongKeLogic.GetSachByIdDauSach(ctpm.IdSach);
                    listSach.Add(sach);
                    // số lượng sách được mượn
                    var soLuong = ctpm.SoLuong != 0 ? ctpm.SoLuong : 1;
                    soSachDuocMuon += soLuong;
                    listSoLuongSach.Add(soLuong);
                }
                listChaCuaSach.Add(listSach);
            }
            ViewBag.ListChaCuaSach = listChaCuaSach;
            ViewBag.SoLuongSachMuon = listSoLuongSach;
            ViewBag.ListThanhVien = listThanhVien;
            ViewBag.ListIdThanhVien = listIdThanhVien;
            ViewBag.ListSach = listSach;
            ViewBag.SoSachDuocMuon = soSachDuocMuon;
            // Phân trang
            int pageSize = 20;
            int pageNumber = (page ?? 1);
            ViewBag.pageSize = pageSize;
            ViewBag.pages = pageNumber;
            // ngay thang nam
            ViewBag.Page = page;
            ViewBag.pagePM = pagePM;
            ViewBag.Day = day;
            ViewBag.Month = month;//!= null ? month : 1;
            ViewBag.Year = year;
            return View(_listPhieuMuon.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult ThongTinDocGia(string id, int? page, ThongKeViewModel model)
        {
            List<PhieuMuon> listChuaTra = new List<PhieuMuon>();
            List<Sach> listSach = new List<Sach>();
            List<PhieuMuon> listModelPM = new List<PhieuMuon>();

            var thanhVien = _thongKeLogic.GetThanhVienById(id);
            ViewBag.Page = page;
            ViewBag.IdThanhVien = id;

            //-----------------
            int soSachDuocMuon = 0;
            // List<ChiTietPhieuMuon> listCTPM = new List<ChiTietPhieuMuon>();
            List<List<Sach>> listChaCuaSach = new List<List<Sach>>();
            List<int> listSoLuongSach = new List<int>();
            var listPhieuMuon = _thongKeLogic.GetPMByIdThanhVien(id);
            // Danh sách phiếu mượn           
            DateTime ngayTraNull = DateTime.ParseExact("01-01-0001", "dd-MM-yyyy", null);
            foreach (PhieuMuon item in listPhieuMuon)
            {
                item.TrangThai = ETinhTrangPhieuMuon.none;
                // ---------------------SẮP XẾP TRẠNG THÁI-------------------------
                if (item.NgayTra == ngayTraNull && item.NgayTra != null)
                {
                    // Ngày phải trả <= ngày hiện tại và ngày trả == NULL là CHƯA TRẢ   
                    if (item.NgayPhaiTra <= DateTime.Today)
                    {
                        item.TrangThai = ETinhTrangPhieuMuon.ChuaTra;
                        item.TenTrangThai = "Chưa trả";
                    }
                    // Ngày phải trả > ngày hiện tại và ngày trả == NULL là GẦN TRẢ                                      
                    else
                    {
                        item.TrangThai = ETinhTrangPhieuMuon.GanTra;
                        item.TenTrangThai = "Gần trả";
                        TimeSpan ts = item.NgayPhaiTra - DateTime.Today;
                        item.SoNgayGiaoDong = ts.Days;
                    }
                    listChuaTra.Add(item);
                }
            }
            // --------                 
            var _listPhieuMuon = listChuaTra.OrderBy(x => x.TrangThai).ThenBy(x => x.NgayMuon);
            // Duyệt dữ liệu từ danh sách đã được sắp xếp lại
            foreach (PhieuMuon item in _listPhieuMuon)
            {
                listModelPM.Add(item);
                var listCTPM = _thongKeLogic.GetCTPMById(item.Id);
                listSach = new List<Sach>();

                foreach (var ctpm in listCTPM)
                {
                    // Lấy danh sách Sách từ Chi Tiết Phiếu Mượn
                    var sach = _thongKeLogic.GetSachByIdDauSach(ctpm.IdSach);
                    listSach.Add(sach);
                    // số lượng sách được mượn
                    var soLuong = ctpm.SoLuong != 0 ? ctpm.SoLuong : 1;
                    soSachDuocMuon += soLuong;
                    listSoLuongSach.Add(soLuong);
                }
                listChaCuaSach.Add(listSach);
            }
            model = new ThongKeViewModel
            {
                HinhChanDung = thanhVien.HinhChanDung,
                TenDocGia = thanhVien.Ten,
                CMND = thanhVien.CMND,
                DiaChi = thanhVien.DiaChi,
                SDT = thanhVien.SDT,
                TrangThaiUser = thanhVien.TrangThai,
                ListChaCuaSach = listChaCuaSach as List<List<Sach>>,
                ListSoLuong = listSoLuongSach,
                ListPhieuMuon = listModelPM as List<PhieuMuon>
            };

            return View(model);
        }

    }
}