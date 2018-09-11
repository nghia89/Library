using BiTech.Library.BLL.DBLogic;
using BiTech.Library.Controllers.BaseClass;
using BiTech.Library.DTO;
using BiTech.Library.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Aspose.Cells;
using System.Collections;
using System.IO;
using BiTech.Library.Models.ViewDataIF;
using System.Drawing;
using BiTech.Library.Helpers;

namespace BiTech.Library.Controllers
{
    public class ExportreportController : BaseController
    {
        NghiepVuThongKe nghiepVu;
        static List<string> list_tk = new List<string>();// List chứ số sách được mượn và số người mượn sách

        public ExportreportController()
        {
            nghiepVu = new NghiepVuThongKe();
            new Aspose.Cells.License().SetLicense(LicenseHelper.License.LStream);
        }

        // GET: exportreport
        public ActionResult Index()
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            var _ThongTinThuVien = new ThongTinThuVienLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);

            #endregion

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



        public ActionResult ExportTTSach(int? year)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            var _thongKeLogic = new ThongKeLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            #endregion
            #region Khai báo
            if (year == null)
            {
                year = DateTime.Now.Year;
            }
            // 1 năm có 12 tháng
            int[] soNguoiMuonSachTrongNam = new int[12];
            int[] soSachDuocMuonTrongNam = new int[12];

            List<ThongTinMuonSach> listPhieuMuon = _thongKeLogic.GetAllTTMS();
            List<ThongTinMuonSach> listYearSelected = new List<ThongTinMuonSach>();

