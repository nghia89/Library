﻿using BiTech.Library.BLL.BarCode_QR;
using BiTech.Library.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using static BiTech.Library.Helpers.Tool;
namespace BiTech.Library.Controllers.BaseClass
{
    public class ThanhVienCommon
    {
        /// <summary>
        /// Tạo niêm khóa cho DropDownList Thành Viên
        /// </summary>
        /// <returns></returns>
        public List<string> TaoNienKhoa()
        {
            int yearStart = 2013;
            int yearEnd = DateTime.Today.Year + 1;
            List<string> listNienKhoa = new List<string>();
            int i = yearStart;
            int j = yearStart + 1;
            do
            {
                listNienKhoa.Add(i + " - " + j);
                i++; j++;
            } while (j != (yearEnd + 1));
            return listNienKhoa;
        }
        /// <summary>
        /// Lưu hình chân dung của Thành Viên khi thêm mới
        /// </summary>
        /// <param name="hinhChanDung"></param>
        /// <param name="physicalWebRootPath"></param>
        /// <param name="idThanhVien"></param>
        /// <param name="maSoThanhVien"></param>
        /// <returns></returns>
        public ThanhVien LuuHinhChanDung(string physicalWebRootPath, ThanhVien thanhVien,
              string ImageName, HttpPostedFileBase hinhChanDung)
        {
            //  string physicalWebRootPath = Server.MapPath("/");
            string uploadFolder = GetUploadFolder(Helpers.UploadFolder.AvatarUser);
            string uploadFileName = null;
            if (ImageName != null)
                uploadFileName = Path.Combine(physicalWebRootPath, uploadFolder, ImageName);
            else
                uploadFileName = Path.Combine(physicalWebRootPath, uploadFolder, thanhVien.MaSoThanhVien + "-" + hinhChanDung.FileName);
            string location = Path.GetDirectoryName(uploadFileName);
            if (!Directory.Exists(location))
            {
                Directory.CreateDirectory(location);
            }
            using (FileStream fileStream = new FileStream(uploadFileName, FileMode.Create))
            {
                hinhChanDung.InputStream.CopyTo(fileStream);
                thanhVien.HinhChanDung = uploadFileName.Replace(physicalWebRootPath, "/").Replace(@"\", @"/").
                    Replace(@"//", @"/");
                return thanhVien;
            }
        }
        public ThanhVien LuuMaVach(string physicalWebRootPath, ThanhVien thanhVien, string imageName)
        {
            BarCodeQRManager barcode = new BarCodeQRManager();
            string uploadFolder = GetUploadFolder(Helpers.UploadFolder.QRCodeUser);
            string uploadFileNameQR = null;
            if (imageName != null)
            {
                uploadFileNameQR = Path.Combine(physicalWebRootPath, uploadFolder, imageName);
            }
            else
            {
                //   ==> Tên hình QR <==
                uploadFileNameQR = Path.Combine(physicalWebRootPath, uploadFolder, thanhVien.MaSoThanhVien +
                "-" + thanhVien.Ten + ".bmp");
            }
            string location = Path.GetDirectoryName(uploadFileNameQR);
            if (!Directory.Exists(location))
            {
                Directory.CreateDirectory(location);
            }
            // chuyển đường dẫn vật lý thành đường dẫn ảo
            var pathQR = uploadFileNameQR.Replace(physicalWebRootPath, "/").Replace(@"\", @"/").Replace(@"//", @"/");
            //   ==> Info QRdata <==
            string info = "BLibUser-" + thanhVien.Id + "-" + thanhVien.MaSoThanhVien + "-" + thanhVien.Ten;
            bool bolQR = barcode.CreateQRCode(info, pathQR);
            if (bolQR == true)
            {
                thanhVien.QRLink = pathQR;
                thanhVien.QRData = info;
            }
            return thanhVien;
        }
        public string GetInfo(string info)
        {
            string[] arrStr = info.Split('-');
            string id = null;
            string maSo = null;
            string ten = null;
            if (arrStr[0].Equals("BLibUser") == true)
            {
                id = arrStr[1];
                maSo = arrStr[2];
                ten = arrStr[3];
            }
            return maSo;
        }
        public List<ThanhVien> ImportFromExcel(string physicalWebRootPath, HttpPostedFileBase linkExcel)
        {
            ExcelManager excelManager = new ExcelManager();
            List<ThanhVien> list = new List<ThanhVien>();
            string uploadForder = GetUploadFolder(Helpers.UploadFolder.FileExcel);
            var sourceFileName = Path.Combine(physicalWebRootPath, uploadForder, linkExcel.FileName);
            string location = Path.GetDirectoryName(sourceFileName);
            if (!Directory.Exists(location))
            {
                Directory.CreateDirectory(location);
            }
            using (FileStream fileStream = new FileStream(sourceFileName, FileMode.Create))
            {
                linkExcel.InputStream.CopyTo(fileStream);
                var sourceDir = fileStream.Name.Replace(physicalWebRootPath, "/").Replace(@"\", @"/").Replace(@"//", @"/");
                list = excelManager.ImportThanhVien(sourceDir);
            }
            return list;
        }
    }
}