using BiTech.Library.BLL.BarCode_QR;
using BiTech.Library.DAL.CommonConstants;
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
			string tenKhongDau = ConvertToTiengVietKhongDauConstants.RemoveSign4VietnameseString(sach.TenSach);
			string uploadFolder = GetUploadFolder(Helpers.UploadFolder.QRCodeBook);
			string uploadFileNameQR = null;
			if (imageName != null)
			{
				uploadFileNameQR = Path.Combine(physicalWebRootPath, uploadFolder, imageName);
			}
			else
			{
				//   ==> Tên hình QR <==
				
				uploadFileNameQR = Path.Combine(physicalWebRootPath, uploadFolder, sach.MaKiemSoat +
				"-" + sach.TenSach + ".jpg");
			}
			string location = Path.GetDirectoryName(uploadFileNameQR);
			if (!Directory.Exists(location))
			{
				Directory.CreateDirectory(location);
			}
			// chuyển đường dẫn vật lý thành đường dẫn ảo
			var pathQR = uploadFileNameQR.Replace(physicalWebRootPath, "/").Replace(@"\", @"/").Replace(@"//", @"/");
			//   ==> Info QRdata <==
			string info = "BLibBook-" + sach.Id + "-" + sach.MaKiemSoat + "-" + tenKhongDau;
			bool bolQR = barcode.CreateQRCode(info, pathQR);
			if (bolQR == true)
			{
				sach.QRlink = pathQR;
				sach.QRData = info;
			}
			return sach;
		}
		public string GetInfo(string info)
		{
			try
			{
				string[] arrStr = info.Split('-');
				string id = null;
				string MaKiemSoat = info;
				string tenSach = null;
				if (arrStr[0].Equals("BLibBook") == true)
				{
					id = arrStr[1];
					MaKiemSoat = arrStr[2];
					tenSach = arrStr[3];
				}
				return MaKiemSoat;
			}
			catch { return info; }
		}		
	}
}