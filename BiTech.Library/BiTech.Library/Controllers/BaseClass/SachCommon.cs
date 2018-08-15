using BiTech.Library.BLL.BarCode_QR;
using BiTech.Library.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using static BiTech.Library.Helpers.Tool;
namespace BiTech.Library.Controllers.BaseClass
{
    public class SachCommon
    {
        public Sach LuuMaVachSach(string physicalWebRootPath, Sach sach, string imageName)
        {
            BarCodeQRManager barcode = new BarCodeQRManager();
            string uploadFolder = GetUploadFolder(Helpers.UploadFolder.QRCodeBook);
            string uploadFileNameQR = null;
            if (imageName != null)
            {
                uploadFileNameQR = Path.Combine(physicalWebRootPath, uploadFolder, imageName);
            }
            else
            {
                //   ==> Tên hình QR <==
                uploadFileNameQR = Path.Combine(physicalWebRootPath, uploadFolder,sach.MaKiemSoat  +
                "-" + sach.TenSach + ".bmp");
            }
            string location = Path.GetDirectoryName(uploadFileNameQR);
            if (!Directory.Exists(location))
            {
                Directory.CreateDirectory(location);
            }
            // chuyển đường dẫn vật lý thành đường dẫn ảo
            var pathQR = uploadFileNameQR.Replace(physicalWebRootPath, "/").Replace(@"\", @"/").Replace(@"//", @"/");
            //   ==> Info QRdata <==
            string info = sach.Id + "-" + sach.MaKiemSoat + "-" + sach.TenSach;
            bool bolQR = barcode.CreateQRCode(info, pathQR);
            if (bolQR == true)
            {
                sach.QRlink = pathQR;
                sach.QRData = info;
            }
            return sach;
        }
    }
}