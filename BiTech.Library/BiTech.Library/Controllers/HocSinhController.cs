using BiTech.Library.BLL.BarCode_QR;
using BiTech.Library.BLL.DBLogic;
using BiTech.Library.DTO;
using BiTech.Library.Helpers;
using BiTech.Library.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using BiTech.Library.Controllers.BaseClass;
using static BiTech.Library.Helpers.Tool;

namespace BiTech.Library.Controllers
{
    //[Authorize]
    public class HocSinhController : BaseController
    {
        ThanhVienCommon thanhVienCommon;

        public HocSinhController()
        {
            thanhVienCommon = new ThanhVienCommon();
        }

        // GET: User
        public ActionResult Index()
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            var _ThanhVienLogic = new ThanhVienLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            #endregion

            UserViewModel model = new UserViewModel();
            List<ThanhVien> listThanhVien = _ThanhVienLogic.GetAllHS();
            int i = 0;
            model.ListAll = new string[listThanhVien.Count * 2];
            foreach (var item in listThanhVien)
            {
                model.ListAll[i] = item.Ten;
                model.ListAll[i + 1] = item.MaSoThanhVien;
                i += 2;
            }
            model.ListThanhVien = listThanhVien;
            ViewBag.ThongBao = false;
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(UserViewModel model)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            var _ThanhVienLogic = new ThanhVienLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            #endregion
            List<ThanhVien> listAll = _ThanhVienLogic.GetAllHS();
            ViewBag.ThongBao = false;
            model.ListThanhVien = new List<ThanhVien>();
            int i = 0;
            model.ListAll = new string[listAll.Count * 2];
            foreach (var item in listAll)
            {
                model.ListAll[i] = item.Ten;
                model.ListAll[i + 1] = item.MaSoThanhVien;
                i += 2;
            }
            // Todo
            string strSearch = model.TextForSearch;
            var listByName = _ThanhVienLogic.GetByName(strSearch);
            var listByMSTV = _ThanhVienLogic.GetByMaSoThanhVien(strSearch);
            if (listByName.Count > 0)
                model.ListThanhVien = listByName;
            else if (listByMSTV != null)
                model.ListThanhVien = new List<ThanhVien>() { listByMSTV };
            else
            {
                model.ListThanhVien = listAll;
                ViewBag.SearchFail = "Chưa tìm được kết quả phù hợp!";
                ViewBag.ThongBao = true;
            }
            return View(model);
        }

        public PartialViewResult _PartialUser(int? page, string IdUser, List<ThanhVien> list)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return null;
            #endregion

            int pageSize = 6;
            int pageNumber = (page ?? 1);
            ViewBag.pageSize = pageSize;
            ViewBag.pages = pageNumber;
            return PartialView(list.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult _CreateUser()
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion           
            UserViewModel model = new UserViewModel();
            model.TemptNgaySinh = "--/--/----";
            return View(model);
        }