            DateTime ngayTraNull = DateTime.ParseExact("01-01-0001", "dd-MM-yyyy", null);// Datatime NULL trong mongoDB khi xuất ra có dạng "01-01-0001"                                                         
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
                #region Lấy list PM theo Tháng và Năm
                DateTime ngayMuon = item.NgayGioMuon;
                // Ngày mượn            
                if (ngayMuon.Year == year)
                {
                    listYearSelected.Add(item);
                }
                #endregion
            }
            // -----------------------THÔNG TIN THỐNG KÊ-----------------------  
            #region Muon Sach
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
            //   List<ThongTinMuonSach>[] arrTTSachMuonMonth = new List<ThongTinMuonSach>[31];
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
                soSachDuocMuonTrongNam[i] = nghiepVu.DemSoSachDuocMuon(list, userdata.MyApps[_AppCode].ConnectionString, userdata.MyApps[_AppCode].DatabaseName);
            }
            #endregion
            #endregion
            List<int> listInfo = new List<int>();
            for (int i = 0; i < 12; i++)
            {
                listInfo.Add(soNguoiMuonSachTrongNam[i]);// số chẳn là Số Người 
                listInfo.Add(soSachDuocMuonTrongNam[i]);// số lẻ là Số Sách
            }

            string fileName = string.Concat("ExportTTSach.xlsx");
            string fileNameOut = string.Concat("OutExportTTSach.xlsx");
            var folderReport = "/Reports";
            string filePath = System.Web.HttpContext.Current.Server.MapPath(folderReport);
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            string fullPath = Path.Combine(filePath, fileName);

            // Khai báo
            Workbook wb = new Workbook(fullPath);
            Worksheet ws = wb.Worksheets[0];
            //// Tên header
            //ws.Cells["A1"].PutValue(year.ToString());
            ////ws.Cells["B2"].PutValue("Tháng");
            //ws.Cells["C2"].PutValue("Số lượng sách được mượn");
            //ws.Cells["D2"].PutValue("Số lượng người mượn sách");

            // Import data 
            ArrayListVM arrListInfor = new ArrayListVM();

            for (var i = 0; i < listInfo.Count(); i++)
            {
                if (i % 2 != 0)
                {
                    arrListInfor.ArrayList1.Add(listInfo[i]);
                }
                else
                {
                    arrListInfor.ArrayList2.Add(listInfo[i]);
                }
            }

            ArrayList arrayListThang = new ArrayList();
            arrayListThang.Add("Tháng 01");
            arrayListThang.Add("Tháng 02");
            arrayListThang.Add("Tháng 03");
            arrayListThang.Add("Tháng 04");
            arrayListThang.Add("Tháng 05");
            arrayListThang.Add("Tháng 06");
            arrayListThang.Add("Tháng 07");
            arrayListThang.Add("Tháng 08");
            arrayListThang.Add("Tháng 09");
            arrayListThang.Add("Tháng 10");
            arrayListThang.Add("Tháng 11");
            arrayListThang.Add("Tháng 12");

            ArrayList arrayYear = new ArrayList();
            arrayYear.Add("Bảng Thống Kê Số  Sách  số người mượn trong năm "+year);

            // Thêm vào vị trí nào trong excel
            ws.Cells.ImportArrayList(arrListInfor.ArrayList1, 4, 1, true);
            ws.Cells.ImportArrayList(arrListInfor.ArrayList2, 4, 2, true);
            ws.Cells.ImportArrayList(arrayListThang, 4, 0, true);
            ws.Cells.ImportArrayList(arrayYear, 1, 0, true);

            // Save 
            ws.AutoFitColumns();
            string FoderFileOut = Path.Combine(filePath, fileNameOut);
            wb.Save(FoderFileOut, SaveFormat.Xlsx);

            string filename = fileName;
            string filepath = AppDomain.CurrentDomain.BaseDirectory + folderReport + "/" + fileNameOut;
            byte[] filedata = System.IO.File.ReadAllBytes(filepath);
            string contentType = MimeMapping.GetMimeMapping(filepath);

            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = filename,
                Inline = true,
            };

            Response.AppendHeader("Content-Disposition", cd.ToString());

            return File(filedata, contentType);
        }

        //xuất tất cả sách theo trạng thái có cho mượn hay không
        public ActionResult ExportSLSachTT()
        {
            #region lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return Json(null, JsonRequestBehavior.AllowGet);
            #endregion

            SachLogic _sachLogic = new SachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            SoLuongSachTrangThaiLogic _SoLuongSachTrangThaiLogic = new SoLuongSachTrangThaiLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            TrangThaiSachLogic _trangThaiSachLogic = new TrangThaiSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);

            var getAllTTSach = _trangThaiSachLogic.GetAll();
            var getAllSLTTS = _SoLuongSachTrangThaiLogic.GetAll();
            var getAllSach = _sachLogic.getAll();
            List<TongSlSachConMuonVM> ListAll = new List<TongSlSachConMuonVM>();

            foreach (var itemSach in getAllSach)
            {
                TongSlSachConMuonVM ItemList = new TongSlSachConMuonVM();
                ItemList.Maks = itemSach.MaKiemSoat;
                ItemList.TenSach = itemSach.TenSach;
                ItemList.TongSl = itemSach.SoLuongTong;
                ItemList.SLChoMuon = 0;
                ItemList.SLKHChoMuon = 0;
                foreach (var itemSLTTS in getAllSLTTS)
                {
                    if (itemSach.Id == itemSLTTS.IdSach)
                    {
                        foreach (var itemTTS in getAllTTSach)
                        {
                            if (itemSLTTS.IdTrangThai == itemTTS.Id)
                            {
                                if (itemTTS.TrangThai == true)
                                    ItemList.SLChoMuon += itemSLTTS.SoLuong;
                                else
                                    ItemList.SLKHChoMuon += itemSLTTS.SoLuong;
                            }

                        }
                    }
                }
                ListAll.Add(ItemList);
            }

            //ArrayList arrayList = new ArrayList(ListAll);
            string fileName = string.Concat("ExportSlSach.xlsx");
            string fileNameOut = string.Concat("OutExportSlSach.xlsx");
            var folderReport = "/Reports";
            string filePath = System.Web.HttpContext.Current.Server.MapPath(folderReport);
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            string fullPath = Path.Combine(filePath, fileName);

            // Khai báo
            Workbook workbook = new Workbook(fullPath);
            Worksheet ws = workbook.Worksheets[0];

            ArrayList TitleExcel = new ArrayList();
            TitleExcel.Add("Mã kiễm soát");
            TitleExcel.Add("Tên Sách");
            TitleExcel.Add("Số Lượng Tổng");
            TitleExcel.Add("Số Lượng Sách Còn Cho Mượn");
            TitleExcel.Add("Số Lượng Sách Còn Không Cho Mượn");

            ws.Cells.ImportCustomObjects(ListAll, 3, 0, null);
            ws.Cells.ImportArrayList(TitleExcel, 3, 0, false);

            ws.AutoFitColumns();

            Style _S_All_Borders = new Style();
            _S_All_Borders.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
            _S_All_Borders.Borders[BorderType.TopBorder].Color = Color.Black;
            _S_All_Borders.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;
            _S_All_Borders.Borders[BorderType.BottomBorder].Color = Color.Black;
            _S_All_Borders.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
            _S_All_Borders.Borders[BorderType.LeftBorder].Color = Color.Black;
            _S_All_Borders.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
            _S_All_Borders.Borders[BorderType.RightBorder].Color = Color.Black;
            var _SF_Borders = new StyleFlag() { Borders = true };

            var range = ws.Cells.CreateRange(3, 0, ListAll.Count + 1, 5);
            range.ApplyStyle(_S_All_Borders, _SF_Borders);

            string FoderFileOut = Path.Combine(filePath, fileNameOut);
            workbook.Save(FoderFileOut, SaveFormat.Xlsx);
            string filename = fileName;
            string filepath = AppDomain.CurrentDomain.BaseDirectory + folderReport + "/" + fileNameOut;
            byte[] filedata = System.IO.File.ReadAllBytes(filepath);
            string contentType = MimeMapping.GetMimeMapping(filepath);


            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = filename,
                Inline = true,
            };

            Response.AppendHeader("Content-Disposition", cd.ToString());

            return File(filedata, contentType);
        }
    }
}