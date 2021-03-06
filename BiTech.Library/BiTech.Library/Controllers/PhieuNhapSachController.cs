﻿using BiTech.Library.BLL.DBLogic;
using BiTech.Library.BLL.BarCode_QR;
using BiTech.Library.Models;
using BiTech.Library.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using BiTech.Library.Controllers.BaseClass;
using static BiTech.Library.Helpers.Tool;
using System.IO;
using BiTech.Library.Helpers;
using Aspose.Cells;
using System.Threading.Tasks;
using System.Collections;

namespace BiTech.Library.Controllers
{
    [AuthorizeRoles(true, Role.CustomerAdmin, Role.CustomerUser)]
#if DEBUG
    [AllowAnonymous]
#endif
    public class PhieuNhapSachController : BaseController
    {
        SachCommon sachCommon;
        XuLyChuoi xuLyChuoi;
        public PhieuNhapSachController()
        {
            sachCommon = new SachCommon();
            xuLyChuoi = new XuLyChuoi();
            new Aspose.Cells.License().SetLicense(LicenseHelper.License.LStream);
        }
        public ActionResult Index(int? page)
        {
            PhieuNhapSachLogic _PhieuNhapSachLogic = new PhieuNhapSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            int PageSize = 20;
            int PageNumber = (page ?? 1);
            var lst = _PhieuNhapSachLogic.Getall();
            List<PhieuNhapSachModels> lstpns = new List<PhieuNhapSachModels>();
            foreach (var item in lst)
            {
                PhieuNhapSachModels pns = new PhieuNhapSachModels()
                {
                    IdPhieuNhap = item.Id,
                    IdUserAdmin = item.IdUserAdmin,
                    GhiChu = item.GhiChu,
                    NgayNhap = item.CreateDateTime,
                    UserName = item.UserName
                };
                lstpns.Add(pns);
            }

            ViewBag.pageSize = PageSize;
            ViewBag.pages = PageNumber;
            ViewBag.number = lst.Count;
            return View(lstpns.OrderByDescending(x => x.NgayNhap).ToPagedList(PageNumber, PageSize));
        }

        public ActionResult Details(string id)
        {
            ChiTietNhapSachLogic _ChiTietNhapSachLogic = new ChiTietNhapSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            TrangThaiSachLogic _TrangThaiSachLogic = new TrangThaiSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            SachLogic _SachLogic = new SachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            PhieuNhapSachLogic _PhieuNhapSachLogic = new PhieuNhapSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);

            var model = _ChiTietNhapSachLogic.GetAllChiTietById(id);
            var phieunhap = _PhieuNhapSachLogic.GetById(id);

            if (phieunhap == null || model == null)
                return RedirectToAction("NotFound", "Error");

            PhieuNhapSachModels pns = new PhieuNhapSachModels()
            {

                IdUserAdmin = phieunhap.IdUserAdmin,
                GhiChu = phieunhap.GhiChu,
                NgayNhap = phieunhap.CreateDateTime

            };
            List<ChiTietNhapSachViewModels> lst = new List<ChiTietNhapSachViewModels>();
            foreach (var item in model)
            {
                ChiTietNhapSachViewModels ctns = new ChiTietNhapSachViewModels();
                ctns.Id = item.Id;
                ctns.IdTinhTrang = item.IdTinhtrang;
                var TinhTrang = _TrangThaiSachLogic.getById(ctns.IdTinhTrang);
                ctns.tenTinhTrang = TinhTrang.TenTT;
                ctns.IdSach = item.IdSach;
                var TenSach = _SachLogic.GetBookById(ctns.IdSach);
                ctns.ten = TenSach.TenSach;
                ctns.soLuong = item.SoLuong;
                ctns.GhiChuDon = item.GhiChu;
                ctns.MaKiemSoat = _SachLogic.GetById(item.IdSach).MaKiemSoat;
                lst.Add(ctns);

            }
            ViewBag.lstctnhap = lst;

            return View(pns);
        }

        public ActionResult TaoPhieuNhapSach()
        {
            TrangThaiSachLogic _TrangThaiSachLogic = new TrangThaiSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);

