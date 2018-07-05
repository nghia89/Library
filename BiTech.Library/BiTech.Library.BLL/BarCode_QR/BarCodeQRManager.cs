
using Aspose.BarCode;
using OnBarcode.Barcode.BarcodeScanner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BiTech.Library.BLL.BarCode_QR
{
    public class BarCodeQRManager : baseBarCodeLic
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
        /// <param name="albumId"></param>
        /// <returns></returns>
        public string CreateBarCode(string barcodeString, string albumId)
        {
            //Đường dẫn lưu file ảnh barcode
            string barcodeSavePath = HttpContext.Current.Server.MapPath("~" + this._barcodePath + albumId.ToString() + ".bmp");
            // ExStart:CreateQRbarcode                
            // The path to the documents directory.
            //string dataDir = "./";

            BarCodeBuilder barCodeBarCodeBuilder_ISBN = new BarCodeBuilder(barcodeString, Symbology.Code128);
            barCodeBarCodeBuilder_ISBN.Save(barcodeSavePath, BarCodeImageFormat.Bmp);

            //BarCodeBuilder barCodeBarCodeBuilder_QR = new BarCodeBuilder("1234567890", Symbology.QR);
            //barCodeBarCodeBuilder_QR.Save(barcodeSavePath, BarCodeImageFormat.Bmp);

            return _barcodePath + albumId + ".bmp";
        }
        /// <summary>codePath
        /// Tạo mã QR
        /// </summary>
        /// <param name="qrCode"> qrCodeString: mã vạch</param>
        /// <param name="qrCode"> qrCodeSavePath: nơi hình dc lưu</param>
        /// <returns></returns>
        public bool CreateQRCode(string qrCodeString, string qrCodeSavePath)
        {
            try
            {
                //string barcodeSavePath = HttpContext.Current.Server.MapPath("~" + this.codePath + qrCodeSavePath.ToString() + ".bmp");
                string barcodeSavePath = HttpContext.Current.Server.MapPath(qrCodeSavePath.ToString());
                BarCodeBuilder barCodeBuilder_QR = new BarCodeBuilder(qrCodeString, Symbology.QR);
                barCodeBuilder_QR.ImageQuality = ImageQualityMode.Default;
                barCodeBuilder_QR.Save(barcodeSavePath, BarCodeImageFormat.Bmp);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// Đọc mã barcode từ hình
        /// </summary>
        /// <param name="_filePath"></param>
        /// <returns></returns>
        public string ReadBarCode(string _filePath)
        {
            //BarCodeReader reader = new BarCodeReader(_filePath, BarCodeReadType.Code39Extended);
            //BarCodeReader reader = new BarCodeReader(@"d:\q.jpeg");
            //var a = reader;

            String[] barcodes = BarcodeScanner.Scan(_filePath, BarcodeType.Code128);
            return barcodes[0];
            //return reader.GetCodeText();
        }
    }
}
