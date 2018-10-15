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
    }
}
