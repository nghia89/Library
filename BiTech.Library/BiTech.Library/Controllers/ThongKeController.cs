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

        NghiepVuThongKe nghiepVu;
        public ThongKeController()
        {
            nghiepVu = new NghiepVuThongKe();
        }
        // GET: ThongKe
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MuonSach(int? page, string day, int? month, int? year)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");

            var _thongKeLogic = new ThongKeLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            #endregion

            #region Khai báo
            List<ThongTinMuonSach> listAll = _thongKeLogic.GetAllTTMS();
            List<ThongTinMuonSach> listSelect = new List<ThongTinMuonSach>();          
            List<ThongTinMuonSach> listMonthSelected = new List<ThongTinMuonSach>();
            List<ThongTinMuonSach> listYearSelected = new List<ThongTinMuonSach>();
            List<ThanhVien> listThanhVien = new List<ThanhVien>();
            List<int> listSoSachTong = new List<int>();
            List<Sach> listSach = new List<Sach>();
            DateTime ngayTraNull = DateTime.ParseExact("01-01-0001", "dd-MM-yyyy", null);
            int soSachChuaTra = 0;
            #endregion

            foreach (var item in listAll)
            {
                #region SẮP XẾP TRẠNG THÁI
                item.TrangThai = ETinhTrangPhieuMuon.none;
                // ---------------------SẮP XẾP TRẠNG THÁI-------------------------             
                DateTime ngayPhaiTra = item.NgayPhaiTra;            
                DateTime ngayTraThucTe = item.NgayTraThucTe;
                DateTime ngayMuon = item.NgayGioMuon;

                // Ngày phải trả < ngày hiện tại và ngày trả == NULL là CHƯA TRẢ                  
                if (ngayPhaiTra < DateTime.Today && (ngayTraThucTe == ngayTraNull || ngayTraThucTe == null))
                {
                    item.TrangThai = ETinhTrangPhieuMuon.ChuaTra;
                    item.TenTrangThai = "Chưa trả";
                    soSachChuaTra++;
                }
                // Ngày phải trả > ngày hiện tại và ngày trả == NULL là GẦN TRẢ                                      
                if (ngayPhaiTra >= DateTime.Today && (ngayTraThucTe == ngayTraNull || ngayTraThucTe == null))
                {
                    item.TrangThai = ETinhTrangPhieuMuon.GanTra;
                    item.TenTrangThai = "Gần trả";
                    TimeSpan ts = ngayPhaiTra - DateTime.Today;
                    item.SoNgayGiaoDong = ts.Days;
                }
                // Ngày phải trả <= ngày hiện tại và ngày trả <= ngày phải trả là TRẢ ĐÚNG HẸN
                if (ngayPhaiTra <= DateTime.Today)
                {
                    if (ngayTraThucTe != ngayTraNull && ngayTraThucTe != null && ngayTraThucTe <= ngayPhaiTra)
                    {
                        item.TrangThai = ETinhTrangPhieuMuon.TraDungHen;
                        item.TenTrangThai = "Trả đúng hẹn";
                    }
                    // ngày trả > ngày phải trả là TRẢ TRỄ
                    if (ngayTraThucTe != ngayTraNull && ngayTraThucTe != null && ngayTraThucTe > ngayPhaiTra)
                    {
                        item.TrangThai = ETinhTrangPhieuMuon.TraTre;
                        item.TenTrangThai = "Trả trễ";
                        TimeSpan ts = ((DateTime)ngayTraThucTe) - ngayPhaiTra;
                        item.SoNgayGiaoDong = ts.Days;
                    }
                }
                // Ngày phải trả > ngày hiện tại và ngày trả <= ngày phải phải trả là TRẢ ĐÚNG HẸN
                if (ngayPhaiTra > DateTime.Today)
                {
                    if (ngayTraThucTe != ngayTraNull && ngayTraThucTe != null && ngayTraThucTe <= ngayPhaiTra)
                    {
                        item.TrangThai = ETinhTrangPhieuMuon.TraDungHen;
                        item.TenTrangThai = "Trả đúng hẹn";
                    }
                }
                #endregion
                // --Sử lý theo Tháng Năm hoặc theo Ngày                                        
                if (day != null && day != "0")
                {
                    if (ngayMuon.ToString("dd/MM/yyyy") == day|| ngayMuon.ToString("dd-MM-yyyy") == day)
                    {
                        listMonthSelected.Add(item);
                    }
                }
                else
                {
                    if (ngayMuon.Year == year)
                    {
                        listYearSelected.Add(item);
                        if (ngayMuon.Month == month)
                            listMonthSelected.Add(item);
                    }
                }          
            }         
            // --------------------THÔNG TIN SÁCH,THÀNH VIÊN--------------------                   
            List<ThongTinMuonSach> listRutGon = nghiepVu.RutGonDanhSach(listMonthSelected)
                .OrderBy(x => x.TrangThai).ThenBy(x => x.NgayGioMuon).ToList();

            foreach (var item in listRutGon)
            {
                var user = _thongKeLogic.GetThanhVienByMSTV(item.idUser);
                if (user != null)
                {
                    listThanhVien.Add(user);                 
                }
                //
                listSach.Add(_thongKeLogic.GetSachById(item.idSach));
            }
            // Dữ liệu thống kê                      
            ViewBag.SoSachChuaTra = soSachChuaTra;           
            ViewBag.SoSachDuocMuon = listMonthSelected.Count();
            ViewBag.SoLuongThanhVien = nghiepVu.DemSoNguoiMuonSach(listRutGon);
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
            return View(listRutGon.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult BieuDoPhieuMuon(int? month, int? year, BieuDoPhieuMuonViewModel model)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            var _thongKeLogic = new ThongKeLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            #endregion
            #region Khai báo
            if (month == null && year == null)
            {
                month = 1;
                year = DateTime.Now.Year;
            }
            // 1 tháng có 31 ngày
            int[] soNguoiMuonSachTrongThang = new int[32];
            int[] soNguoiTraTreTrongThang = new int[32];
            int[] soNguoiKhongTraTrongThang = new int[32];
            int[] soPhieuMuonTrongThang = new int[32];
            int[] soSachDuocMuonTrongThang = new int[32];
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


            List<ThongTinMuonSach> listPhieuMuon = _thongKeLogic.GetAllTTMS();
            List<ThongTinMuonSach> listMonthSelected = new List<ThongTinMuonSach>();
            List<ThongTinMuonSach> listYearSelected = new List<ThongTinMuonSach>();
            List<ThongTinMuonSach> listPhieuMuonTrongNgay = new List<ThongTinMuonSach>();

            DateTime ngayTraNull = DateTime.ParseExact("01-01-0001", "dd-MM-yyyy", null);// Datatime NULL trong mongoDB khi xuất ra có dạng "01-01-0001"                                                                                       
            List<ThongTinMuonSach> listQuy1 = new List<ThongTinMuonSach>();
            List<ThongTinMuonSach> listQuy2 = new List<ThongTinMuonSach>();
            List<ThongTinMuonSach> listQuy3 = new List<ThongTinMuonSach>();
            List<ThongTinMuonSach> listQuy4 = new List<ThongTinMuonSach>();
            List<ThongTinMuonSach> listMonth1 = new List<ThongTinMuonSach>();
            List<ThongTinMuonSach> listMonth2 = new List<ThongTinMuonSach>();
            List<ThongTinMuonSach> listMonth3 = new List<ThongTinMuonSach>();
            List<ThongTinMuonSach> listMonth4 = new List<ThongTinMuonSach>();
            List<ThongTinMuonSach> listMonth5 = new List<ThongTinMuonSach>();
            List<ThongTinMuonSach> listMonth6 = new List<ThongTinMuonSach>();
            List<ThongTinMuonSach> listMonth7 = new List<ThongTinMuonSach>();
            List<ThongTinMuonSach> listMonth8 = new List<ThongTinMuonSach>();
            List<ThongTinMuonSach> listMonth9 = new List<ThongTinMuonSach>();
            List<ThongTinMuonSach> listMonth10 = new List<ThongTinMuonSach>();
            List<ThongTinMuonSach> listMonth11 = new List<ThongTinMuonSach>();
            List<ThongTinMuonSach> listMonth12 = new List<ThongTinMuonSach>();
            #endregion
            // lấy danh sách phiếu mượn trong năm 
            foreach (var item in listPhieuMuon)
            {
                DateTime ngayMuon = item.NgayGioMuon;
                if (ngayMuon.Year == year)
                {
                    listYearSelected.Add(item);
                    if (ngayMuon.Month == month)
                        listMonthSelected.Add(item);
                }
            }
            List<ThongTinMuonSach> _listPhieuMuon = listMonthSelected.ToList();
            // -----------------------THÔNG TIN THỐNG KÊ-----------------------  
            foreach (var item in _listPhieuMuon)
            {
                DateTime ngayMuon = item.NgayGioMuon;
                // danh sách phiếu mượn trong ngày (ghi tắt DSPMTN)
                listPhieuMuonTrongNgay = _thongKeLogic.GetTTMSByNgayMuon(item.NgayGioMuon);

                // từ DSPMTN lấy ra 5 loại dữ liệu để thống kê
                soPhieuMuonTrongThang[ngayMuon.Day] = listPhieuMuonTrongNgay.Count;
                soNguoiMuonSachTrongThang[ngayMuon.Day] = nghiepVu.DemSoNguoiMuonSach(listPhieuMuonTrongNgay);
                soSachDuocMuonTrongThang[ngayMuon.Day] = nghiepVu.DemSoSachDuocMuon(listPhieuMuonTrongNgay, userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
                soNguoiKhongTraTrongThang[ngayMuon.Day] = nghiepVu.DemSoNguoiKhongTra(listPhieuMuonTrongNgay);
                soNguoiTraTreTrongThang[ngayMuon.Day] = nghiepVu.DemSoNguoiTraTre(listPhieuMuonTrongNgay);

            }
            foreach (var item in listYearSelected)
            {
                DateTime ngayMuon = item.NgayGioMuon;

                #region --------12 thang
                // chia danh sách phiếu mượn trong năm thành 12 (ứng với 12 tháng) và 4 Quý
                switch (ngayMuon.Month)
                {
                    case 1:
                        {
                            listMonth1.Add(item);
                            listQuy1.Add(item);
                            break;
                        }
                    case 2:
                        {
                            listMonth2.Add(item);
                            listQuy1.Add(item);
                            break;
                        }
                    case 3:
                        {
                            listMonth3.Add(item);
                            listQuy1.Add(item);
                            break;
                        }
                    case 4:
                        {
                            listQuy2.Add(item);
                            listMonth4.Add(item);
                            break;
                        }
                    case 5:
                        {
                            listMonth5.Add(item);
                            listQuy2.Add(item);
                            break;
                        }
                    case 6:
                        {
                            listMonth6.Add(item);
                            listQuy2.Add(item);
                            break;
                        }
                    case 7:
                        {
                            listMonth7.Add(item);
                            listQuy3.Add(item);
                            break;
                        }
                    case 8:
                        {
                            listMonth8.Add(item);
                            listQuy3.Add(item);
                            break;
                        }
                    case 9:
                        {
                            listMonth9.Add(item);
                            listQuy3.Add(item);
                            break;
                        }
                    case 10:
                        {
                            listMonth10.Add(item);
                            listQuy4.Add(item);
                            break;
                        }
                    case 11:
                        {
                            listMonth11.Add(item);
                            listQuy4.Add(item);
                            break;
                        }
                    case 12:
                        {
                            listMonth12.Add(item);
                            listQuy4.Add(item);
                            break;
                        }
                }
                #endregion                
            }
            // truyền dữ liệu thống kê vào từng tháng trong năm         
            for (int i = 0; i < 12; i++)
            {
                List<ThongTinMuonSach> list = new List<ThongTinMuonSach>();

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
                soSachDuocMuonTrongNam[i] = nghiepVu.DemSoSachDuocMuon(list, userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            }
            // truyền dữ liệu thống kê vào từng quý trong năm
            for (int i = 0; i < 4; i++)
            {
                List<ThongTinMuonSach> list = new List<ThongTinMuonSach>();
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
                soSachDuocMuonTrongQuy[i] = nghiepVu.DemSoSachDuocMuon(list, userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            }
            // Chọn số ngày cho từng tháng
            int soNgayTrongThang = 0;
            switch (month)
            {
                case 1:
                case 3:
                case 5:
                case 7:
                case 8:
                case 10:
                case 12:
                    {
                        soNgayTrongThang = 31;
                        break;
                    }
                case 4:
                case 6:
                case 9:
                case 11:
                    {
                        soNgayTrongThang = 30;
                        break;
                    }
                case 2:
                    {
                        if (((year % 4 == 0) && (year % 100 != 0)) || (year % 400 == 0))
                            soNgayTrongThang = 29;
                        else
                            soNgayTrongThang = 28;
                        break;
                    }

            }

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
                SoNgayTrongThang = soNgayTrongThang,
                lsoPMTrongNgay = soPhieuMuonTrongThang,
                lsoNguoiMuonTrongNgay = soNguoiMuonSachTrongThang,
                lsoSachDuocMuonTrongNgay = soSachDuocMuonTrongThang,
                lsoNguoiKhongTraTrongNgay = soNguoiKhongTraTrongThang,
                lsoNguoiTraTreTrongNgay = soNguoiTraTreTrongThang
            };
            // Tháng, năm       
            //  ViewBag.Month = month;//!= null ? month : 1;
            //  ViewBag.Year = year;// != null ? year : DateTime.Now.Year;
            ViewBag.Month = month != null ? month : 1;
            ViewBag.Year = year != null ? year : 2017;
            return View(model);
        }
        public ActionResult LichSuMuonSach(int? page, string id, int? pagePM, string day, int? month, int? year)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            var _thongKeLogic = new ThongKeLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            #endregion

            // kiem tra null du lieu
            ThanhVien thanhVien = _thongKeLogic.GetThanhVienById(id);
            if (id == null || thanhVien == null|| _thongKeLogic.GetTTMSByIdUser(thanhVien.MaSoThanhVien)==null)
                return RedirectToAction("NotFound", "Error");           
            List<Sach> listSach = new List<Sach>();                              
            ViewBag.TenThanhVien = thanhVien.Ten;
   
            var listPhieuMuon = _thongKeLogic.GetTTMSByIdUser(thanhVien.MaSoThanhVien);
            if (listPhieuMuon == null)
                return RedirectToAction("NotFound", "Error");
            // Danh sách phiếu mượn           
            DateTime ngayTraNull = DateTime.ParseExact("01-01-0001", "dd-MM-yyyy", null);
            foreach (var item in listPhieuMuon)
            {
                DateTime ngayPhaiTra = item.NgayPhaiTra;
                DateTime ngayTraThucTe = item.NgayTraThucTe;
                DateTime ngayMuon = item.NgayGioMuon;

                item.TrangThai = ETinhTrangPhieuMuon.none;
                // ---------------------SẮP XẾP TRẠNG THÁI-------------------------
                // Ngày phải trả <= ngày hiện tại và ngày trả == NULL là CHƯA TRẢ   
                if (ngayPhaiTra <= DateTime.Today && (ngayTraThucTe == ngayTraNull || ngayTraThucTe == null))
                {
                    item.TrangThai = ETinhTrangPhieuMuon.ChuaTra;
                    item.TenTrangThai = "Chưa trả";
                }
                // Ngày phải trả > ngày hiện tại và ngày trả == NULL là GẦN TRẢ                                      
                if (ngayPhaiTra > DateTime.Today && (ngayTraThucTe == ngayTraNull || ngayTraThucTe == null))
                {
                    item.TrangThai = ETinhTrangPhieuMuon.GanTra;
                    item.TenTrangThai = "Gần trả";
                    TimeSpan ts = ngayPhaiTra - DateTime.Today;
                    item.SoNgayGiaoDong = ts.Days;
                }
                // -- cong Phieu Tra vo
                // --------  
                // Ngày phải trả <= ngày hiện tại và ngày trả <= ngày phải trả là TRẢ ĐÚNG HẸN
                if (ngayPhaiTra <= DateTime.Today)
                {
                    if (ngayTraThucTe != ngayTraNull && ngayTraThucTe != null && ngayTraThucTe != null && ngayTraThucTe <= ngayPhaiTra)
                    {
                        item.TrangThai = ETinhTrangPhieuMuon.TraDungHen;
                        item.TenTrangThai = "Trả đúng hẹn";
                    }
                    // ngày trả > ngày phải trả là TRẢ TRỄ
                    if (ngayTraThucTe != ngayTraNull && ngayTraThucTe != null && ngayTraThucTe != null && ngayTraThucTe > ngayPhaiTra)
                    {
                        item.TrangThai = ETinhTrangPhieuMuon.TraTre;
                        item.TenTrangThai = "Trả trễ";
                        TimeSpan ts = ((DateTime)ngayTraThucTe) - ngayPhaiTra;
                        item.SoNgayGiaoDong = ts.Days;
                    }
                }
                // Ngày phải trả > ngày hiện tại và ngày trả <= ngày phải phải trả là TRẢ ĐÚNG HẸN
                if (ngayPhaiTra > DateTime.Today)
                {
                    if (ngayTraThucTe != ngayTraNull && ngayTraThucTe != null && ngayTraThucTe <= ngayPhaiTra)
                    {
                        item.TrangThai = ETinhTrangPhieuMuon.TraDungHen;
                        item.TenTrangThai = "Trả đúng hẹn";
                    }
                }
            }           
            // Duyệt dữ liệu từ danh sách đã được sắp xếp lại
            List<ThongTinMuonSach> listRutGon = nghiepVu.RutGonDanhSach(listPhieuMuon)
                .OrderBy(x => x.TrangThai)
                .ThenBy(x => x.NgayGioMuon).ToList();
            foreach (var item in listRutGon)
            {               
                Sach sach = _thongKeLogic.GetSachById(item.idSach);
                listSach.Add(sach);
            }                    
            ViewBag.ListSach = listSach;
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
            return View(listRutGon.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult DanhSachChuaTra(int? page, int? pagePM, string day, int? month, int? year)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            var _thongKeLogic = new ThongKeLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            #endregion          

            List<Sach> listSach = new List<Sach>();
            // List<ChiTietPhieuMuon> listCTPM = new List<ChiTietPhieuMuon>();
            List<object> listChaCuaSach = new List<object>();
            List<int> listSoLuongSach = new List<int>();
            List<ThongTinMuonSach> listChuaTra = new List<ThongTinMuonSach>();
            List<ThanhVien> listThanhVien = new List<ThanhVien>();
            List<string> listIdThanhVien = new List<string>();

            List<ThongTinMuonSach> listPMChuaTra = new List<ThongTinMuonSach>();
            var listMuon = _thongKeLogic.GetAllTTMS();
            // Danh sách phiếu mượn           
            DateTime ngayTraNull = DateTime.ParseExact("01-01-0001", "dd-MM-yyyy", null);
            foreach (var item in listMuon)
            {
                item.TrangThai = ETinhTrangPhieuMuon.none;
                DateTime ngayPhaiTra = item.NgayPhaiTra;
                DateTime ngayTraThucTe = item.NgayTraThucTe;
                DateTime ngayMuon = item.NgayGioMuon;

                // ---------------------SẮP XẾP TRẠNG THÁI-------------------------
                if (ngayTraThucTe == ngayTraNull || ngayTraThucTe == null)
                {
                    // Ngày phải trả <= ngày hiện tại và ngày trả == NULL là CHƯA TRẢ   
                    if (ngayPhaiTra <= DateTime.Today)
                    {
                        item.TrangThai = ETinhTrangPhieuMuon.ChuaTra;
                        item.TenTrangThai = "Chưa trả";
                    }
                    // Ngày phải trả > ngày hiện tại và ngày trả == NULL là GẦN TRẢ                                      
                    else
                    {
                        item.TrangThai = ETinhTrangPhieuMuon.GanTra;
                        item.TenTrangThai = "Gần trả";
                        TimeSpan ts = ngayPhaiTra - DateTime.Today;
                        item.SoNgayGiaoDong = ts.Days;
                    }
                    listChuaTra.Add(item);
                }
                #region Kiểm tra tên trùng
                //if (listPMChuaTra.Count == 0)
                //{
                //    listPMChuaTra.Add(item);
                //}
                //else
                //{
                //    foreach (var id in listPMChuaTra)
                //    {
                //        if (id.IdUser.Equals(item.IdUser) == true)
                //        {
                //            testName = false;
                //            break;
                //        }
                //        else
                //        {
                //            testName = true;
                //        }
                //    }
                //}
                //if (testName)
                //{
                //    listPMChuaTra.Add(item);
                //}
                #endregion
            }          
            List<ThongTinMuonSach> listRutGon = nghiepVu.RutGonDanhSach(listChuaTra)
                .OrderBy(x => x.TrangThai)
                .ThenBy(x => x.NgayGioMuon).ToList();
            foreach (var item in listRutGon)
            {
                // Lấy danh sách Thành Viên
                listThanhVien.Add(_thongKeLogic.GetThanhVienByMSTV(item.idUser));
                listIdThanhVien.Add(item.idUser);
                listSach.Add(_thongKeLogic.GetSachById(item.idSach));                           
            }          
            // Thông tin Sách,Thành Viên
            ViewBag.ListThanhVien = listThanhVien;
            ViewBag.ListIdThanhVien = listIdThanhVien;
            ViewBag.ListSach = listSach;
            //  ViewBag.SoSachDuocMuon = soSachDuocMuon;
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
            return View(listRutGon.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult ThongTinDocGia(string id, int? page, ThongKeViewModel model)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            var _thongKeLogic = new ThongKeLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            #endregion

            ThanhVien thanhVien = _thongKeLogic.GetThanhVienById(id);
            if (id == null || _thongKeLogic.GetTTMSByIdUser(thanhVien.MaSoThanhVien) == null || thanhVien == null)
                return RedirectToAction("NotFound", "Error");
            List<ThongTinMuonSach> listChuaTra = new List<ThongTinMuonSach>();
            List<Sach> listSach = new List<Sach>();
            List<ThongTinMuonSach> listRutGon = new List<ThongTinMuonSach>();                   
            var listMuon = _thongKeLogic.GetTTMSByIdUser(thanhVien.MaSoThanhVien);
            // Danh sách phiếu mượn           
            DateTime ngayTraNull = DateTime.ParseExact("01-01-0001", "dd-MM-yyyy", null);
            foreach (var item in listMuon)
            {
                item.TrangThai = ETinhTrangPhieuMuon.none;
                DateTime ngayPhaiTra = item.NgayPhaiTra;
                DateTime ngayTraThucTe = item.NgayTraThucTe;
                DateTime ngayMuon = item.NgayGioMuon;

                // ---------------------SẮP XẾP TRẠNG THÁI-------------------------
                if (ngayTraThucTe == ngayTraNull || ngayTraThucTe == null)
                {
                    if (ngayPhaiTra <= DateTime.Today)
                    {
                        item.TrangThai = ETinhTrangPhieuMuon.ChuaTra;
                        item.TenTrangThai = "Chưa trả";
                    }
                    else
                    {
                        item.TrangThai = ETinhTrangPhieuMuon.GanTra;
                        item.TenTrangThai = "Gần trả";
                        TimeSpan ts = ngayPhaiTra - DateTime.Today;
                        item.SoNgayGiaoDong = ts.Days;
                    }
                    listChuaTra.Add(item);
                }
            }
            // Duyệt dữ liệu từ danh sách đã được sắp xếp lại           
            listRutGon = nghiepVu.RutGonDanhSach(listChuaTra)
                .OrderBy(x => x.TrangThai)
                 .ThenBy(x => x.NgayGioMuon).ToList();

            foreach (var item in listRutGon)
            {
               // var listCTPM = _thongKeLogic.GetCTPMById(item.Id);
                listSach.Add(_thongKeLogic.GetSachById(item.idSach));
            }

            model = new ThongKeViewModel
            {
                HinhChanDung = thanhVien.HinhChanDung,
                TenDocGia = thanhVien.Ten,
                DiaChi = thanhVien.DiaChi,
                SDT = thanhVien.SDT,
                TrangThaiUser = thanhVien.TrangThai,
                ListPhieuMuon = listRutGon as List<ThongTinMuonSach>,
                LinkAvatar = thanhVien.HinhChanDung,
                LopHoc = thanhVien.LopHoc,
                NienKhoa = thanhVien.NienKhoa,
                QRLink = thanhVien.QRLink,
                NgaySinh=thanhVien.NgaySinh,
                GioiTinh=thanhVien.GioiTinh
                
            };

            ViewBag.Page = page;
            ViewBag.IdThanhVien = id;
            ViewBag.ListSach = listSach;
            return View(model);
        }
    }
}