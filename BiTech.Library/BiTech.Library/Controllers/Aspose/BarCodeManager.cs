using Aspose.BarCode;
using Microsoft.Web.Mvc.Controls;
using OnBarcode.Barcode.BarcodeScanner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BiTech.Library.Models
{
    public class BarCodeManager : baseBarCodeLic
    {
        public BarCodeManager() : base()
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
            string barcodeSavePath = HttpContext.Current.Server.MapPath("~" + this._barcodePath + albumId.ToString() + ".jpeg");
            // ExStart:CreateQRbarcode                
            // The path to the documents directory.
            //string dataDir = "./";

            BarCodeBuilder barCodeBarCodeBuilder_Code128 = new BarCodeBuilder(barcodeString, Symbology.ISBN);            
            barCodeBarCodeBuilder_Code128.Save(barcodeSavePath, BarCodeImageFormat.Jpeg);

            //BarCodeBuilder barCodeBarCodeBuilder_QR = new BarCodeBuilder("1234567890", Symbology.QR);
            //barCodeBarCodeBuilder_QR.Save(barcodeSavePath, BarCodeImageFormat.Bmp);

            return _barcodePath + albumId + ".jpeg";
        }

        public string ReadBarCode(string _filePath)
        {
            //BarCodeReader reader = new BarCodeReader(_filePath, BarCodeReadType.Code39Extended);
            //BarCodeReader reader = new BarCodeReader(@"d:\q.jpeg");
            //var a = reader;

            String[] barcodes = BarcodeScanner.Scan(_filePath, BarcodeType.ISBN);
            return barcodes[0];
            //return reader.GetCodeText();
        }
    }
}