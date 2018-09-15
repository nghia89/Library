using BiTech.Library.BLL.DBLogic;
using BiTech.Library.DTO;
using BiTech.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using BiTech.Library.Controllers.BaseClass;
using BiTech.Library.Helpers;
using System.Threading.Tasks;
using static BiTech.Library.Helpers.Tool;
using System.IO;
using Aspose.Cells;
using System.Collections;

namespace BiTech.Library.Controllers
{
    public class DDCController : BaseController
    {
        XuLyChuoi xuLyChuoi;
        public DDCController()
        {
            xuLyChuoi = new XuLyChuoi();
            new Aspose.Cells.License().SetLicense(LicenseHelper.License.LStream);
        }

        public ActionResult Index(int? page)
        {
            DDCLogic _DDCLogic = new DDCLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);

            var list = _DDCLogic.GetAllDDC();
            List<DDC> lst = new List<DDC>();
            foreach (var item in list)
            {
                DDC ks = new DDC()
                {
                    Id = item.Id,
                    MaDDC = item.MaDDC,
                    Ten = item.Ten,
                    CreateDateTime = item.CreateDateTime
                };
                lst.Add(ks);
            }
            int PageSize = 10;
            int PageNumber = (page ?? 1);
            ViewBag.pageSize = PageSize;
            ViewBag.pages = PageNumber;

            return View(lst.OrderBy(x => x.Ten).ToPagedList(PageNumber, PageSize));
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(DDC model)
        {
            DDCLogic _DDCLogic = new DDCLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);

            DDC ddc = new DDC()
            {
                MaDDC = model.MaDDC,
                Ten = model.Ten
            };
            _DDCLogic.Add(ddc);
            return RedirectToAction("Index");
        }

        public ActionResult Edit(string Id)
        {
            DDCLogic _DDCLogic = new DDCLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            var item = _DDCLogic.getById(Id);
            if (item == null)
                return RedirectToAction("NotFound", "Error");
            DDC ddc = new DDC()
            {
                Id = item.Id,
                Ten = item.Ten,
                MaDDC = item.MaDDC,
            };

            return View(ddc);
        }

        [HttpPost]
        public ActionResult Edit(DDC model)
        {
            DDCLogic _DDCLogic = new DDCLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            DDC ddc = new DDC()
            {
                Id = model.Id,
                Ten = model.Ten,
                MaDDC = model.MaDDC,
            };
            _DDCLogic.Update(ddc);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Delete(string Id)
        {
            try
            {
                DDCLogic _DDCLogic = new DDCLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
                var dl = _DDCLogic.getById(Id);
                _DDCLogic.Delete(dl.Id);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return RedirectToAction("NotFound", "Error");
            }
        }

        #region Tai
        public ActionResult ImportFromExcel()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> PreviewImport(HttpPostedFileBase file)
        {
            if (file != null)
            {
                // Chỉ chấp nhận file *.xls, *.xlsx
                if (Path.GetExtension(file.FileName).EndsWith(".xls") || Path.GetExtension(file.FileName).EndsWith(".xlsx"))
                {
                    var viewModel = new ImportExcelDDCViewModel();
                    // Đường dẫn để lưu nội dung file Excel
                    string uploadFolder = GetUploadFolder(Helpers.UploadFolder.FileExcel);
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
                        //
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
        public ActionResult ImportSave(List<string[]> data)
        {
            DDCLogic _DDCLogic = new DDCLogic(Tool.GetConfiguration("ConnectionString"), _UserAccessInfo.DatabaseName);
            var listAll = new List<DDC>();
            var ListFail = new List<DDC>();
            var ListSuccess = new List<DDC>();
            List<ArrayList> ListShow = new List<ArrayList>();
            var model = new ImportExcelDDCViewModel();
            foreach (var item in data)
            {
                // test
                int maDDC = 0;
                if (String.IsNullOrEmpty(item[1].ToString().Trim()))
                    maDDC = -1;
                else
                    maDDC = Int32.Parse(item[1].ToString().Trim());
                DDC ddc = new DDC
                {
                    MaDDC = item[1].ToString().Trim(),
                    Ten = item[2].ToString().Trim(),
                };
                listAll.Add(ddc);
            }
            if (listAll != null)
            {
                foreach (var item in listAll)
                {
                    // Mã DDC rỗng
                    if (String.IsNullOrEmpty(item.MaDDC.Trim()))
                    {
                        item.ListError.Add("Rỗng ô nhập \"Mã DDC\"");
                    }
                    // Thể loại sách rỗng
                    if (String.IsNullOrEmpty(item.Ten.Trim()))
                    {
                        item.ListError.Add("Rỗng ô nhập \"Tên thể loại\"");
                    }
                    if (item.ListError.Count == 0)
                        ListSuccess.Add(item);
                    else
                        ListFail.Add(item);
                }
                #region Lưu vào CSDL ds DDC không bị lỗi  
                if (ListSuccess.Count > 0)
                {
                    foreach (var item in ListSuccess)
                    {
                        _DDCLogic.Add(new DDC { MaDDC = item.MaDDC, Ten = xuLyChuoi.ChuanHoaChuoi(item.Ten) });
                    }
                }
                #endregion
                #region Tạo file excel cho ds DDC bị lỗi   

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
                    ws.Cells["B1"].PutValue("Mã DDC");
                    ws.Cells["B1"].SetStyle(style);
                    ws.Cells["C1"].PutValue("Tên thể loại");
                    ws.Cells["C1"].SetStyle(style);
                    // Import data             
                    int firstRow = 1;
                    int firstColumn = 0;
                    int stt = 1;
                    foreach (var item in ListFail)
                    {
                        ArrayList arrList = new ArrayList();
                        arrList.Add(stt);
                        arrList.Add(item.MaDDC);
                        arrList.Add(item.Ten);
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
                        for (int i = firstColumn; i < firstColumn + 3; i++)
                        {
                            ws.Cells[firstRow, i].SetStyle(styleData);
                        }
                        if (String.IsNullOrEmpty(item.MaDDC.Trim()) || item.IsExist == false)
                            ws.Cells[firstRow, firstColumn + 1].SetStyle(styleError);

                        if (String.IsNullOrEmpty(item.Ten.ToString().Trim()))
                            ws.Cells[firstRow, firstColumn + 2].SetStyle(styleError);

                        // K lưu vào file Excel, chỉ xuất lên table
                        arrList.Add(errorExcel);
                        ListShow.Add(arrList);
                        firstRow++;
                        stt++;
                    }
                    ws.AutoFitColumns();
                    // Save
                    string fileName = "DsTheLoaiSachBiLoi.xlsx";
                    wb.Save(@"D:\Pro Test\pro2\BiTech.Library\BiTech.Library\Upload\FileExcel\" + fileName, SaveFormat.Xlsx);
                    model.FileName = fileName;
                }
                #endregion
            }
            model.ListSuccess = ListSuccess;
            model.ListFail = ListFail;
            model.ListShow = ListShow;
            return View(model);
        }

        public ActionResult DowloadExcel(string fileName)
        {
            if (fileName == null)
                return RedirectToAction("NotFound", "Error");
            // To do Download             
            string filepath = @"D:\Pro Test\pro2\BiTech.Library\BiTech.Library\Upload\FileExcel\" + fileName;
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

    }
}