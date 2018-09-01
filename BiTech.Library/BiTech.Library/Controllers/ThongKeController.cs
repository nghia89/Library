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
        static List<string> list_tk = new List<string>();// List chứ số sách được mượn và số người mượn sách

        public ThongKeController()
        {
            nghiepVu = new NghiepVuThongKe();
        }

        // GET: ThongKe
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MuonSach()
        {
            return View();
        }

        [HttpPost]
        public JsonResult MuonSachJson(int? page, string day, int? month, int? year)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return Json(false, JsonRequestBehavior.AllowGet);

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
            if (day == null && month == null && year == null)
            {
                month = DateTime.Now.Month;
                year = DateTime.Now.Year;
            }
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
                    item.TrangThaiString = ETinhTrangPhieuMuon.ChuaTra.ToString();
                    item.TenTrangThai = "Chưa trả";
                    soSachChuaTra++;
                }
                // Ngày phải trả > ngày hiện tại và ngày trả == NULL là GẦN TRẢ                                      
                if (ngayPhaiTra >= DateTime.Today && (ngayTraThucTe == ngayTraNull || ngayTraThucTe == null))
                {
                    item.TrangThai = ETinhTrangPhieuMuon.GanTra;
                    item.TrangThaiString = ETinhTrangPhieuMuon.GanTra.ToString();
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
                        item.TrangThaiString = ETinhTrangPhieuMuon.TraDungHen.ToString();
                        item.TenTrangThai = "Trả đúng hẹn";
                    }
                    // ngày trả > ngày phải trả là TRẢ TRỄ
                    if (ngayTraThucTe != ngayTraNull && ngayTraThucTe != null && ngayTraThucTe > ngayPhaiTra)
                    {
                        item.TrangThai = ETinhTrangPhieuMuon.TraTre;
                        item.TrangThaiString = ETinhTrangPhieuMuon.TraTre.ToString();
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
                        item.TrangThaiString = ETinhTrangPhieuMuon.TraDungHen.ToString();
                        item.TenTrangThai = "Trả đúng hẹn";
                    }
                }
                #endregion
                // --Sử lý theo Tháng Năm hoặc theo Ngày  
                if (day != null && day != "0")
                {
                    string[] arrTemp = day.Split('T');
                    day = arrTemp[0];
                    if (ngayMuon.ToString("dd/MM/yyyy") == day || ngayMuon.ToString("dd-MM-yyyy") == day)
                    {
                        item.NgayMuonTemp = item.NgayGioMuon.ToString("dd-MM-yyyy");
                        item.NgayTraTemp = item.NgayPhaiTra.ToString("dd-MM-yyyy");
                        #region Ngày trả thật tế
                        string ngayTra = null;
                        if (item.NgayTraThucTe != null)
                            ngayTra = item.NgayTraThucTe.ToString("dd-MM-yyyy");
                        if (ngayTra.Equals("01-01-0001") == false && ngayTra != null)
                        {
                            item.NgayTraThucTeTemp = ngayTra;
                            if (item.SoNgayGiaoDong != null)
                            {
                                // Trễ 
                                item.TenTrangThai = item.TenTrangThai + " - (" + (item.SoNgayGiaoDong.ToString() + " ngày)");
                            }
                            else
                            {
                                // đúng hạn
                                item.TenTrangThai = item.TenTrangThai;
                            }
                        }

                        else
                        {
                            item.NgayTraThucTeTemp = "---//---";
                            if (item.SoNgayGiaoDong != null && item.SoNgayGiaoDong < 30)
                            {
                                // Gần sắp trả
                                item.TenTrangThai = item.TenTrangThai + " - (còn " + item.SoNgayGiaoDong.ToString() + " ngày)";
                            }
                            else
                            {
                                if (item.SoNgayGiaoDong == null)
                                {
                                    // Trễ
                                    var ngayTre = DateTime.Now - item.NgayPhaiTra;
                                    item.TenTrangThai = item.TenTrangThai + " - (trễ " + ngayTre.Days + " ngày)";
                                }
                                else
                                {
                                    // Gần trả
                                    item.TenTrangThai = "Còn " + item.SoNgayGiaoDong.ToString() + " ngày";
                                }
                            }
                        }
                        #endregion
                        listMonthSelected.Add(item);
                    }
                }
                else
                {
                    if (ngayMuon.Year == year)
                    {
                        if (ngayMuon.Month == month)
                        {
                            item.NgayMuonTemp = item.NgayGioMuon.ToString("dd-MM-yyyy");
                            item.NgayTraTemp = item.NgayPhaiTra.ToString("dd-MM-yyyy");
                            #region Ngày trả thật tế
                            string ngayTra = null;
                            if (item.NgayTraThucTe != null)
                                ngayTra = item.NgayTraThucTe.ToString("dd-MM-yyyy");
                            if (ngayTra.Equals("01-01-0001") == false && ngayTra != null)
                            {
                                item.NgayTraThucTeTemp = ngayTra;
                                if (item.SoNgayGiaoDong != null)
                                {
                                    // Trễ 
                                    item.TenTrangThai = item.TenTrangThai + " - (" + (item.SoNgayGiaoDong.ToString() + " ngày)");
                                }
                                else
                                {
                                    // đúng hạn
                                    item.TenTrangThai = item.TenTrangThai;
                                }
                            }

                            else
                            {
                                item.NgayTraThucTeTemp = "---//---";
                                if (item.SoNgayGiaoDong != null && item.SoNgayGiaoDong < 30)
                                {
                                    // Gần sắp trả
                                    item.TenTrangThai = item.TenTrangThai + " - (còn " + item.SoNgayGiaoDong.ToString() + " ngày)";
                                }
                                else
                                {
                                    if (item.SoNgayGiaoDong == null)
                                    {
                                        // Trễ
                                        var ngayTre = DateTime.Now - item.NgayPhaiTra;
                                        item.TenTrangThai = item.TenTrangThai + " - (trễ " + ngayTre.Days + " ngày)";
                                    }
                                    else
                                    {
                                        // Gần trả
                                        item.TenTrangThai = "Còn " + item.SoNgayGiaoDong.ToString() + " ngày";
                                    }
                                }
                            }
                            #endregion                         
                            listMonthSelected.Add(item);
                        }
                    }
                }
            }
            // --------------------THÔNG TIN SÁCH,THÀNH VIÊN--------------------                   
            List<ThongTinMuonSach> listRutGon = nghiepVu.RutGonDanhSach(listMonthSelected)
                .OrderBy(x => x.TrangThai).ThenBy(x => x.NgayGioMuon).ToList();
            int i = 1;
            foreach (var item in listRutGon)
            {
                var user = _thongKeLogic.GetThanhVienByMSTV(item.idUser);
                if (user != null)
                {
                    item.TenThanhVien = user.Ten;
                }
                item.TenSach = _thongKeLogic.GetSachById(item.idSach).TenSach;
                item.STT = i.ToString();
                i++;
            }

            list_tk.Clear();
            list_tk.Add(listMonthSelected.Count().ToString());
            list_tk.Add(nghiepVu.DemSoNguoiMuonSach(listRutGon).ToString());

            // Phân trang
            int pageSize = 20;
            int pageNumber = (page ?? 1);
            ViewBag.pageSize = pageSize;
            return Json(listRutGon.ToPagedList(pageNumber, pageSize), JsonRequestBehavior.AllowGet);
        }

        public JsonResult MuonSachJson_ssdm()
        {
            return Json(list_tk, JsonRequestBehavior.AllowGet);
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
            int[] soSachDuocTraTrongThang = new int[32];
            int[] soSachKhongTraTrongThang = new int[32];
            // 1 năm có 4 quý
            int[] soNguoiMuonSachTrongQuy = new int[4];
            int[] soNguoiTraTreTrongQuy = new int[4];
            int[] soNguoiKhongTraTrongQuy = new int[4];
            int[] soPhieuMuonTrongQuy = new int[4];
            int[] soSachDuocMuonTrongQuy = new int[4];
            int[] soSachDuocTraTrongQuy = new int[4];
            int[] soSachKhongTraTrongQuy = new int[4];
            // 1 năm có 12 tháng
            int[] soNguoiMuonSachTrongNam = new int[12];
            int[] soNguoiTraTreTrongNam = new int[12];
            int[] soNguoiKhongTraTrongNam = new int[12];
            int[] soPhieuMuonTrongNam = new int[12];
            int[] soSachDuocMuonTrongNam = new int[12];
            int[] soSachDuocTraTrongNam = new int[12];
            int[] soSachKhongTraTrongNam = new int[12];


            List<ThongTinMuonSach> listPhieuMuon = _thongKeLogic.GetAllTTMS();
            List<ThongTinMuonSach> listMonthSelected = new List<ThongTinMuonSach>();
            List<ThongTinMuonSach> listMonthSachTra = new List<ThongTinMuonSach>();
            List<ThongTinMuonSach> listYearSelected = new List<ThongTinMuonSach>();
            List<ThongTinMuonSach> listYearSachTra = new List<ThongTinMuonSach>();
            List<ThongTinMuonSach> listPhieuMuonTrongNgay = new List<ThongTinMuonSach>();
            List<ThongTinMuonSach> listSachTraTrongNgay = new List<ThongTinMuonSach>();

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

            List<ThongTinMuonSach> listQuy1s = new List<ThongTinMuonSach>();
            List<ThongTinMuonSach> listQuy2s = new List<ThongTinMuonSach>();
            List<ThongTinMuonSach> listQuy3s = new List<ThongTinMuonSach>();
            List<ThongTinMuonSach> listQuy4s = new List<ThongTinMuonSach>();
            List<ThongTinMuonSach> listMonth1s = new List<ThongTinMuonSach>();
            List<ThongTinMuonSach> listMonth2s = new List<ThongTinMuonSach>();
            List<ThongTinMuonSach> listMonth3s = new List<ThongTinMuonSach>();
            List<ThongTinMuonSach> listMonth4s = new List<ThongTinMuonSach>();
            List<ThongTinMuonSach> listMonth5s = new List<ThongTinMuonSach>();
            List<ThongTinMuonSach> listMonth6s = new List<ThongTinMuonSach>();
            List<ThongTinMuonSach> listMonth7s = new List<ThongTinMuonSach>();
            List<ThongTinMuonSach> listMonth8s = new List<ThongTinMuonSach>();
            List<ThongTinMuonSach> listMonth9s = new List<ThongTinMuonSach>();
            List<ThongTinMuonSach> listMonth10s = new List<ThongTinMuonSach>();
            List<ThongTinMuonSach> listMonth11s = new List<ThongTinMuonSach>();
            List<ThongTinMuonSach> listMonth12s = new List<ThongTinMuonSach>();


            #endregion          
            // lấy danh sách phiếu mượn trong năm 
            foreach (var item in listPhieuMuon)
            {
                #region Lấy list PM theo Thánh và Năm
                DateTime ngayMuon = item.NgayGioMuon;
                DateTime ngayTra = new DateTime();
                // Ngày mượn            
                if (ngayMuon.Year == year)
                {
                    listYearSelected.Add(item);
                    if (ngayMuon.Month == month)
                        listMonthSelected.Add(item);
                }
                // Ngày trả
                if (item.NgayTraThucTe != null)
                {
                    ngayTra = item.NgayTraThucTe;
                    if (ngayTra.Year == year)
                    {
                        listYearSachTra.Add(item);
                        if (ngayTra.Month == month)
                            listMonthSachTra.Add(item);
                    }
                }
                #endregion
            }
            // -----------------------THÔNG TIN THỐNG KÊ-----------------------  
            #region Tra Sach
            #region Theo Ngay trong thang
            List<ThongTinMuonSach> _listMonthSachTra = listMonthSachTra.ToList();
            List<ThongTinMuonSach>[] arrTTSachTra = new List<ThongTinMuonSach>[31];
            for (int i = 1; i <= 31; i++)
            {
                List<ThongTinMuonSach> listDay = new List<ThongTinMuonSach>();
                foreach (var item in _listMonthSachTra)
                {
                    if (item.NgayTraThucTe.Day == i)
                    {
                        listDay.Add(item);// List dữ liệu có cùng ngày trả
                        arrTTSachTra[i] = listDay;
                    }
                }
            }
            foreach (var item in _listMonthSachTra)
            {
                DateTime ngayTra = item.NgayTraThucTe;
                soSachDuocTraTrongThang[ngayTra.Day] = nghiepVu.DemSoSachDuocTra(arrTTSachTra[ngayTra.Day], ngayTra.Day, 0, 0);
            }

            #endregion
            #region Theo Tuan trong thang
            #endregion
            #region Theo Thang trong nam
            foreach (var item in listYearSachTra)
            {
                DateTime ngayTra = item.NgayTraThucTe;
                #region --------12 thang (sach tra)
                // chia danh sách phiếu mượn trong năm thành 12 (ứng với 12 tháng) và 4 Quý
                switch (ngayTra.Month)
                {
                    case 1:
                        {
                            listMonth1s.Add(item);
                            listQuy1s.Add(item);
                            break;
                        }
                    case 2:
                        {
                            listMonth2s.Add(item);
                            listQuy1s.Add(item);
                            break;
                        }
                    case 3:
                        {
                            listMonth3s.Add(item);
                            listQuy1s.Add(item);
                            break;
                        }
                    case 4:
                        {
                            listQuy2s.Add(item);
                            listMonth4s.Add(item);
                            break;
                        }
                    case 5:
                        {
                            listMonth5s.Add(item);
                            listQuy2s.Add(item);
                            break;
                        }
                    case 6:
                        {
                            listMonth6s.Add(item);
                            listQuy2s.Add(item);
                            break;
                        }
                    case 7:
                        {
                            listMonth7s.Add(item);
                            listQuy3s.Add(item);
                            break;
                        }
                    case 8:
                        {
                            listMonth8s.Add(item);
                            listQuy3s.Add(item);
                            break;
                        }
                    case 9:
                        {
                            listMonth9s.Add(item);
                            listQuy3s.Add(item);
                            break;
                        }
                    case 10:
                        {
                            listMonth10s.Add(item);
                            listQuy4s.Add(item);
                            break;
                        }
                    case 11:
                        {
                            listMonth11s.Add(item);
                            listQuy4s.Add(item);
                            break;
                        }
                    case 12:
                        {
                            listMonth12s.Add(item);
                            listQuy4s.Add(item);
                            break;
                        }
                }
                #endregion                
            }
            for (int i = 0; i < 12; i++)
            {
                List<ThongTinMuonSach> list = new List<ThongTinMuonSach>();
                #region -------- 12 thang (sach tra)
                switch (i)
                {
                    case 0:
                        {
                            list = listMonth1s;
                            break;
                        }
                    case 1:
                        {
                            list = listMonth2s;
                            break;
                        }
                    case 2:
                        {
                            list = listMonth3s;
                            break;
                        }
                    case 3:
                        {
                            list = listMonth4s;
                            break;
                        }
                    case 4:
                        {
                            list = listMonth5s;
                            break;
                        }
                    case 5:
                        {
                            list = listMonth6s;
                            break;
                        }
                    case 6:
                        {
                            list = listMonth7s;
                            break;
                        }
                    case 7:
                        {
                            list = listMonth8s;
                            break;
                        }
                    case 8:
                        {
                            list = listMonth9s;
                            break;
                        }
                    case 9:
                        {
                            list = listMonth10s;
                            break;
                        }
                    case 10:
                        {
                            list = listMonth11s;
                            break;
                        }
                    case 11:
                        {
                            list = listMonth12s;
                            break;
                        }
                }
                #endregion
                // dữ liệu mỗi tháng ứng mới 1 phần tử trong mảng (mảng có 12 phần tử ứng với 12 tháng)              
                // new                 
                int thang = 0;
                int nam = 0;
                if (month != null)
                    thang = (int)month;
                if (year != null)
                    nam = (int)year;
                soSachDuocTraTrongNam[i] = nghiepVu.DemSoSachDuocTra(list, null, thang, nam);
            }
            #endregion
            #region Theo Quy trong nam
            // truyền dữ liệu thống kê vào từng quý trong năm (Sach Tra)
            for (int i = 0; i < 4; i++)
            {
                List<ThongTinMuonSach> list = new List<ThongTinMuonSach>();
                switch (i)
                {
                    case 0:
                        {
                            list = listQuy1s;
                            break;
                        }
                    case 1:
                        {
                            list = listQuy2s;
                            break;
                        }
                    case 2:
                        {
                            list = listQuy3s;
                            break;
                        }
                    case 3:
                        {
                            list = listQuy4s;
                            break;
                        }
                }
                // dữ liệu mỗi quý ứng với từng phần tử trong mảng (mảng có 4 phần tử ứng với 4 quý)              
                // new 
                int thang = 0;
                int nam = 0;
                if (month != null)
                    thang = (int)month;
                if (year != null)
                    nam = (int)year;
                soSachDuocTraTrongQuy[i] = nghiepVu.DemSoSachDuocTra(list, null, thang, nam);
            }
            #endregion
            #endregion

            #region Muon Sach
            #region Theo Ngay trong thang
            List<ThongTinMuonSach> _listMonthSachMuon = listMonthSelected.ToList();
            List<ThongTinMuonSach>[] arrTTSachMuon = new List<ThongTinMuonSach>[31];
            for (int i = 1; i <= 31; i++)
            {
                List<ThongTinMuonSach> listDay = new List<ThongTinMuonSach>();
                foreach (var item in _listMonthSachMuon)
                {
                    if (item.NgayGioMuon.Day == i)
                    {
                        listDay.Add(item);// List dữ liệu có cùng ngày mượn
                        arrTTSachMuon[i] = listDay;
                    }
                }
            }
            foreach (var item in _listMonthSachMuon)
            {
                DateTime ngayMuon = item.NgayGioMuon;

                soPhieuMuonTrongThang[ngayMuon.Day] = nghiepVu.DemSoPhieuMuon(arrTTSachMuon[ngayMuon.Day]);
                soNguoiMuonSachTrongThang[ngayMuon.Day] = nghiepVu.DemSoNguoiMuonSach(arrTTSachMuon[ngayMuon.Day]);
                soSachDuocMuonTrongThang[ngayMuon.Day] = nghiepVu.DemSoSachDuocMuon(arrTTSachMuon[ngayMuon.Day], userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
                soNguoiKhongTraTrongThang[ngayMuon.Day] = nghiepVu.DemSoNguoiKhongTra(arrTTSachMuon[ngayMuon.Day]);
                soNguoiTraTreTrongThang[ngayMuon.Day] = nghiepVu.DemSoNguoiTraTre(arrTTSachMuon[ngayMuon.Day]);
                soSachKhongTraTrongThang[ngayMuon.Day] = nghiepVu.DemSoSachKhongTra(arrTTSachMuon[ngayMuon.Day]);
            }
            #endregion
            #region Theo Tuan trong thang        
            DateTime dayStart = new DateTime();
            DateTime dayEnd = new DateTime();

            arrTTSachMuon = new List<ThongTinMuonSach>[31];
            for (int i = 1; i <= 31; i++)
            {
                List<ThongTinMuonSach> listDay = new List<ThongTinMuonSach>();
                foreach (var item in _listMonthSachMuon)
                {
                    if (item.NgayGioMuon.Day == i)
                    {
                        listDay.Add(item);// List dữ liệu có cùng ngày mượn
                        arrTTSachMuon[i] = listDay;
                    }
                }
            }
            foreach (var item in _listMonthSachMuon)
            {
                DateTime ngayMuon = item.NgayGioMuon;

                soPhieuMuonTrongThang[ngayMuon.Day] = nghiepVu.DemSoPhieuMuon(arrTTSachMuon[ngayMuon.Day]);
                soNguoiMuonSachTrongThang[ngayMuon.Day] = nghiepVu.DemSoNguoiMuonSach(arrTTSachMuon[ngayMuon.Day]);
                soSachDuocMuonTrongThang[ngayMuon.Day] = nghiepVu.DemSoSachDuocMuon(arrTTSachMuon[ngayMuon.Day], userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
                soNguoiKhongTraTrongThang[ngayMuon.Day] = nghiepVu.DemSoNguoiKhongTra(arrTTSachMuon[ngayMuon.Day]);
                soNguoiTraTreTrongThang[ngayMuon.Day] = nghiepVu.DemSoNguoiTraTre(arrTTSachMuon[ngayMuon.Day]);
                soSachKhongTraTrongThang[ngayMuon.Day] = nghiepVu.DemSoSachKhongTra(arrTTSachMuon[ngayMuon.Day]);
            }
            #endregion
            #region Theo Thang trong nam
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
            List<ThongTinMuonSach>[] arrTTSachMuonMonth = new List<ThongTinMuonSach>[31];
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
                // new                                 
                soSachKhongTraTrongNam[i] = nghiepVu.DemSoSachKhongTra(list);
            }

            #endregion
            #region Theo Quy trong nam

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
                // new            
                soSachKhongTraTrongQuy[i] = nghiepVu.DemSoSachKhongTra(list);
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
            #endregion
            #endregion

            // Chuyền dữ liệu vào Model
            model = new BieuDoPhieuMuonViewModel
            {
                // Thống kê trong Năm (chia ra 12 tháng)
                lsoPhieuMuonTrongNam = soPhieuMuonTrongNam,
                lsoNguoiMuonSachTrongNam = soNguoiMuonSachTrongNam,
                lsoSachDuocMuonTrongNam = soSachDuocMuonTrongNam,
                lsoNguoiKhongTraTrongNam = soNguoiKhongTraTrongNam,
                lsoNguoiTraTreTrongNam = soNguoiTraTreTrongNam,
                //-- new
                lsoSachDuocTraTrongNam = soSachDuocTraTrongNam,
                lsoSachKhongTraTrongNam = soSachKhongTraTrongNam,
                // Thống kê trong Quý (chia ra 4 Quý)          
                lsoPhieuMuonTrongQuy = soPhieuMuonTrongQuy,
                lsoNguoiMuonSachTrongQuy = soNguoiMuonSachTrongQuy,
                lsoSachDuocMuonTrongQuy = soSachDuocMuonTrongQuy,
                lsoNguoiKhongTraTrongQuy = soNguoiKhongTraTrongQuy,
                lsoNguoiTraTreTrongQuy = soNguoiTraTreTrongQuy,
                //-- new
                lsoSachDuocTraTrongQuy = soSachDuocTraTrongQuy,
                lsoSachKhongTraTrongQuy = soSachKhongTraTrongQuy,
                // Thống kê trong Tháng (chia ra 31 ngày)
                SoNgayTrongThang = soNgayTrongThang,
                lsoPMTrongNgay = soPhieuMuonTrongThang,
                lsoNguoiMuonTrongNgay = soNguoiMuonSachTrongThang,
                lsoSachDuocMuonTrongNgay = soSachDuocMuonTrongThang,
                lsoNguoiKhongTraTrongNgay = soNguoiKhongTraTrongThang,
                lsoNguoiTraTreTrongNgay = soNguoiTraTreTrongThang,
                //-- new
                lsoSachDuocTraTrongNgay = soSachDuocTraTrongThang,
                lsoSachKhongTraTrongNgay = soSachKhongTraTrongThang,

            };

            // Tháng, năm                   
            ViewBag.Month = month != null ? month : 1;
            ViewBag.Year = year != null ? year : 2017;
            return View(model);
        }

        public ActionResult ThongKeTheoTuan(DateTime date)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            var _thongKeLogic = new ThongKeLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            #endregion
            // Thống kê theo tuần          
            DateTime thisWeekStart = date.AddDays(-(int)date.DayOfWeek);
            DateTime thisWeekEnd = thisWeekStart.AddDays(7).AddSeconds(-1);
            List<DateTime> listDates = new List<DateTime>();
            for (var i = 0; i < 7; i++)
            {
                listDates.Add(thisWeekStart.Date);
                thisWeekStart = thisWeekStart.AddDays(1);
            }
            BieuDoPhieuMuonViewModel model = new BieuDoPhieuMuonViewModel();
            model.thongKeTheoTuan = new List<int[]>();           
            foreach (var item in listDates)
            {
                // list chứ thông tin thống kê của 1 ngày
                model.thongKeTheoTuan.Add(nghiepVu.ThongKeTheoTuan(item, userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName));
            }
            return View(model);
        }

        public ActionResult LichSuMuonSach(int? page, string idUser, int? pagePM, string day, int? month, int? year, bool MuonSach)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            var _thongKeLogic = new ThongKeLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            #endregion

            // kiem tra null du lieu
            ThanhVien thanhVien = _thongKeLogic.GetThanhVienByMSTV(idUser);
            if (idUser == null || thanhVien == null || _thongKeLogic.GetTTMSByIdUser(thanhVien.MaSoThanhVien) == null)
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
            ViewBag.Id = idUser;
            // ngay thang nam
            ViewBag.MuonSach = MuonSach;
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

        public ActionResult ThongTinDocGia(string idUser, int? page, ThongKeViewModel model)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            var _thongKeLogic = new ThongKeLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            #endregion

            ThanhVien thanhVien = _thongKeLogic.GetThanhVienByMSTV(idUser);
            if (idUser == null || _thongKeLogic.GetTTMSByIdUser(thanhVien.MaSoThanhVien) == null || thanhVien == null)
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
                LopHoc = thanhVien.LopHoc,
                NienKhoa = thanhVien.NienKhoa,
                QRLink = thanhVien.QRLink,
                NgaySinh = thanhVien.NgaySinh,
                GioiTinh = thanhVien.GioiTinh,
                idUser = thanhVien.MaSoThanhVien
            };
            if (thanhVien.HinhChanDung != null)
                model.LinkAvatar = thanhVien.HinhChanDung;
            else
                model.LinkAvatar = "/Content/Images/user5.jpg";
            ViewBag.Page = page;
            ViewBag.IdThanhVien = idUser;
            ViewBag.ListSach = listSach;
            return View(model);
        }
    }
}