        [HttpPost]
        public ActionResult _CreateUser(UserViewModel viewModel)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            var _ThanhVienLogic = new ThanhVienLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            #endregion
            BarCodeQRManager barcode = new BarCodeQRManager();
            ThanhVien thanhVien = new ThanhVien()
            {
                UserName = viewModel.UserName,
                Password = viewModel.Password,
                Ten = viewModel.Ten,
                MaSoThanhVien = viewModel.MaSoThanhVien,
                NgaySinh = viewModel.NgaySinh,
                GioiTinh = viewModel.GioiTinh,
                LopHoc = viewModel.LopHoc,
                DiaChi = viewModel.DiaChi,
                SDT = viewModel.SDT,
                NienKhoa = viewModel.NienKhoa,
                //IdChucVu = viewModel.IdChucVu,
                TrangThai = EUser.Active, // mac dinh la Active
                CreateDateTime = DateTime.Now,
                // Loại tài khoản  
                LoaiTK = "hs"
            };
            DateTime ngaySinh = new DateTime();
            if (viewModel.TemptNgaySinh.Equals("--/--/----") == false)
            {
                ngaySinh = DateTime.ParseExact(viewModel.TemptNgaySinh, "dd/MM/yyyy", null);
                thanhVien.NgaySinh = ngaySinh;
            }
            else
            {
                ViewBag.NullNgaySinh = "Bạn chưa chọn ngày sinh!";
                return View(viewModel);
            }
            // Kiem tra trung ma thanh vien
            var idMaThanhVien = _ThanhVienLogic.GetByMaSoThanhVien(viewModel.MaSoThanhVien);
            if (idMaThanhVien == null)
            {
                string id = _ThanhVienLogic.Insert(thanhVien);
                ThanhVien tv = _ThanhVienLogic.GetById(id);
                string physicalWebRootPath = Server.MapPath("/");
                ThanhVien temp = new ThanhVien();
                try
                {
                    // Lưu hình chân dung     
                    if (viewModel.HinhChanDung != null)
                    {
                        temp = thanhVienCommon.LuuHinhChanDung(physicalWebRootPath, tv, null, viewModel.HinhChanDung);
                        tv.HinhChanDung = temp.HinhChanDung;
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.ImageUnsuccess = "Lưu hình chân dung thất bại" + ex.Message;
                    viewModel.ListNienKhoa = thanhVienCommon.TaoNienKhoa();
                    return View(viewModel);
                }
                try
                {
                    // Lưu mã vạch
                    temp = thanhVienCommon.LuuMaVach(physicalWebRootPath, tv, null);
                    if (temp != null)
                    {
                        tv.QRLink = temp.QRLink;
                        tv.QRData = temp.QRData;
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.QRUnsuccess = "Tạo mã QR thất bại" + ex.Message;
                    viewModel.ListNienKhoa = thanhVienCommon.TaoNienKhoa();
                    return View(viewModel);
                }
                if (id == null || id == "")
                {
                    //Fail
                    ViewBag.UnSuccess = "Thêm mới thất bại";
                    viewModel.ListNienKhoa = thanhVienCommon.TaoNienKhoa();
                    return View(viewModel);
                }
                else
                {
                    _ThanhVienLogic.Update(tv);
                }
            }
            else
            {
                ViewBag.Duplicate = "Mã số thành viên bị trùng";
                viewModel.ListNienKhoa = thanhVienCommon.TaoNienKhoa();
                return View(viewModel);
            }
            return RedirectToAction("Index", "HocSinh");
        }
        //Get
        public ActionResult _Edit(string id)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion
            var _ThanhVienLogic = new ThanhVienLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

            ViewBag.Success = TempData["Success"];
            ViewBag.UnSucces = TempData["UnSuccess"];
            ThanhVien tv = _ThanhVienLogic.GetById(id);
            if (tv == null)
                return RedirectToAction("NotFound", "Error");
            // tv.NgaySinh.ToString("dd-mm-yyyy");

            EditUserViewModel model = new EditUserViewModel()
            {
                // thông tin cho phép cập nhật
                Ten = tv.Ten,
                DiaChi = tv.DiaChi,
                SDT = tv.SDT,
                GioiTinh = tv.GioiTinh,
                NgaySinh = tv.NgaySinh,
                LopHoc = tv.LopHoc,
                NienKhoa = tv.NienKhoa,
                LinkAvatar = tv.HinhChanDung,
                Id = tv.Id
                // thông tin không được thay đổi 

                /* UserName = tv.UserName,
                Password = tv.Password,
                MaSoThanhVien = tv.MaSoThanhVien,
                ChucVu = tv.ChucVu,
                TrangThai = tv.TrangThai, */
            };
            ViewBag.HinhChanDung = tv.HinhChanDung;
            return View(model);
        }

        [HttpPost]
        public ActionResult _Edit(EditUserViewModel viewModel)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            var _ThanhVienLogic = new ThanhVienLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            #endregion          

            var thanhVien = _ThanhVienLogic.GetById(viewModel.Id);
            // thông tin cho phép cập nhật            
            thanhVien.LopHoc = viewModel.LopHoc;
            thanhVien.NienKhoa = viewModel.NienKhoa;
            thanhVien.Ten = viewModel.Ten;
            thanhVien.DiaChi = viewModel.DiaChi;
            thanhVien.GioiTinh = viewModel.GioiTinh;
            thanhVien.NgaySinh = viewModel.NgaySinh;
            thanhVien.SDT = viewModel.SDT;        
            
