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
using System.Net.Http;

namespace BiTech.Library.BLL.BarCode_QR
{
    public class ExcelManager : baseAsposeLic
    {
        public ExcelManager() : base()
        {

        }
               
        public Document ExportWord(string sourceDir, List<ThanhVien> list, string fileName, List<string> lstHeader)
        {
            string sourceSavePath = HttpContext.Current.Server.MapPath(sourceDir.ToString());
            Document outputDoc = new Document();
            DocumentBuilder outputBuilder = new DocumentBuilder(outputDoc);
            foreach (var item in list)
            {
                Document docx = new Document(sourceSavePath);
                if (lstHeader[0] != null)
                    docx.Range.Replace("_TenDonVi_", lstHeader[0].ToUpper(), true, true);
                else
                    docx.Range.Replace("_TenDonVi_", "", true, true);

                if (lstHeader[1] != null)
                    docx.Range.Replace("_TenTruong_", lstHeader[1].ToUpper(), true, true);
                else
                    docx.Range.Replace("_TenTruong_", "", true, true);

                //docx.Range.Replace("_TenDonVi_", "SỞ GIÁO DỤC VÀ ĐÀO TẠO TPHCM", true, true);
                //docx.Range.Replace("_TenTruong_", "TRƯỜNG THPT VÕ THỊ SÁU", true, true);

                if (item.Ten != null)
                    docx.Range.Replace("_FullName_", item.Ten.ToUpper(), true, true);
                else
                    docx.Range.Replace("_FullName_", "", true, true);

                if (item.GioiTinh != null)
                    docx.Range.Replace("_GioiTinh_", item.GioiTinh, true, true);
                else
                    docx.Range.Replace("_GioiTinh_", "", true, true);

                // if (item.NgaySinh != null && item.NgaySinh.ToString("dd-MM-yyyy") != "01-01-0001")
                if (item.NgaySinh != null)
                    docx.Range.Replace("_NgaySinh_", item.NgaySinh.ToString("dd/MM/yyyy"), true, true);
                else
                    docx.Range.Replace("_NgaySinh_", "", true, true);

                if (item.MaSoThanhVien != null)
                    docx.Range.Replace("_MaSoHS_", item.MaSoThanhVien, true, true);
                else
                    docx.Range.Replace("_MaSoHS_", "", true, true);

                if (item.LopHoc != null)
                    docx.Range.Replace("_LopHoc_", item.LopHoc, true, true);
                else
                    docx.Range.Replace("_LopHoc_", "", true, true);

                if (item.ChucVu != null)
                    docx.Range.Replace("_To_", item.ChucVu, true, true);
                else
                    docx.Range.Replace("_To_", "", true, true);

                docx.Range.Replace("_NgayTaoThe_", DateTime.Today.ToString("dd-MM-yyyy"), true, true);
                if (item.NienKhoa != null)
                    docx.Range.Replace("_NienKhoa_", item.NienKhoa, true, true);
                else
                    docx.Range.Replace("_NienKhoa_", "", true, true);

                List<string> lstMS_Import = new List<string>();
                string strMS = item.MaSoThanhVien;
                for (int i = (strMS.Length / 4) + 1; i > 0; i--) //Chia mã số 1451010245 = 1 451 010 245  
                {
                    if (strMS.Length >= 4)
                    {
                        lstMS_Import.Add(strMS.Substring(strMS.Length - 4, 4));
                        strMS = strMS.Substring(0, strMS.Length - 4);
                    }
                    else
                        lstMS_Import.Add(strMS);
                }
                string MS = "";
                for (int i = lstMS_Import.Count - 1; i >= 0; i--)
                {
                    MS += lstMS_Import[i] + "  ";
                }

                if (item.MaSoThanhVien != null)
                    docx.Range.Replace("_MS_", MS, true, true);
                else
                    docx.Range.Replace("_MS_", "", true, true);

                if (item.HinhChanDung != null)
                {
                    string linkImage = HttpContext.Current.Server.MapPath(item.HinhChanDung.ToString());
                    if (File.Exists(linkImage))
                        docx.Range.Replace(new Regex("_Image_"), new ReplaceWithImageEvaluator(linkImage), false);
                    else
                        docx.Range.Replace("_Image_", "", true, true);
                }
                else
                    docx.Range.Replace("_Image_", "", true, true);

                if (item.QRLink != null)
                {
                    //mẫu 1 hình nhỏ hơn mẫu 2
                    string linkImage = HttpContext.Current.Server.MapPath(item.QRLink.ToString());

                    if (File.Exists(linkImage))
                    {
                        if (sourceDir == "/Content/MauWord/Mau2-GV.docx" || sourceDir == "/Content/MauWord/Mau2-HS.docx")
                            docx.Range.Replace(new Regex("_QR_"), new ReplaceWithImageQR_Large(linkImage), false);
                        else
                            docx.Range.Replace(new Regex("_QR_"), new ReplaceWithImageQR(linkImage), false);
                    }
                    else
                        docx.Range.Replace("_QR_", "", true, true);
                }
                string thoiHan = DateTime.Today.Month.ToString() + "/" + (DateTime.Today.Year + 1).ToString();
                docx.Range.Replace("_ThoiHan_", thoiHan, true, true);
                outputBuilder.MoveToDocumentEnd();
                outputBuilder.InsertDocument(docx, ImportFormatMode.KeepDifferentStyles);
            }           
            return outputDoc;
        }
     
