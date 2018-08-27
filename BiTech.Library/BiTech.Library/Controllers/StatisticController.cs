using BiTech.Library.BLL.DBLogic;
using BiTech.Library.Controllers.BaseClass;
using BiTech.Library.DTO;
using BiTech.Library.Helpers;
using BiTech.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BiTech.Library.Controllers
{
    public class StatisticController : BaseController
    {
        NghiepVuThongKe nghiepVu = new NghiepVuThongKe();
        ThongTinThuVienLogic _ThongTinThuVien;

        // GET: ThongKe
        public ActionResult Index()
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            _ThongTinThuVien = new ThongTinThuVienLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

            string Key = "nambatdau";
            ThongTinThuVien tt = _ThongTinThuVien.GetCustomKey(Key);
            if (tt == null)
            {
                tt = new ThongTinThuVien()
                {
                    Key = Key,
                    Value = DateTime.Now.Year.ToString()
                };
                _ThongTinThuVien.SetCustomKey(tt);
            }
            ViewBag.tt = tt.Value;
            return View();
        }

        public JsonResult BieuDoPhieuMuon(int? month, int? year, BieuDoPhieuMuonViewModel model)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return Json(null,JsonRequestBehavior.AllowGet);
            var _thongKeLogic = new ThongKeLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            #endregion
            #region Khai báo
            if (month == null && year == null)
            {
                month =DateTime.Now.Month;
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
            }
            // -----------------------THÔNG TIN THỐNG KÊ-----------------------  
            #region Sach Tra           
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
            JsonResult result = new JsonResult();
            // Tháng, năm       
            //  ViewBag.Month = month;//!= null ? month : 1;
            //  ViewBag.Year = year;// != null ? year : DateTime.Now.Year;
            ViewBag.Month = month != null ? month : 1;
            ViewBag.Year = year != null ? year : 2017;
            result.Data = model;
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            return result;
        }
    }
}