            //viewModel.LinkAvatar = "";
            //if (viewModel.HinhChanDung != null && Tool.IsImage(viewModel.HinhChanDung))
            //{
            //    viewModel.LinkAvatar = viewModel.HinhChanDung.FileName;
            //}
            if (viewModel.HinhChanDung != null)
            {
                try
                {
                    // Cập nhật Hình Chân Dung
                    string physicalWebRootPath = Server.MapPath("/");
                    string imageName = null;
                    if (thanhVien.HinhChanDung != null)
                        imageName = thanhVien.HinhChanDung.Replace(@"/Upload/AvatarUser/", @"").Replace(@"/", @"\").Replace(@"/", @"//");
                    ThanhVien tempt = thanhVienCommon.LuuHinhChanDung(physicalWebRootPath, thanhVien, imageName, viewModel.HinhChanDung);
                    if (tempt != null)
                    {
                        thanhVien.HinhChanDung = tempt.HinhChanDung;
                    }
                }
                catch { }
            }

            try
            {
                // cập nhật QR
                string physicalWebRootPath = Server.MapPath("/");
                //string uploadFolder = GetUploadFolder(Helpers.UploadFolder.QRCodeUser);
                string imageName = null;
                if (thanhVien.QRLink != null)
                    imageName = thanhVien.QRLink.Replace(@"/Upload/QRCodeUser/", @"").Replace(@"/", @"\").Replace(@"/", @"//");
                ThanhVien temp = thanhVienCommon.LuuMaVach(physicalWebRootPath, thanhVien, imageName);
                if (temp != null)
                {
                    thanhVien.QRLink = temp.QRLink;
                    thanhVien.QRData = temp.QRData;
                }
            }
            catch { }

            bool resultInfo = _ThanhVienLogic.Update(thanhVien);

            if (resultInfo == true)
            {
                ViewBag.UpdateSuccess = "Cập nhật thành công!";
            }
            else
            {
                ViewBag.UpdateFail = "Cập nhật không thành công!";
            }
            return View(viewModel);
        }

        public ActionResult Delete(string id)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");

            var _ThanhVienLogic = new ThanhVienLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            #endregion
            ThanhVien thanhVien = _ThanhVienLogic.GetById(id);
            if (thanhVien == null)
            {
                return RedirectToAction("NotFound", "Error");
            }
            if (thanhVien.TrangThai == EUser.Active)
            {
                // model.TenTrangThai = "Đang kích Hoạt";
                thanhVien.TrangThai = EUser.DeActive;

            }
            else if (thanhVien.TrangThai == EUser.DeActive)
            {
                // model.TenTrangThai = "Đã bị khóa";
                thanhVien.TrangThai = EUser.Active;
            }
            bool result = _ThanhVienLogic.Update(thanhVien);
            if (result == true)
            {
                TempData["Success"] = "Xóa thành công";
                return RedirectToAction("Index", "HocSinh");
            }
            else
                TempData["Success"] = "Xóa thất bại";
            return View();
        }

        public ActionResult Details(string idUser)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            var _ThanhVienLogic = new ThanhVienLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            ThanhVien thanhVien = _ThanhVienLogic.GetById(idUser);
            if (thanhVien == null)
                return RedirectToAction("NotFound", "Error");

