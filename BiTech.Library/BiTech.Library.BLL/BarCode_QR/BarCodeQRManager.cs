using Aspose.BarCode;
using Aspose.BarCodeRecognition;
using OnBarcode.Barcode.BarcodeScanner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
namespace BiTech.Library.BLL.BarCode_QR
{
    public class BarCodeQRManager : baseAsposeLic
    {
        public BarCodeQRManager() : base()
        {

        }
        private string _barcodePath = "/Content/Images/BarCodes/";
        public string BarCodePath
        {
            get { return _barcodePath; }
        }
        private string codePath = "/Upload/QRCodeUser/";
        /// <summary>
        /// Tạo mã BarCode
        /// </summary>
        /// <param name="barcodeString"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string CreateBarCode(string barcodeString, string fileName)
        {
            //Đường dẫn lưu file ảnh barcode
            string barcodeSavePath = HttpContext.Current.Server.MapPath("~" + this._barcodePath + fileName.ToString() + ".bmp");
            // ExStart:CreateQRbarcode                
            // The path to the documents directory.
            //string dataDir = "./";

            BarCodeBuilder barCodeBarCodeBuilder_ISBN = new BarCodeBuilder(barcodeString, Symbology.ISBN);
            barCodeBarCodeBuilder_ISBN.Save(barcodeSavePath, BarCodeImageFormat.Bmp);

            //BarCodeBuilder barCodeBarCodeBuilder_QR = new BarCodeBuilder("1234567890", Symbology.QR);
            //barCodeBarCodeBuilder_QR.Save(barcodeSavePath, BarCodeImageFormat.Bmp);

            return _barcodePath + fileName + ".bmp";
        }

        /// <summary>codePath
        /// Tạo mã QR
        /// </summary>
        /// <param name="qrCodeString"> qrCodeString: mã vạch</param>
        /// <param name="qrCodeSavePath"> qrCodeSavePath: nơi hình dc lưu</param>
        /// <returns></returns>
        public bool CreateQRCode(string qrCodeString, string qrCodeSavePath)
        {
            try
            {
                float mr = 5;

                //string barcodeSavePath = HttpContext.Current.Server.MapPath("~" + this.codePath + qrCodeSavePath.ToString() + ".bmp");
                string barcodeSavePath = HttpContext.Current.Server.MapPath(qrCodeSavePath.ToString());
                BarCodeBuilder barCodeBuilder_QR = new BarCodeBuilder(qrCodeString, Symbology.QR);

                barCodeBuilder_QR.CodeTextFont = new System.Drawing.Font("Times New Roman", 20);
                barCodeBuilder_QR.CodeTextEncoding = Encoding.UTF8;
                barCodeBuilder_QR.CodeLocation = CodeLocation.None; // Ẩn codetext trên QR
                barCodeBuilder_QR.ImageQuality = ImageQualityMode.Default;

                barCodeBuilder_QR.Margins.Bottom = mr;
                barCodeBuilder_QR.Margins.Right = mr;
                barCodeBuilder_QR.Margins.Top = mr;
                barCodeBuilder_QR.Margins.Left = mr;
                barCodeBuilder_QR.BorderVisible = false;
                barCodeBuilder_QR.BorderWidth = 0;

                barCodeBuilder_QR.GraphicsUnit = System.Drawing.GraphicsUnit.Pixel;
                barCodeBuilder_QR.xDimension = 5f;

                barCodeBuilder_QR.Save(barcodeSavePath, BarCodeImageFormat.Jpeg);
            }
            catch (Exception ex)
            {
                string x = ex.ToString();
                return false;
            }
            return true;
        }

        /// <summary>
        /// Tạo hình chứa mã ISBN và EAN13
        /// </summary>
        /// <param name="strEAN13"></param>
        /// <param name="strISBN"></param>
        /// <param name="pathEAN13"></param>
        /// <param name="pathISBN"></param>
        /// <returns></returns>
        public bool CreateBarCode(string strEAN13, string strISBN, string pathEAN13, string pathISBN)
        {
            try
            {
                //string barcodeSavePath = HttpContext.Current.Server.MapPath("~" + this.codePath + qrCodeSavePath.ToString() + ".bmp");
                string barcodeSavePath = HttpContext.Current.Server.MapPath(pathEAN13.ToString());
                string barcodeSavePath2 = HttpContext.Current.Server.MapPath(pathISBN.ToString());
                // Tạo mã ISBN
                BarCodeBuilder barCodeBuilder_QR = new BarCodeBuilder(strISBN, Symbology.ISBN);
                barCodeBuilder_QR.ImageQuality = ImageQualityMode.Default;              
                string s1 = strISBN.Substring(0, 3);
                string s2 = strISBN.Substring(3, 3);
                string s3 = strISBN.Substring(6, 2);
                string s4 = strISBN.Substring(8, 4);
                string s5 = strISBN.Substring(12,1);
                Caption caption = new Caption();
                caption.Text = "ISBN: " + s1 + "-" + s2 + "-" + s3 + "-" + s4 + "-" + s5;
                barCodeBuilder_QR.CaptionAbove = caption;
                barCodeBuilder_QR.Save(barcodeSavePath2, BarCodeImageFormat.Bmp);
                // Tạo mã EAN13
                BarCodeBuilder barCodeBuilder_QR2 = new BarCodeBuilder(strEAN13, Symbology.EAN13);
                barCodeBuilder_QR2.ImageQuality = ImageQualityMode.Default;               
                barCodeBuilder_QR2.Save(barcodeSavePath, BarCodeImageFormat.Bmp);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 
        /// Đọc mã barcode từ hình
        /// </summary>
        /// <param name="_filePath"></param>
        /// <returns></returns>
        public string ReadBarCode(string _filePath)
        {
            //BarCodeReader reader = new BarCodeReader(_filePath, BarCodeReadType.Code39Extended);
            //BarCodeReader reader = new BarCodeReader(@"d:\q.jpeg");
            //var a = reader;                    
            String[] barcodesAll = BarcodeScanner.Scan(_filePath, BarcodeType.All);
            return barcodesAll[0];
            //return reader.GetCodeText();
        }
        /// <summary>
        /// Đọc mã QR
        /// </summary>
        /// <param name="_filePath"></param>
        /// <returns></returns>
        public string ReadQRCode(string _filePath)
        {
            String[] barcodes = BarcodeScanner.Scan(_filePath, BarcodeType.QRCode);
            return barcodes[0];
        }
    }
}
