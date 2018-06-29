using Aspose.BarCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BiTech.Library.Models
{
    public class FileManager : baseBarCodeLic
    {
        public FileManager() : base()
        {

        }
        private string _barcodePath = "/Content/Images/BarCodes/";
        public string BarCodePath
        {
            get { return _barcodePath; }
        }
        public string CreateBarCode(string barcodeString, string albumId)
        {
            //Đường dẫn lưu file ảnh barcode
            string barcodeSavePath = HttpContext.Current.Server.MapPath("~" + this._barcodePath + albumId.ToString() + ".bmp");
            // ExStart:CreateQRbarcode                
            // The path to the documents directory.
            //string dataDir = "./";

            BarCodeBuilder barCodeBarCodeBuilder_Code128 = new BarCodeBuilder(barcodeString, Symbology.Code128);
            barCodeBarCodeBuilder_Code128.Save(barcodeSavePath, BarCodeImageFormat.Bmp);

            //BarCodeBuilder barCodeBarCodeBuilder_QR = new BarCodeBuilder("1234567890", Symbology.QR);
            //barCodeBarCodeBuilder_QR.Save(barcodeSavePath, BarCodeImageFormat.Bmp);

            return _barcodePath + albumId + ".bmp";
        }
    }
}