            UserViewModel model = new UserViewModel()
            {
                // thông tin cho phép cập nhật
                Id = thanhVien.Id,
                Ten = thanhVien.Ten,
                DiaChi = thanhVien.DiaChi,
                SDT = thanhVien.SDT,
                GioiTinh = thanhVien.GioiTinh,
                NgaySinh = thanhVien.NgaySinh,
                LopHoc = thanhVien.LopHoc,
                NienKhoa = thanhVien.NienKhoa,

                // thông tin không được thay đổi 
                UserName = thanhVien.UserName,
                Password = thanhVien.Password,
                MaSoThanhVien = thanhVien.MaSoThanhVien,
                TrangThai = thanhVien.TrangThai,
                QRLink = thanhVien.QRLink,
                LinkAvatar = thanhVien.HinhChanDung
            };
            return View(model);
        }

        public ActionResult ChangePassword(string idUser)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion
            var _ThanhVienLogic = new ThanhVienLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            ThanhVien thanhVien = _ThanhVienLogic.GetById(idUser);
            if (thanhVien == null)
            {
                return RedirectToAction("NotFound", "Error");
            }
            ChangePasswordViewModel model = new ChangePasswordViewModel();
            model.Id = idUser;
            ViewBag.Error = TempData["Error"];
            ViewBag.Sussces = TempData["Sussces"];
            return View(model);
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordViewModel model)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            var _ThanhVienLogic = new ThanhVienLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            #endregion
            ThanhVien thanhVien = _ThanhVienLogic.GetById(model.Id);
            if (thanhVien.Password != null)
            {
                if (thanhVien.Password.Equals(model.OldPassword) == true)
                {
                    if (model.NewPassword.Equals(model.ConfirmPassword) == true)
                    {
                        TempData["Sussces"] = "Đổi mật khẩu thành công!";
                        return RedirectToAction("ChangePassword", "HocSinh", new { @idUser = model.Id });
                    }
                    TempData["Error"] = "Nhập lại mật khẩu không khớp nhau!";
                    return RedirectToAction("ChangePassword", "HocSinh", new { @idUser = model.Id });
                }
                TempData["Error"] = "Mật khẩu hiện tại không đúng!";
                return RedirectToAction("ChangePassword", "HocSinh", new { @idUser = model.Id });
            }
            return View(model);
        }

        public ActionResult ImportFromExcel()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ImportFromExcel(UserViewModel model)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            var _ThanhVienLogic = new ThanhVienLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            #endregion

            ExcelManager excelManager = new ExcelManager();
            List<ThanhVien> list = new List<ThanhVien>();
            if (model.LinkExcel != null)
            {
                string physicalWebRootPath = Server.MapPath("/");
                list = thanhVienCommon.ImportFromExcel(physicalWebRootPath, model.LinkExcel);             
                int i = 0;
                foreach (var item in list)
                {
                    // ktr trùng mã số thành viên
                    var thanhVien = _ThanhVienLogic.GetByMaSoThanhVien(item.MaSoThanhVien);
                    if (thanhVien == null)
                    {
                        // Thêm thành viên,lưu mã vạch                        
                        var id = _ThanhVienLogic.Insert(item);
                        ThanhVien tv = _ThanhVienLogic.GetById(id);
                        ThanhVien temp = new ThanhVien();
                        temp = thanhVienCommon.LuuMaVach(physicalWebRootPath, tv, null);
                        if (temp != null)
                        {
                            tv.QRLink = temp.QRLink;
                            tv.QRData = temp.QRData;
                            _ThanhVienLogic.Update(tv);
                        }
                    }
                    else
                    {
                        ViewBag.Duplicate = "Mã thành viên bị trùng ở dòng số " + (item.RowExcel + i).ToString();
                        return View();
                    }
                    i++;
                }
            }
            //return View();
            return RedirectToAction("Index", "HocSinh");
        }

        public ActionResult ExportWord()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ExportWord(UserViewModel model)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            var _ThanhVienLogic = new ThanhVienLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            #endregion
            ExcelManager excelManager = new ExcelManager();
            var listTV = _ThanhVienLogic.GetAllHS();
            string fileName = "MauTheHS.docx";           
            if (model.LinkWord != null)
            {
                string physicalWebRootPath = Server.MapPath("/");
                string uploadForder = GetUploadFolder(Helpers.UploadFolder.FileWord);
              

                var sourceFileName = Path.Combine(physicalWebRootPath, uploadForder, model.LinkWord.FileName);

                string location = Path.GetDirectoryName(sourceFileName);
                if (!Directory.Exists(location))
                {
                    Directory.CreateDirectory(location);
                }
                using (FileStream fileStream = new FileStream(sourceFileName, FileMode.Create))
                {
                    model.LinkWord.InputStream.CopyTo(fileStream);
                    var sourceDir = fileStream.Name.Replace(physicalWebRootPath, "/").Replace(@"\", @"/").Replace(@"//", @"/");
                    fileStream.Close();
                    excelManager.ExportWord(sourceDir, listTV, fileName);
                    // To do Download           
                    string filepath = @"D:\Pro Test\pro2\BiTech.Library\BiTech.Library\Upload\FileWord\" + fileName;
                    byte[] filedata = System.IO.File.ReadAllBytes(filepath);
                    string contentType = MimeMapping.GetMimeMapping(filepath);
                    var cd = new System.Net.Mime.ContentDisposition
                    {
                        FileName = fileName,
                        Inline = true,
                    };
                    Response.AppendHeader("Content-Disposition", cd.ToString());
                    return File(filedata, contentType);
                }
            }
            // return View();
            return RedirectToAction("Index", "HocSinh");
        }

        public ActionResult MauThe(string mauThe)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            var _ThanhVienLogic = new ThanhVienLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            #endregion
            if (mauThe == null)
                return RedirectToAction("NotFound", "Error");
            ExcelManager excelManager = new ExcelManager();
            var listTV = _ThanhVienLogic.GetAllHS();
            string linkMau = null;
            string fileName = "MauTheHS.docx";
            if (mauThe.Equals("mau1") == true)
            {
                linkMau = "/Content/MauWord/Mau1.docx";
                fileName = "MauTheHS-Mau1.docx";
            }
            else if ((mauThe.Equals("mau2") == true))
            {
                linkMau = "/Content/MauWord/Mau2.docx";
                fileName = "MauTheHS-Mau2.docx";
            }
            excelManager.ExportWord(linkMau, listTV,fileName);
            // To do Download           
            string filepath = @"D:\Pro Test\pro2\BiTech.Library\BiTech.Library\Upload\FileWord\" + fileName;
            byte[] filedata = System.IO.File.ReadAllBytes(filepath);
            string contentType = MimeMapping.GetMimeMapping(filepath);
            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = fileName,
                Inline = true,
            };
            Response.AppendHeader("Content-Disposition", cd.ToString());
            return File(filedata, contentType);
        }
    }
}