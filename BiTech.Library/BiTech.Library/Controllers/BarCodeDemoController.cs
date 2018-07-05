using Aspose.BarCodeRecognition;
using BiTech.Library.BLL.BarCode_QR;
using BiTech.Library.Models;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BiTech.Library.Controllers
{
    public class BarCodeDemoController : BaseController
    {
        // GET: BarCodeDemo
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult BarCodeRead()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult BarCodeRead(HttpPostedFileBase barCodeUpload)
        {
            String localSavePath = "~/Upload/";
            string str = string.Empty;
            string strImage = string.Empty;
            string strBarCode = string.Empty;
            BarCodeQRManager barcode = new BarCodeQRManager();

            if (barCodeUpload != null)
            {
                //Save hình ở thư mục upload
                String fileName = barCodeUpload.FileName;
                localSavePath += fileName;
                barCodeUpload.SaveAs(Server.MapPath(localSavePath));

                Bitmap bitmap = null;
                try
                {
                    //load lên bitmap
                    bitmap = new Bitmap(barCodeUpload.InputStream);
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }

                if (bitmap == null)
                {

                    str = "Your file is not an image";

                }
                else
                {
                    //img từ local
                    strImage = "http://localhost:" + Request.Url.Port + "/Upload/" + fileName;

                    strBarCode = barcode.ReadBarCode(Server.MapPath(localSavePath));

                }
            }
            else
            {
                str = "Please upload the bar code Image.";
            }
            ViewBag.ErrorMessage = str;
            ViewBag.BarCode = strBarCode;
            ViewBag.BarImage = strImage;
            return View();
        }
        /// <summary>
        /// Đọc barcode
        /// </summary>
        /// <param name="_Filepath"></param>
        /// <returns></returns>
        private String ReadBarcodeFromFile(string _Filepath)
        {
            //BarCodeManager barcode = new BarCodeManager();
            BarCodeReader reader = new BarCodeReader(_Filepath, BarCodeReadType.ISBN);
            var a = reader.GetCodeText();
            return a;

            //String[] barcodes = BarcodeScanner.Scan(_Filepath, BarcodeType.Code39);
            //if (barcodes != null)
            //{
            //    return barcodes[0];
            //}
            //return "";
        }
    }
}