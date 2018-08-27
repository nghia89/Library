using Aspose.Cells;
using Aspose.Words;
using Aspose.Words.Drawing;
using BiTech.Library.BLL.DBLogic;
using BiTech.Library.DTO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace BiTech.Library.BLL.BarCode_QR
{
    public class ExcelManager : baseAsposeLic
    {
        public ExcelManager() : base()
        {

        }
        public void ConvertCellToHTML(string sourceDir, string outputDir)
        {
            // Chuyển đường dẫn ảo thành đường dẫn vật lý
            string sourceSavePath = HttpContext.Current.Server.MapPath(sourceDir.ToString());
            string outputSavePath = HttpContext.Current.Server.MapPath(outputDir.ToString());
            // load file to converted         
            var workBook = new Workbook(sourceSavePath);
            // save in different formats           
            workBook.Save(outputSavePath, Aspose.Cells.SaveFormat.Xlsx);
        }

        public List<ThanhVien> ImportThanhVien(string sourceDir)
        {
            List<ThanhVien> list = new List<ThanhVien>();
            string sourceSavePath = HttpContext.Current.Server.MapPath(sourceDir.ToString());
            Workbook wb = new Workbook(sourceSavePath);
            Worksheet ws = wb.Worksheets[0];
            // import data form Excel           
            int firstRow = ws.Cells.FirstCell.Row + 1;
            int firstColumn = ws.Cells.FirstCell.Column;
            int totalRows = ws.Cells.MaxDataRow - ws.Cells.MinDataRow;
            int totalColumns = (ws.Cells.MaxDataColumn + 1) - ws.Cells.MinDataColumn;

            var data = ws.Cells.ExportArray(firstRow, firstColumn, totalRows, totalColumns);

            for (int i = 0; i < totalRows; i++)
            {
                ThanhVien tv = new ThanhVien();
                if ((object)data[i, 0] != null)
                    tv.Ten = (string)data[i, 0].ToString().Trim();
                if ((object)data[i, 1] != null)
                    tv.UserName = (string)data[i, 1].ToString().Trim();
                if ((object)data[i, 2] != null)
                {
                    tv.MaSoThanhVien = (string)data[i, 2].ToString().Trim();
                    tv.RowExcel = firstRow + 1;
                }
                if ((object)data[i, 3] != null)
                    tv.LoaiTK = (string)data[i, 3].ToString().Trim();
                if ((object)data[i, 4] != null)
                    tv.GioiTinh = (string)data[i, 4].ToString().Trim();
                if ((object)data[i, 5] != null)
                {
                    string day = (string)data[i, 5];
                    DateTime ngaySinh = DateTime.ParseExact(day, "dd-MM-yyyy", null);
                    tv.NgaySinh = ngaySinh;
                }
                if ((object)data[i, 6] != null)
                    tv.LopHoc = (string)data[i, 6].ToString().Trim();
                if ((object)data[i, 7] != null)
                    tv.NienKhoa = (string)data[i, 7].ToString().Trim();
                if ((object)data[i, 8] != null)
                    tv.DiaChi = (string)data[i, 8].ToString().Trim();
                if ((object)data[i, 9] != null)
                    tv.SDT = (string)data[i, 9].ToString();
                tv.Password = "1";
                list.Add(tv);
            }
            return list;
        }
        public List<TheLoaiSach> ImportTheLoaiSach(string sourceDir)
        {
            List<TheLoaiSach> list = new List<TheLoaiSach>();
            string sourceSavePath = HttpContext.Current.Server.MapPath(sourceDir.ToString());
            Workbook wb = new Workbook(sourceSavePath);
            Worksheet ws = wb.Worksheets[0];
            // import data form Excel           
            int firstRow = ws.Cells.FirstCell.Row + 1;
            int firstColumn = ws.Cells.FirstCell.Column;
            int totalRows = ws.Cells.MaxDataRow - ws.Cells.MinDataRow;
            int totalColumns = (ws.Cells.MaxDataColumn + 1) - ws.Cells.MinDataColumn;

            var data = ws.Cells.ExportArray(firstRow, firstColumn, totalRows, totalColumns);

            for (int i = 0; i < totalRows; i++)
            {
                TheLoaiSach theLoai = new TheLoaiSach();
                if ((object)data[i, 0] != null)
                    theLoai.MaDDC = (string)data[i, 0].ToString().Trim();
                if ((object)data[i, 1] != null)
                    theLoai.TenTheLoai = (string)data[i, 1].ToString().Trim();
                if ((object)data[i, 2] != null)
                    theLoai.IdParent = (string)data[i, 2].ToString().Trim();
                if ((object)data[i, 3] != null)
                    theLoai.MoTa = (string)data[i, 3].ToString().Trim(); ;
                list.Add(theLoai);
            }
            return list;
        }

        public List<Sach> ImportSach(string sourceDir)
        {
            List<Sach> list = new List<Sach>();
            string sourceSavePath = HttpContext.Current.Server.MapPath(sourceDir.ToString());
            Workbook wb = new Workbook(sourceSavePath);
            Worksheet ws = wb.Worksheets[0];
            // import data form Excel           
            int firstRow = ws.Cells.FirstCell.Row + 1;
            int firstColumn = ws.Cells.FirstCell.Column;
            int totalRows = ws.Cells.MaxDataRow - ws.Cells.MinDataRow;
            int totalColumns = (ws.Cells.MaxDataColumn + 1) - ws.Cells.MinDataColumn;

            var data = ws.Cells.ExportArray(firstRow, firstColumn, totalRows, totalColumns);

            for (int i = 0; i < totalRows; i++)
            {
                Sach sach = new Sach();
                sach.TenSach = (string)data[i, 0].ToString().Trim();
                sach.ISBN = (string)data[i, 1].ToString();
                sach.IdTheLoai = (string)data[i, 2].ToString();

                var input = (string)data[i, 3].ToString();
                string[] tenTacGia = input.Split(new Char[] { ',', '.', '!', '\\', '/', ':', ';', '\n', '_', '-' });
                sach.listTacGia = new List<TacGia>();
                foreach (var ten in tenTacGia)
                {
                    sach.listTacGia.Add(new TacGia() { TenTacGia = ten.Trim() });
                }

                sach.IdNhaXuatBan = (string)data[i, 4].ToString().Trim();
                sach.IdKeSach = (string)data[i, 5].ToString().Trim();
                sach.SoTrang = (string)data[i, 6].ToString().Trim(); 
                sach.IdNgonNgu = (string)data[i, 7].ToString().Trim();
                sach.NamXuatBan = (string)data[i, 8].ToString().Trim();
                sach.GiaBia = (string)data[i, 9].ToString().Trim();
                sach.PhiMuonSach = (string)data[i, 10].ToString().Trim();
                sach.XuatXu = (string)data[i, 11].ToString().Trim();
                sach.NguoiBienDich = (string)data[i, 12].ToString().Trim();
                sach.TaiBan = (string)data[i, 13].ToString().Trim();
                sach.TomTat = (string)data[i, 14].ToString().Trim();

                list.Add(sach);
            }
            return list;
        }
        public void ExportWord(string sourceDir, List<ThanhVien> list)
        {
            string sourceSavePath = HttpContext.Current.Server.MapPath(sourceDir.ToString());
            Document outputDoc = new Document();
            DocumentBuilder outputBuilder = new DocumentBuilder(outputDoc);
            foreach (var item in list)
            {
                Document docx = new Document(sourceSavePath);
                if (item.Ten != null)
                    docx.Range.Replace("_FullName_", item.Ten, true, true);
                if (item.GioiTinh != null)
                    docx.Range.Replace("_GioiTinh_", item.GioiTinh, true, true);
                if (item.NgaySinh != null)
                    docx.Range.Replace("_NgaySinh_", item.NgaySinh.ToString("dd-MM-yyyy"), true, true);
                if (item.LopHoc != null)
                    docx.Range.Replace("_LopHoc_", item.LopHoc, true, true);
                docx.Range.Replace("_NgayTaoThe_", DateTime.Today.ToString("dd-MM-yyyy"), true, true);
                if (item.NienKhoa != null)
                    docx.Range.Replace("_NienKhoa_", item.NienKhoa, true, true);
                if (item.HinhChanDung != null)
                {
                    string linkImage = HttpContext.Current.Server.MapPath(item.HinhChanDung.ToString());
                    docx.Range.Replace(new Regex("_Image_"), new ReplaceWithImageEvaluator(linkImage), false);
                }
                if (item.QRLink != null)
                {
                    string linkImage = HttpContext.Current.Server.MapPath(item.QRLink.ToString());
                    docx.Range.Replace(new Regex("_QR_"), new ReplaceWithImageQR(linkImage), false);
                }
                // int month= DateTime.Today.Month;
                // int year = (DateTime.Today.Year + 1);
                string thoiHan = DateTime.Today.Month.ToString() + "/" + (DateTime.Today.Year + 1).ToString();
                docx.Range.Replace("_ThoiHan_",thoiHan, true, true);
                outputBuilder.MoveToDocumentEnd();
                outputBuilder.InsertDocument(docx, ImportFormatMode.KeepDifferentStyles);
            }
            // outputDoc.Save(Path.GetDirectoryName(sourceSavePath) + @"/DanhSachThanhVien-Export.docx");
            outputDoc.Save(@"D:/DanhSachThanhVien-Export.docx");
        }

        public List<ChiTietNhapSach> ImportPhieuNhapSach(string sourceDir)
        {
            List<ChiTietNhapSach> listCTNS = new List<ChiTietNhapSach>();
            string sourceSavePath = HttpContext.Current.Server.MapPath(sourceDir.ToString());
            Workbook wb = new Workbook(sourceSavePath);
            Worksheet ws = wb.Worksheets[0];
            // import data form Excel           
            int firstRow = ws.Cells.FirstCell.Row + 1;
            int firstColumn = ws.Cells.FirstCell.Column;
            int totalRows = ws.Cells.MaxDataRow - ws.Cells.MinDataRow;
            int totalColumns = (ws.Cells.MaxDataColumn + 1) - ws.Cells.MinDataColumn;

            var data = ws.Cells.ExportArray(firstRow, firstColumn, totalRows, totalColumns);

            for (int i = 0; i < totalRows; i++)
            {
                ChiTietNhapSach ctns = new ChiTietNhapSach();

                if ((object)data[i, 0] != null)
                {
                    ctns.IdSach = (string)data[i, 0].ToString().Trim();
                    ctns.RowExcel= firstRow + 1;
                }                   
                if ((object)data[i, 1] != null)
                    ctns.SoLuong = (int)data[i, 1];
                if ((object)data[i, 2] != null)
                    ctns.IdTinhtrang = (string)data[i, 2].ToString().Trim();
                if ((object)data[i, 3] != null)
                    ctns.GhiChu = (string)data[i, 3].ToString().Trim();

                listCTNS.Add(ctns);
            }
            return listCTNS;
        }
    }
}
