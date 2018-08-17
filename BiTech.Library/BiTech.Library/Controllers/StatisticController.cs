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



        //public JsonResult BieuDoPhieuMuon(int? month, int? year, BieuDoPhieuMuonViewModel model)
        //{
        //    #region  Lấy thông tin người dùng
        //    var userdata = GetUserData();
        //    //if (userdata == null)
        //    //    return RedirectToAction("LogOff", "Account");
        //    #endregion
        //    if (year == null)
        //        year = DateTime.Now.Year;
        //    if (month == null)
        //        month = DateTime.Now.Month;

        //    var _thongKeLogic = new ThongKeLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
        //    var _chiTietPhieuMuonLogic = new ChiTietPhieuMuonLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
        //    // 1 tháng có 31 ngày
        //    int[] soNguoiMuonSachTrongThang = new int[32];
        //    int[] soNguoiTraTreTrongThang = new int[32];
        //    int[] soNguoiKhongTraTrongThang = new int[32];
        //    int[] soPhieuMuonTrongThang = new int[32];
        //    int[] soSachDuocMuonTrongThang = new int[32];
        //    // 1 năm có 4 quý
        //    int[] soNguoiMuonSachTrongQuy = new int[4];
        //    int[] soNguoiTraTreTrongQuy = new int[4];
        //    int[] soNguoiKhongTraTrongQuy = new int[4];
        //    int[] soPhieuMuonTrongQuy = new int[4];
        //    int[] soSachDuocMuonTrongQuy = new int[4];
        //    // 1 năm có 12 tháng
        //    int[] soNguoiMuonSachTrongNam = new int[12];
        //    int[] soNguoiTraTreTrongNam = new int[12];
        //    int[] soNguoiKhongTraTrongNam = new int[12];
        //    int[] soPhieuMuonTrongNam = new int[12];
        //    int[] soSachDuocMuonTrongNam = new int[12];

        //    List<PhieuMuon> listPhieuMuon = _thongKeLogic.GetAllPhieuMuon();
        //    List<PhieuMuon> listMonthSelected = new List<PhieuMuon>();
        //    List<PhieuMuon> listYearSelected = new List<PhieuMuon>();
        //    List<PhieuMuon> listPhieuMuonTrongNgay = new List<PhieuMuon>();

        //    DateTime ngayTraNull = DateTime.ParseExact("01-01-0001", "dd-MM-yyyy", null);// Datatime NULL trong mongoDB khi xuất ra có dạng "01-01-0001"                                                                                       
        //    List<PhieuMuon> listQuy1 = new List<PhieuMuon>();
        //    List<PhieuMuon> listQuy2 = new List<PhieuMuon>();
        //    List<PhieuMuon> listQuy3 = new List<PhieuMuon>();
        //    List<PhieuMuon> listQuy4 = new List<PhieuMuon>();
        //    List<PhieuMuon> listMonth1 = new List<PhieuMuon>();
        //    List<PhieuMuon> listMonth2 = new List<PhieuMuon>();
        //    List<PhieuMuon> listMonth3 = new List<PhieuMuon>();
        //    List<PhieuMuon> listMonth4 = new List<PhieuMuon>();
        //    List<PhieuMuon> listMonth5 = new List<PhieuMuon>();
        //    List<PhieuMuon> listMonth6 = new List<PhieuMuon>();
        //    List<PhieuMuon> listMonth7 = new List<PhieuMuon>();
        //    List<PhieuMuon> listMonth8 = new List<PhieuMuon>();
        //    List<PhieuMuon> listMonth9 = new List<PhieuMuon>();
        //    List<PhieuMuon> listMonth10 = new List<PhieuMuon>();
        //    List<PhieuMuon> listMonth11 = new List<PhieuMuon>();
        //    List<PhieuMuon> listMonth12 = new List<PhieuMuon>();
        //    // lấy danh sách phiếu mượn trong năm 
        //    foreach (var item in listPhieuMuon)
        //    {
        //        if (item.NgayMuon.Year == year)
        //        {
        //            listYearSelected.Add(item);
        //            if (item.NgayMuon.Month == month)
        //                listMonthSelected.Add(item);
        //        }
        //    }
        //    List<PhieuMuon> _listPhieuMuon = listMonthSelected.ToList();
        //    // -----------------------THÔNG TIN THỐNG KÊ-----------------------  
        //    foreach (var item in _listPhieuMuon)
        //    {
        //        // danh sách phiếu mượn trong ngày (ghi tắt DSPMTN)
        //        listPhieuMuonTrongNgay = _thongKeLogic.GetPMByNgayMuon(item.NgayMuon);
        //        // từ DSPMTN lấy ra 5 loại dữ liệu để thống kê
        //        soPhieuMuonTrongThang[item.NgayMuon.Day] = listPhieuMuonTrongNgay.Count; //!= 0 ? listPhieuMuonTrongNgay.Count : -1;
        //        soNguoiMuonSachTrongThang[item.NgayMuon.Day] = nghiepVu.DemSoNguoiMuonSach(listPhieuMuonTrongNgay);// != 0 ? nghiepVu.DemSoNguoiMuonSach(listPhieuMuonTrongNgay) : -1;
        //        soSachDuocMuonTrongThang[item.NgayMuon.Day] = nghiepVu.DemSoSachDuocMuon(listPhieuMuonTrongNgay, userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName); //!= 0 ? nghiepVu.DemSoSachDuocMuon(listPhieuMuonTrongNgay) : -1;
        //        soNguoiKhongTraTrongThang[item.NgayMuon.Day] = nghiepVu.DemSoNguoiKhongTra(listPhieuMuonTrongNgay); //!= 0 ? nghiepVu.DemSoNguoiKhongTra(listPhieuMuonTrongNgay) : -1;
        //        soNguoiTraTreTrongThang[item.NgayMuon.Day] = nghiepVu.DemSoNguoiTraTre(listPhieuMuonTrongNgay); //!= 0 ? nghiepVu.DemSoNguoiTraTre(listPhieuMuonTrongNgay) : -1;
        //    }
        //    foreach (var item in listYearSelected)
        //    {
        //        // chia danh sách phiếu mượn trong năm thành 4 (ứng với 4 quý)
        //        if (item.NgayMuon.Month == 1 || item.NgayMuon.Month == 2 || item.NgayMuon.Month == 3)
        //        {
        //            listQuy1.Add(item);
        //        }
        //        if (item.NgayMuon.Month == 4 || item.NgayMuon.Month == 5 || item.NgayMuon.Month == 6)
        //        {
        //            listQuy2.Add(item);
        //        }
        //        if (item.NgayMuon.Month == 7 || item.NgayMuon.Month == 8 || item.NgayMuon.Month == 9)
        //        {
        //            listQuy3.Add(item);
        //        }
        //        if (item.NgayMuon.Month == 10 || item.NgayMuon.Month == 11 || item.NgayMuon.Month == 12)
        //        {
        //            listQuy4.Add(item);
        //        }
        //        #region --------12 thang
        //        // chia danh sách phiếu mượn trong năm thành 12 (ứng với 12 tháng)
        //        switch (item.NgayMuon.Month)
        //        {
        //            case 1:
        //                {
        //                    listMonth1.Add(item);
        //                    break;
        //                }
        //            case 2:
        //                {
        //                    listMonth2.Add(item);
        //                    break;
        //                }
        //            case 3:
        //                {
        //                    listMonth3.Add(item);
        //                    break;
        //                }
        //            case 4:
        //                {
        //                    listMonth4.Add(item);
        //                    break;
        //                }
        //            case 5:
        //                {
        //                    listMonth5.Add(item);
        //                    break;
        //                }
        //            case 6:
        //                {
        //                    listMonth6.Add(item);
        //                    break;
        //                }
        //            case 7:
        //                {
        //                    listMonth7.Add(item);
        //                    break;
        //                }
        //            case 8:
        //                {
        //                    listMonth8.Add(item);
        //                    break;
        //                }
        //            case 9:
        //                {
        //                    listMonth9.Add(item);
        //                    break;
        //                }
        //            case 10:
        //                {
        //                    listMonth10.Add(item);
        //                    break;
        //                }
        //            case 11:
        //                {
        //                    listMonth11.Add(item);
        //                    break;
        //                }
        //            case 12:
        //                {
        //                    listMonth12.Add(item);
        //                    break;
        //                }
        //        }
        //        #endregion                
        //    }
        //    // truyền dữ liệu thống kê vào từng tháng trong năm
        //    for (int i = 0; i < 12; i++)
        //    {
        //        List<PhieuMuon> list = new List<PhieuMuon>();
        //        #region -------- 12 thang
        //        switch (i)
        //        {
        //            case 0:
        //                {
        //                    list = listMonth1;
        //                    break;
        //                }
        //            case 1:
        //                {
        //                    list = listMonth2;
        //                    break;
        //                }
        //            case 2:
        //                {
        //                    list = listMonth3;
        //                    break;
        //                }
        //            case 3:
        //                {
        //                    list = listMonth4;
        //                    break;
        //                }
        //            case 4:
        //                {
        //                    list = listMonth5;
        //                    break;
        //                }
        //            case 5:
        //                {
        //                    list = listMonth6;
        //                    break;
        //                }
        //            case 6:
        //                {
        //                    list = listMonth7;
        //                    break;
        //                }
        //            case 7:
        //                {
        //                    list = listMonth8;
        //                    break;
        //                }
        //            case 8:
        //                {
        //                    list = listMonth9;
        //                    break;
        //                }
        //            case 9:
        //                {
        //                    list = listMonth10;
        //                    break;
        //                }
        //            case 10:
        //                {
        //                    list = listMonth11;
        //                    break;
        //                }
        //            case 11:
        //                {
        //                    list = listMonth12;
        //                    break;
        //                }
        //        }
        //        #endregion
        //        // dữ liệu mỗi tháng ứng mới 1 phần tử trong mảng (mảng có 12 phần tử ứng với 12 tháng)
        //        soNguoiMuonSachTrongNam[i] = nghiepVu.DemSoNguoiMuonSach(list);
        //        soNguoiTraTreTrongNam[i] = nghiepVu.DemSoNguoiTraTre(list);
        //        soNguoiKhongTraTrongNam[i] = nghiepVu.DemSoNguoiKhongTra(list);
        //        soPhieuMuonTrongNam[i] = nghiepVu.DemSoPhieuMuon(list);
        //        soSachDuocMuonTrongNam[i] = nghiepVu.DemSoSachDuocMuon(list, userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
        //    }
        //    // truyền dữ liệu thống kê vào từng quý trong năm
        //    for (int i = 0; i < 4; i++)
        //    {
        //        List<PhieuMuon> list = new List<PhieuMuon>();
        //        switch (i)
        //        {
        //            case 0:
        //                {
        //                    list = listQuy1;
        //                    break;
        //                }
        //            case 1:
        //                {
        //                    list = listQuy2;
        //                    break;
        //                }
        //            case 2:
        //                {
        //                    list = listQuy3;
        //                    break;
        //                }
        //            case 3:
        //                {
        //                    list = listQuy4;
        //                    break;
        //                }
        //        }
        //        // dữ liệu mỗi quý ứng với từng phần tử trong mảng (mảng có 4 phần tử ứng với 4 quý)
        //        soNguoiMuonSachTrongQuy[i] = nghiepVu.DemSoNguoiMuonSach(list);
        //        soNguoiTraTreTrongQuy[i] = nghiepVu.DemSoNguoiTraTre(list);
        //        soNguoiKhongTraTrongQuy[i] = nghiepVu.DemSoNguoiKhongTra(list);
        //        soPhieuMuonTrongQuy[i] = nghiepVu.DemSoPhieuMuon(list);
        //        soSachDuocMuonTrongQuy[i] = nghiepVu.DemSoSachDuocMuon(list, userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
        //    }
        //    // Khai báo list chứa dữ liệu thống kê của từng ngày trong tháng 
        //    //List<int> lsoNgayTrongThang = new List<int>();
        //    //List<int> lsoPMTrongNgay = new List<int>();
        //    //List<int> lsoNguoiMuonTrongNgay = new List<int>();
        //    //List<int> lsoSachDuocMuonTrongNgay = new List<int>();
        //    //List<int> lsoNguoiKhongTraTrongNgay = new List<int>();
        //    //List<int> lsoNguoiTraTreTrongNgay = new List<int>();

        //    // Chọn số ngày cho từng tháng
        //    int soNgayTrongThang = 0;
        //    switch (month)
        //    {
        //        case 1:
        //        case 3:
        //        case 5:
        //        case 7:
        //        case 8:
        //        case 10:
        //        case 12:
        //            {
        //                soNgayTrongThang = 31;
        //                break;
        //            }
        //        case 4:
        //        case 6:
        //        case 9:
        //        case 11:
        //            {
        //                soNgayTrongThang = 30;
        //                break;
        //            }
        //        case 2:
        //            {
        //                if (((year % 4 == 0) && (year % 100 != 0)) || (year % 400 == 0))
        //                    soNgayTrongThang = 29;
        //                else
        //                    soNgayTrongThang = 28;
        //                break;
        //            }

        //    }
        //    //// chọn ra những ngày nào có phiếu mượn để gắn vào list 
        //    //for (int i = 0; i < 31; i++)
        //    //{

        //    //    lsoNgayTrongThang.Add(i);
        //    //    lsoPMTrongNgay.Add(soPhieuMuonTrongThang[i]);
        //    //    lsoNguoiMuonTrongNgay.Add(soNguoiMuonSachTrongThang[i]);
        //    //    lsoSachDuocMuonTrongNgay.Add(soSachDuocMuonTrongThang[i]);
        //    //    lsoNguoiKhongTraTrongNgay.Add(soNguoiKhongTraTrongThang[i]);
        //    //    lsoNguoiTraTreTrongNgay.Add(soNguoiTraTreTrongThang[i]);
        //    //}

        //    JsonResult result = new JsonResult();
        //    // Chuyền dữ liệu vào Model
        //    model = new BieuDoPhieuMuonViewModel
        //    {
        //        // Thống kê trong Năm (chia ra 12 tháng)
        //        lsoPhieuMuonTrongNam = soPhieuMuonTrongNam,

        //        lsoNguoiMuonSachTrongNam = soNguoiMuonSachTrongNam,
        //        lsoSachDuocMuonTrongNam = soSachDuocMuonTrongNam,
        //        lsoNguoiKhongTraTrongNam = soNguoiKhongTraTrongNam,
        //        lsoNguoiTraTreTrongNam = soNguoiTraTreTrongNam,
        //        // Thống kê trong Quý (chia ra 4 Quý)          
        //        lsoPhieuMuonTrongQuy = soPhieuMuonTrongQuy,
        //        lsoNguoiMuonSachTrongQuy = soNguoiMuonSachTrongQuy,
        //        lsoSachDuocMuonTrongQuy = soSachDuocMuonTrongQuy,
        //        lsoNguoiKhongTraTrongQuy = soNguoiKhongTraTrongQuy,
        //        lsoNguoiTraTreTrongQuy = soNguoiTraTreTrongQuy,
        //        // Thống kê trong Tháng (chia ra 31 ngày)

        //        SoNgayTrongThang = soNgayTrongThang,
        //        lsoPMTrongNgay = soPhieuMuonTrongThang,
        //        lsoNguoiMuonTrongNgay = soNguoiMuonSachTrongThang,
        //        lsoSachDuocMuonTrongNgay = soSachDuocMuonTrongThang,
        //        lsoNguoiKhongTraTrongNgay = soNguoiKhongTraTrongThang,
        //        lsoNguoiTraTreTrongNgay = soNguoiTraTreTrongThang
        //    };

        //    result.Data = model;
        //    result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

        //    return result;
        //}


    }
}