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
        // GET: Statictical

        NghiepVuThongKe nghiepVu;
        ThongTinThuVienLogic _ThongTinThuVien;
        public StatisticController()
        {
            _ThongTinThuVien = new ThongTinThuVienLogic(Tool.GetConfiguration("ConnectionString"), Tool.GetConfiguration("DatabaseName"));
            //_chiTietPhieuMuonLogic = new ChiTietPhieuMuonLogic(Tool.GetConfiguration("ConnectionString"), Tool.GetConfiguration("DatabaseName"));
            nghiepVu = new NghiepVuThongKe();
        }
        // GET: ThongKe
        public ActionResult Index()
        {
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
            #region  L?y thông tin ngu?i dùng
            var userdata = GetUserData();
            if (userdata == null)
                return Json(null, JsonRequestBehavior.AllowGet);


            var _thongKeLogic = new ThongKeLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            #endregion
            #region Khai báo
            if (year == null)
                year = DateTime.Now.Year;
            if (month == null)
                month = DateTime.Now.Month;

            // 1 tháng có 31 ngày
            int[] soNguoiMuonSachTrongThang = new int[32];
            int[] soNguoiTraTreTrongThang = new int[32];
            int[] soNguoiKhongTraTrongThang = new int[32];
            int[] soPhieuMuonTrongThang = new int[32];
            int[] soSachDuocMuonTrongThang = new int[32];
            // 1 nam có 4 quý
            int[] soNguoiMuonSachTrongQuy = new int[4];
            int[] soNguoiTraTreTrongQuy = new int[4];
            int[] soNguoiKhongTraTrongQuy = new int[4];
            int[] soPhieuMuonTrongQuy = new int[4];
            int[] soSachDuocMuonTrongQuy = new int[4];
            // 1 nam có 12 tháng
            int[] soNguoiMuonSachTrongNam = new int[12];
            int[] soNguoiTraTreTrongNam = new int[12];
            int[] soNguoiKhongTraTrongNam = new int[12];
            int[] soPhieuMuonTrongNam = new int[12];
            int[] soSachDuocMuonTrongNam = new int[12];


            List<ThongTinMuonSach> listPhieuMuon = _thongKeLogic.GetAllTTMS();
            List<ThongTinMuonSach> listMonthSelected = new List<ThongTinMuonSach>();
            List<ThongTinMuonSach> listYearSelected = new List<ThongTinMuonSach>();
            List<ThongTinMuonSach> listPhieuMuonTrongNgay = new List<ThongTinMuonSach>();

            DateTime ngayTraNull = DateTime.ParseExact("01-01-0001", "dd-MM-yyyy", null);// Datatime NULL trong mongoDB khi xu?t ra có d?ng "01-01-0001"                                                                                       
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
            // l?y danh sách phi?u mu?n trong nam 
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
            // -----------------------THÔNG TIN TH?NG KÊ-----------------------  
            foreach (var item in _listPhieuMuon)
            {
                DateTime ngayMuon = item.NgayGioMuon;
                // danh sách phi?u mu?n trong ngày (ghi t?t DSPMTN)
                listPhieuMuonTrongNgay = _thongKeLogic.GetTTMSByNgayMuon(item.NgayGioMuon);
                // t? DSPMTN l?y ra 5 lo?i d? li?u d? th?ng kê
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
                // chia danh sách phi?u mu?n trong nam thành 12 (?ng v?i 12 tháng) và 4 Quý
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
            // truy?n d? li?u th?ng kê vào t?ng tháng trong nam         
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

                // d? li?u m?i tháng ?ng m?i 1 ph?n t? trong m?ng (m?ng có 12 ph?n t? ?ng v?i 12 tháng)
                soNguoiMuonSachTrongNam[i] = nghiepVu.DemSoNguoiMuonSach(list);
                soNguoiTraTreTrongNam[i] = nghiepVu.DemSoNguoiTraTre(list);
                soNguoiKhongTraTrongNam[i] = nghiepVu.DemSoNguoiKhongTra(list);
                soPhieuMuonTrongNam[i] = nghiepVu.DemSoPhieuMuon(list);
                soSachDuocMuonTrongNam[i] = nghiepVu.DemSoSachDuocMuon(list, userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            }
            // truy?n d? li?u th?ng kê vào t?ng quý trong nam
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
                // d? li?u m?i quý ?ng v?i t?ng ph?n t? trong m?ng (m?ng có 4 ph?n t? ?ng v?i 4 quý)
                soNguoiMuonSachTrongQuy[i] = nghiepVu.DemSoNguoiMuonSach(list);
                soNguoiTraTreTrongQuy[i] = nghiepVu.DemSoNguoiTraTre(list);
                soNguoiKhongTraTrongQuy[i] = nghiepVu.DemSoNguoiKhongTra(list);
                soPhieuMuonTrongQuy[i] = nghiepVu.DemSoPhieuMuon(list);
                soSachDuocMuonTrongQuy[i] = nghiepVu.DemSoSachDuocMuon(list, userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            }
            // Ch?n s? ngày cho t?ng tháng
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
            JsonResult result = new JsonResult();
            // Chuy?n d? li?u vào Model
            model = new BieuDoPhieuMuonViewModel
            {
                // Th?ng kê trong Nam (chia ra 12 tháng)
                lsoPhieuMuonTrongNam = soPhieuMuonTrongNam,

                lsoNguoiMuonSachTrongNam = soNguoiMuonSachTrongNam,
                lsoSachDuocMuonTrongNam = soSachDuocMuonTrongNam,
                lsoNguoiKhongTraTrongNam = soNguoiKhongTraTrongNam,
                lsoNguoiTraTreTrongNam = soNguoiTraTreTrongNam,
                // Th?ng kê trong Quý (chia ra 4 Quý)          
                lsoPhieuMuonTrongQuy = soPhieuMuonTrongQuy,
                lsoNguoiMuonSachTrongQuy = soNguoiMuonSachTrongQuy,
                lsoSachDuocMuonTrongQuy = soSachDuocMuonTrongQuy,
                lsoNguoiKhongTraTrongQuy = soNguoiKhongTraTrongQuy,
                lsoNguoiTraTreTrongQuy = soNguoiTraTreTrongQuy,
                // Th?ng kê trong Tháng (chia ra 31 ngày)
                SoNgayTrongThang = soNgayTrongThang,
                lsoPMTrongNgay = soPhieuMuonTrongThang,
                lsoNguoiMuonTrongNgay = soNguoiMuonSachTrongThang,
                lsoSachDuocMuonTrongNgay = soSachDuocMuonTrongThang,
                lsoNguoiKhongTraTrongNgay = soNguoiKhongTraTrongThang,
                lsoNguoiTraTreTrongNgay = soNguoiTraTreTrongThang
            };
            // Tháng, nam       
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