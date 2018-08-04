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

        public List<ThanhVien> ImportExcel(string sourceDir)
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
                tv.Ten = (string)data[i, 0];
                tv.GioiTinh = (string)data[i, 1];              
                if ((object)data[i, 2] != null)
                    tv.NgaySinh = (DateTime)data[i, 2];
                tv.LopHoc = (string)data[i, 3];
                tv.DiaChi = (string)data[i, 4];             
                if ((object)data[i, 5] != null)
                    tv.SDT = (string)data[i, 5].ToString();
                list.Add(tv);
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
                docx.Range.Replace("_FullName_", item.Ten, true, true);
                docx.Range.Replace("_GioiTinh_", item.GioiTinh, true, true);
                if (item.LopHoc != null)
                    docx.Range.Replace("_LopHoc_", item.LopHoc, true, true);
                docx.Range.Replace("_NgayTaoThe_", DateTime.Today.ToString("dd-MM-yyyy"), true, true);
                if (item.HinhChanDung != null)
                {
                    string linkImage = HttpContext.Current.Server.MapPath(item.HinhChanDung.ToString());
                    docx.Range.Replace(new Regex("_image_"), new ReplaceWithImageEvaluator(linkImage), false);
                }
                outputBuilder.MoveToDocumentEnd();
                outputBuilder.InsertDocument(docx, ImportFormatMode.KeepDifferentStyles);
            }
            // outputDoc.Save(Path.GetDirectoryName(sourceSavePath) + @"/outt.docx");
            outputDoc.Save(@"D:/DanhSachThanhVien-Export.docx");
        }
    }
}