            ViewBag.listtt = _TrangThaiSachLogic.GetAll();
            return View();
        }

        [HttpPost]
        public ActionResult TaoPhieuNhapSach(PhieuNhapSachModels model)
        {
            SachLogic _SachLogic = new SachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            PhieuNhapSachLogic _PhieuNhapSachLogic = new PhieuNhapSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            ChiTietNhapSachLogic _ChiTietNhapSachLogic = new ChiTietNhapSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            TrangThaiSachLogic _TrangThaiSachLogic = new TrangThaiSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            SachCaBietLogic _SachCaBietLogic = new SachCaBietLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);

            if (model.listChiTietJsonString.Count > 0)
            {
                PhieuNhapSach pns = new PhieuNhapSach()
                {
                    GhiChu = model.GhiChu,
                    IdUserAdmin = _UserAccessInfo.Id,
                    UserName = _UserAccessInfo.FullName
                };

                string idPhieuNhap = _PhieuNhapSachLogic.NhapSach(pns);

                if (!String.IsNullOrEmpty(idPhieuNhap))
                {
                    foreach (var json in model.listChiTietJsonString)
                    {
                        var ctModel = Newtonsoft.Json.JsonConvert.DeserializeObject<ChiTietNhapSachViewModels>(json);

                        var ctns = new ChiTietNhapSach()
                        {
                            IdPhieuNhap = idPhieuNhap,
                            IdSach = ctModel.IdSach,
                            SoLuong = ctModel.soLuong,
                            CreateDateTime = DateTime.Now,
                            IdTinhtrang = ctModel.IdTinhTrang,
                            GhiChu = ctModel.GhiChuDon
                        };

                        _ChiTietNhapSachLogic.Insert(ctns);
                        //Insert vào bảng SachCaBiet từng cuốn một
                        ///MaKSCB = MaKS.MaCB , Ex: 0001.1, 0001.2 ... + QR
                        int STTHienTai = _SachLogic.GetByID_IsDeleteFalse(ctModel.IdSach).STTMaCB; //Lấy STTCB của đầu sách hiện tại

                        SachCaBiet scb = new SachCaBiet()
                        {
                            IdSach = ctModel.IdSach,
                            IdTrangThai = ctModel.IdTinhTrang,
                            TenSach = _SachLogic.GetByID_IsDeleteFalse(ctModel.IdSach).TenSach,
                        };
                        for (int i = 0; i < ctModel.soLuong; i++)
                        {
                            scb.MaKSCB = ctModel.MaKiemSoat + "." + (++STTHienTai);
                            _SachCaBietLogic.Insert(scb);
                        }
                        ///Lưu mã QR
                        var lstSachCB = _SachCaBietLogic.GetListCaBietFromIdSach(ctModel.IdSach);
                        try
                        {
                            string physicalWebRootPath = Server.MapPath("/");
                            foreach (var item in lstSachCB)
                            {
                                SachCaBiet temp = sachCommon.LuuMaVachSach_SachCaBiet(physicalWebRootPath, item, null, _SubDomain);
                                if (temp != null)
                                {
                                    item.QRlink = temp.QRlink;
                                    item.QRData = temp.QRData;
                                    _SachCaBietLogic.Update(item);
                                }
                            }
                        }
                        catch
                        {
                        }
                        //Update bảng Sach 
                        //Cập nhật STT Cá biệt cho mỗi đầu sách  
                        var modelSach = _SachLogic.GetBook_NonDelete_ByMKS(ctModel.MaKiemSoat);
                        modelSach.SoLuongTong += ctModel.soLuong;
                        modelSach.SoLuongConLai += ctModel.soLuong;
                        modelSach.STTMaCB = STTHienTai;
                        _SachLogic.Update(modelSach);
                    }
                    return RedirectToAction("Index");
                }
            }

            ViewBag.listtt = _TrangThaiSachLogic.GetAll();
            ModelState.Clear();
            return View();
        }

        public ActionResult ImportFromExcel()
        {
            return View();
        }

        #region Tai        
        [HttpPost]
        public async Task<ActionResult> PreviewImport(HttpPostedFileBase file, string ghiChu)
        {
            if (file != null)
            {
                // Chỉ chấp nhận file *.xls, *.xlsx
                if (Path.GetExtension(file.FileName).EndsWith(".xls") || Path.GetExtension(file.FileName).EndsWith(".xlsx"))
                {
                    var viewModel = new ImportExcelPNSViewModel();
                    // Đường dẫn để lưu nội dung file Excel
                    string uploadFolder = GetUploadFolder(UploadFolder.FileExcel, _SubDomain);
                    string uploadFileName = null;
                    string physicalWebRootPath = Server.MapPath("/");
                    uploadFileName = Path.Combine(physicalWebRootPath, uploadFolder, file.FileName);
                    string location = Path.GetDirectoryName(uploadFileName);
                    if (!Directory.Exists(location))
                    {
                        Directory.CreateDirectory(location);
                    }
                    // Ghi nội dung file Excel vào tệp tạm
                    using (var fileStream = new FileStream(uploadFileName, FileMode.Create))
                    {
                        // Lưu                
                        file.InputStream.CopyTo(fileStream);
                        string sourceSavePath = uploadFileName;
                        Workbook workBook = new Workbook(sourceSavePath);
                        Worksheet workSheet = workBook.Worksheets[0];
                        // Số dòng, đầu tiên chứ dữ liệu
                        int firstRow = workSheet.Cells.FirstCell.Row + 1;
                        int firstColumn = workSheet.Cells.FirstCell.Column;
                        // Số dòng, cột tối đa
                        var maxRows = workSheet.Cells.MaxDataRow - workSheet.Cells.MinDataRow;
                        var maxColumns = (workSheet.Cells.MaxDataColumn + 1) - workSheet.Cells.MinDataColumn;                        
                        viewModel.RawDataList = new List<string[]>();
                        // Đọc từng dòng trong Excel
                        for (int rowIndex = firstRow; rowIndex <= firstRow + maxRows; rowIndex++)
                        {
                            // Xác định dòng dữ liệu này có bị trống dữ liệu CẢ DÒNG hay không.
                            var isEmptyRow = true;
                            // Tạo từng dòng thông tin
                            var rowData = new string[maxColumns];
                            // Lấy nội dung từng cột dữ liệu trong hàng hiện tại.
                            for (int columnIndex = firstColumn; columnIndex <= firstColumn + maxColumns; columnIndex++)
                            {
                                // Đọc nội dung ô
                                var cellData = (workSheet.Cells[rowIndex, columnIndex]).Value?.ToString() ?? "";
                                if (false == string.IsNullOrEmpty(cellData))
                                {
                                    // Lấy nội dung của Ô, lưu vào bộ nhớ
                                    rowData[columnIndex - firstColumn] = cellData;
                                    // Xác định Row hiện tại không bị trống dữ liệu
                                    isEmptyRow = false;
                                }
                            }
                            #region Nếu dòng không trống thì thêm vào danh sách đã quét.
                            if (isEmptyRow == false)
                            {
                                viewModel.RawDataList.Add(rowData);
                            }
                            #endregion                            
                        }
                        workBook.Dispose();
                    }
                    // Xóa file đã lưu tạm
                    System.IO.File.Delete(uploadFileName);
                    viewModel.TotalEntry = viewModel.RawDataList.Count;
                    if (String.IsNullOrEmpty(ghiChu))
                        ViewBag.NullGhiChu = true;
                    viewModel.GhiChu = ghiChu;
                    return View(viewModel);
                }
                else
                {
                    return Json(new { status = "fail", message = "Tập tin không đúng định dạng của Excel, vui lòng kiểm tra lại" });
                }
            }
            return Json(new { status = "fail", message = "Quá trình Upload bị gián đoạn. Vui lòng thữ lại" });
        }

        [HttpPost]
        public ActionResult ImportSave(List<string[]> data, string ghiChu)
        {
            PhieuNhapSachLogic _PhieuNhapSachLogic = new PhieuNhapSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            ChiTietNhapSachLogic _ChiTietNhapSachLogic = new ChiTietNhapSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            SachLogic _SachLogic = new SachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            TrangThaiSachLogic _TrangThaiSachLogic = new TrangThaiSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            SoLuongSachTrangThaiLogic _SoLuongSachTrangThaiLogic = new SoLuongSachTrangThaiLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            SachCaBietLogic _SachCaBietLogic = new SachCaBietLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            var listAll = new List<ChiTietNhapSach>();
            var ListFail = new List<ChiTietNhapSach>();
            var ListSuccess = new List<ChiTietNhapSach>();
            List<ArrayList> ListShow = new List<ArrayList>();
            var model = new ImportExcelPNSViewModel();
            foreach (var item in data)
            {
                int soLuong = 0;
                if (String.IsNullOrEmpty(item[2].ToString().Trim()))
                    soLuong = -1;
                else
                    soLuong = Int32.Parse(item[2].ToString().Trim());
                ChiTietNhapSach ctns = new ChiTietNhapSach
                {
                    IdSach = item[1].ToString().Trim(),
                    SoLuong = soLuong,
                    IdTinhtrang = item[3].ToString().Trim(),
                    GhiChu = item[4].ToString().Trim(),
                };
                listAll.Add(ctns);
            }
            if (listAll != null)
            {
                // Tạo phiếu nhập sách
                PhieuNhapSach pns = new PhieuNhapSach()
                {
                    GhiChu = ghiChu,
                    IdUserAdmin = _UserAccessInfo.Id,
                    UserName = _UserAccessInfo.UserName
                };
                string idPhieuNhap = _PhieuNhapSachLogic.NhapSach(pns);
                var listAllTTS = _TrangThaiSachLogic.GetAll();
                // 
                foreach (var item in listAll)
                {
                    // Mã sách rỗng
                    if (String.IsNullOrEmpty(item.IdSach.Trim()))
                    {
                        item.ListError.Add("Rỗng ô nhập \"Mã sách\"");
                    }
                    else
                    {
                        // Kiểm tra mã sách không tồn tại
                        var sach = _SachLogic.GetByMaMaKiemSoat(item.IdSach);
                        if (sach == null)
                        {
                            item.ListError.Add("Mã sách không tồn tại");
                            item.IsExist = false;
                        }
                    }
                    // Số lượng rỗng
                    if (item.SoLuong == -1)
                    {
                        item.ListError.Add("Rỗng ô nhập \"Số lượng\"");
                    }
                    // Trạng thái sách rỗng
                    if (String.IsNullOrEmpty(item.IdTinhtrang.Trim()))
                    {
                        item.ListError.Add("Rỗng ô nhập \"Trạng thái sách\"");
                    }
                    // Lưu vào CSDL Chi Tiết Phiếu Nhập không bị lỗi
                    if (item.ListError.Count == 0)
                    {
                        // Tạo phiếu chi tiết nhập sách                        
                        var sach = _SachLogic.GetByMaMaKiemSoat(item.IdSach);
                        TrangThaiSach trangThaiSach = _TrangThaiSachLogic.GetByName(item.IdTinhtrang.Trim());// Trạng thái sách
                        // Nếu trạng thái sách chưa tồn tại thì thêm mới
                        if (trangThaiSach == null)
                        {
                            trangThaiSach = new TrangThaiSach
                            {
                                TenTT = xuLyChuoi.ChuanHoaChuoi(item.IdTinhtrang),
                                TrangThai = true
                            };
                            _TrangThaiSachLogic.Insert(trangThaiSach);
                        }
                        item.IdSach = sach.Id;
                        item.IdPhieuNhap = idPhieuNhap;
                        item.IdTinhtrang = trangThaiSach.Id;

                        _ChiTietNhapSachLogic.Insert(item);
                        // update số lượng sách trạng thái                        
                        var sltt = _SoLuongSachTrangThaiLogic.getBy_IdSach_IdTT(sach.Id, trangThaiSach.Id);
                        if (sltt != null)
                        {
                            sltt.SoLuong += item.SoLuong;
                            _SoLuongSachTrangThaiLogic.Update(sltt);
                        }
                        else
                        {
                            sltt = new SoLuongSachTrangThai();
                            sltt.IdSach = sach.Id;
                            sltt.IdTrangThai = trangThaiSach.Id;
                            sltt.SoLuong = item.SoLuong;
                            _SoLuongSachTrangThaiLogic.Insert(sltt);
                        }
                        ///MaKSCB = MaKS.MaCB , Ex: 0001.1, 0001.2 ... + QR
                        Sach dauSach = _SachLogic.GetByID_IsDeleteFalse(item.IdSach);
                        int STTHienTai = dauSach.STTMaCB; //Lấy STTCB của đầu sách hiện tại
                        string maKiemSoat = dauSach.MaKiemSoat;
                        SachCaBiet scb = new SachCaBiet()
                        {
                            IdSach = item.IdSach,
                            IdTrangThai = item.IdTinhtrang,
                            TenSach = _SachLogic.GetByID_IsDeleteFalse(item.IdSach).TenSach,
                        };
                        for (int i = 0; i < item.SoLuong; i++)
                        {
                            scb.MaKSCB = maKiemSoat + "." + (++STTHienTai);
                            _SachCaBietLogic.Insert(scb);
                        }
                        ///Lưu mã QR
                        var lstSachCB = _SachCaBietLogic.GetListCaBietFromIdSach(item.IdSach);
                        try
                        {
                            string physicalWebRootPath = Server.MapPath("/");
                            foreach (var sachCB in lstSachCB)
                            {
                                SachCaBiet temp = sachCommon.LuuMaVachSach_SachCaBiet(physicalWebRootPath, sachCB, null, _SubDomain);
                                if (temp != null)
                                {
                                    sachCB.QRlink = temp.QRlink;
                                    sachCB.QRData = temp.QRData;
                                    _SachCaBietLogic.Update(sachCB);
                                }
                            }
                        }
                        catch { }
                        //Update bảng Đầu Sách                       
                        dauSach.SoLuongTong += item.SoLuong;
                        dauSach.STTMaCB = STTHienTai;
                        dauSach.SoLuongConLai += item.SoLuong;
                        _SachLogic.Update(dauSach);                        
                        ListSuccess.Add(item);
                    }
                    else
                        ListFail.Add(item);
                }
                #region Tạo file excel cho ds Chi Tiết Phiếu Nhập bị lỗi   
                if (ListFail.Count > 0)
                {
                    Workbook wb = new Workbook();
                    Worksheet ws = wb.Worksheets[0];
                    // Tên header
                    Style style = new Style();
                    style.Pattern = BackgroundType.Solid;
                    style.ForegroundColor = System.Drawing.Color.FromArgb(139, 195, 234);
                    style.Font.Size = 20;
                    style.Font.IsBold = true;
                    style.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
                    style.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
                    style.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
                    style.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;

                    Style styleData = new Style();
                    styleData.Font.Size = 18;
                    styleData.Font.Name = "Times New Roman";
                    styleData.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
                    styleData.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
                    styleData.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
                    styleData.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;

                    Style styleError = new Style();
                    styleError.Pattern = BackgroundType.Solid;
                    styleError.ForegroundColor = System.Drawing.Color.LightPink;
                    styleError.Font.Size = 18;
                    styleError.Font.Name = "Times New Roman";
                    styleError.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
                    styleError.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
                    styleError.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
                    styleError.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;

                    ws.Cells["A1"].PutValue("STT");
                    ws.Cells["A1"].SetStyle(style);
                    ws.Cells["B1"].PutValue("Mã sách");
                    ws.Cells["B1"].SetStyle(style);
                    ws.Cells["C1"].PutValue("Số lượng");
                    ws.Cells["C1"].SetStyle(style);
                    ws.Cells["D1"].PutValue("Trạng thái sách");
                    ws.Cells["D1"].SetStyle(style);
                    ws.Cells["E1"].PutValue("Ghi chú");
                    ws.Cells["E1"].SetStyle(style);
                    // Import data             
                    int firstRow = 1;
                    int firstColumn = 0;
                    int stt = 1;
                    model.ArrRows = new bool[ListFail.Count + 1];
                    foreach (var item in ListFail)
                    {
                        ArrayList arrList = new ArrayList();
                        arrList.Add(stt);
                        arrList.Add(item.IdSach);
                        if (item.SoLuong == -1)
                            arrList.Add("");
                        else
                            arrList.Add(item.SoLuong);
                        arrList.Add(item.IdTinhtrang);
                        arrList.Add(item.GhiChu);
                        string errorExcel = null;
                        bool isFirst = true;// xét dấu phẩy cho chuỗi thông báo
                        foreach (var err in item.ListError)
                        {
                            if (isFirst)
                                errorExcel += err;
                            else
                                errorExcel += ", " + err;
                            isFirst = false;
                        }
                        ws.Cells.ImportArrayList(arrList, firstRow, firstColumn, false);
                        // Set style màu sắc
                        for (int i = firstColumn; i < firstColumn + 5; i++)
                        {
                            ws.Cells[firstRow, i].SetStyle(styleData);
                        }
                        if (String.IsNullOrEmpty(item.IdSach.Trim()) || item.IsExist == false)
                            ws.Cells[firstRow, firstColumn + 1].SetStyle(styleError);

                        if (item.SoLuong == -1)
                            ws.Cells[firstRow, firstColumn + 2].SetStyle(styleError);

                        if (String.IsNullOrEmpty(item.IdTinhtrang.ToString().Trim()))
                            ws.Cells[firstRow, firstColumn + 3].SetStyle(styleError);


                        model.ArrRows[firstRow] = true;// khởi tạo dòng true (mã sách tồn tại)
                        if (item.IsExist == false)
                        {
                            model.ArrRows[firstRow] = false;
                        }
                        // K lưu vào file Excel, chỉ xuất lên table
                        arrList.Add(errorExcel);
                        ListShow.Add(arrList);
                        firstRow++;
                        stt++;
                    }
                    ws.AutoFitColumns();
                    // Save
                    string fileName = "DsPhieuNhapSachBiLoi.xlsx";
                    string physicalWebRootPath = Server.MapPath("/");
                    string uploadFolder = GetUploadFolder(UploadFolder.FileExcel, _SubDomain);
                    string uploadFileName = null;
                    uploadFileName = Path.Combine(physicalWebRootPath, uploadFolder, fileName);
                    string location = Path.GetDirectoryName(uploadFileName);
                    if (!Directory.Exists(location))
                    {
                        Directory.CreateDirectory(location);
                    }
                    wb.Save(uploadFileName, SaveFormat.Xlsx);
                    model.FileName = fileName;
                    model.FilePath = uploadFileName;
                }
                #endregion
            }
            model.ListSuccess = ListSuccess;
            model.ListFail = ListFail;
            model.ListShow = ListShow;
            return View(model);
        }

        public ActionResult DowloadExcel(string filePath, string fileName)
        {
            if (fileName == null)
                return RedirectToAction("NotFound", "Error");
            // To do Download                       
            string filepath = filePath;
            byte[] filedata = System.IO.File.ReadAllBytes(filepath);
            string contentType = MimeMapping.GetMimeMapping(filepath);
            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = fileName,
                Inline = true,
            };
            Response.AppendHeader("Content-Disposition", cd.ToString());
            return File(filedata, contentType);
        }
        #endregion
        // Ajax ---

        [HttpGet]
        public JsonResult _GetBookItemById(string maKS, int soLuong, string idtrangthai, string GhiChuDon)
        {
            SachLogic _SachLogic = new SachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            TrangThaiSachLogic _TrangThaiSachLogic = new TrangThaiSachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            if (maKS != null)
            {
                maKS = maKS.Trim();
                GhiChuDon = GhiChuDon.Trim();

                JsonResult result = new JsonResult();
                result.Data = null;
                result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

                if (!string.IsNullOrEmpty(maKS) && !string.IsNullOrEmpty(idtrangthai) && soLuong > 0)
                {
                    var book = _SachLogic.GetBook_NonDelete_ByMKS(maKS);

                    var tt = _TrangThaiSachLogic.getById(idtrangthai);
                    if (book != null && tt != null)
                    {
                        ChiTietNhapSachViewModels pp = new ChiTietNhapSachViewModels()
                        {
                            IdSach = book.Id,
                            ten = book.TenSach,
                            soLuong = soLuong,
                            IdTinhTrang = idtrangthai,
                            tenTinhTrang = tt.TenTT,
                            MaKiemSoat = book.MaKiemSoat,
                            GhiChuDon = GhiChuDon
                        };
                        result.Data = pp;
                    }
                }
                return result;
            }
            return null;
        }

        [HttpPost]
        public ActionResult Autocomplete(string a)
        {
            SachLogic _SachLogic =
               new SachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);

            var ListTD = (from N in _SachLogic.getAllSach()
                          where N.MaKiemSoat.StartsWith(a)
                          select new { N.MaKiemSoat });
            return Json(ListTD, JsonRequestBehavior.AllowGet);
        }

        //VINH
        /// <summary>
        /// idSach là maKS hoặc ISBN
        /// </summary>
        /// <param name="idSach"></param>
        /// <returns></returns>
        public JsonResult GetBookByID(string idSach)
        {
            SachLogic _SachLogicLogic = new SachLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);

            Sach _sach = _SachLogicLogic.GetByMaKiemSoatorISBN(new SachCommon().GetInfo(idSach));

            return Json(_sach, JsonRequestBehavior.AllowGet);
        }
    }
}