        #region Nghia

        // xuat excelSoLuongSach

        #endregion

        #region Vinh ExportWord_QR

        public void ExportQRToWord(string srcDir, List<SachCaBiet> lstBook, string filePath)
        {
            string sourceSavePath = HttpContext.Current.Server.MapPath(srcDir.ToString());
            Document outputDoc = new Document();
            DocumentBuilder outputBuilder = new DocumentBuilder(outputDoc);


            for (int i = 0; i < lstBook.Count; i += 2)
            {
                Document docx = new Document(sourceSavePath);
                //1
                if (lstBook[i].TenSach != null)
                    docx.Range.Replace("_TenSach1_", lstBook[i].TenSach, true, true);
                if (lstBook[i].MaKSCB != null)
                    docx.Range.Replace("_MaCaBiet1_", lstBook[i].MaKSCB, true, true);

                if (lstBook[i].QRlink != null)
                {
                    string linkImage = HttpContext.Current.Server.MapPath(lstBook[i].QRlink.ToString());
                    docx.Range.Replace(new Regex("_ImgQR1_"), new ReplaceWithImageQRBook_Export(linkImage), false);
                }

                //2
                if ((i + 1) < lstBook.Count)
                {
                    if (lstBook[i + 1].TenSach != null)
                        docx.Range.Replace("_TenSach2_", lstBook[i + 1].TenSach, true, true);
                    if (lstBook[i + 1].MaKSCB != null)
                        docx.Range.Replace("_MaCaBiet2_", lstBook[i + 1].MaKSCB, true, true);

                    if (lstBook[i + 1].QRlink != null)
                    {
                        string linkImage = HttpContext.Current.Server.MapPath(lstBook[i + 1].QRlink.ToString());
                        docx.Range.Replace(new Regex("_ImgQR2_"), new ReplaceWithImageQRBook_Export(linkImage), false);
                    }
                }
                else
                {
                    docx.Range.Replace("_TenSach2_", "", true, true);
                    docx.Range.Replace("_MaCaBiet2_", "", true, true);
                    docx.Range.Replace("_ImgQR2_", "", true, true);
                }
                outputBuilder.MoveToDocumentEnd();
                outputBuilder.InsertDocument(docx, ImportFormatMode.KeepDifferentStyles);
            }
            outputDoc.Save(filePath);
        }

        #endregion
    }
}
