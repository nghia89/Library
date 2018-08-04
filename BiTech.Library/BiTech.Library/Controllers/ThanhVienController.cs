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
using static BiTech.Library.Helpers.Tool;

namespace BiTech.Library.Controllers
{
    //[Authorize]
    public class ThanhVienController : BaseController
    {
        public ThanhVienController()
        {
        }

        // GET: User
        public ActionResult Index(string IdUser)
        {
            ViewBag.SearchFail = TempData["SearchFail"];
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            if (String.IsNullOrEmpty(IdUser))
                IdUser = "";
            ViewBag.IdUser = IdUser;
            return View();
        }

        public PartialViewResult _PartialUser(string IdUser)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return null;
            #endregion

            List<ThanhVien> lstUser = new List<ThanhVien>();
            var _ThanhVienLogic = new ThanhVienLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            if (String.IsNullOrEmpty(IdUser))
            {
                lstUser = _ThanhVienLogic.GetAll();
            }
            else
            {
                if (_ThanhVienLogic.GetByMaSoThanhVien(IdUser) != null)
                {
                    lstUser.Add(_ThanhVienLogic.GetByMaSoThanhVien(IdUser));
                }
                else
                {
                    ViewBag.SearchFail = TempData["SearchFail"] = "Tìm kiếm thất bại";
                    lstUser = _ThanhVienLogic.GetAll();
                }
            }
            ViewBag.stt = lstUser.Count;
            return PartialView(lstUser);

            //return new JsonResult(new { tensach = "", cover = "" });
        }
        public ActionResult _CreateUser()
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            ViewBag.Success = TempData["Success"];
            ViewBag.UnSuccess = TempData["UnSuccess"];
            ViewBag.IdUser = TempData["IdUser"];
            ViewBag.Duplicate = TempData["Duplicate"];


            return View();
        }

        /// <summary>
        /// Create User
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _CreateUser(UserViewModel viewModel)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion
            var _ThanhVienLogic = new ThanhVienLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);
            BarCodeQRManager barcode = new BarCodeQRManager();

            if (viewModel.MaSoThanhVien == null || viewModel.Ten == null || viewModel.Password == null)
            {
                TempData["IdUser"] = "Dữ liệu không phù hợp";
                return View();
            }
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
                ChucVu = viewModel.ChucVu,
                //IdChucVu = viewModel.IdChucVu,
                TrangThai = EUser.Active, // mac dinh la Active
                CreateDateTime = DateTime.Now
            };
            // Kiem tra trung ma thanh vien
            var idMaThanhVien = _ThanhVienLogic.GetByMaSoThanhVien(viewModel.MaSoThanhVien);
            if (idMaThanhVien == null)
            {
                //insertl
                string id = _ThanhVienLogic.Insert(thanhVien);
                // Lưu hình ảnh     

                if (viewModel.HinhChanDung != null)
                {
                    try
                    {
                        string physicalWebRootPath = Server.MapPath("~/");
                        string uploadFolder = GetUploadFolder(Helpers.UploadFolder.AvatarUser);
                        //var uploadFileName = Path.Combine(physicalWebRootPath, uploadFolder, Guid.NewGuid()
                        //    + Path.GetExtension(viewModel.HinhChanDung.FileName));
                        var uploadFileName = Path.Combine(physicalWebRootPath, uploadFolder, thanhVien.MaSoThanhVien + "-" + viewModel.HinhChanDung.FileName);

                        //string location = Path.GetDirectoryName(uploadFileName);
                        //if (!Directory.Exists(location))
                        //{
                        //    Directory.CreateDirectory(location);
                        //}
                        using (FileStream fileStream = new FileStream(uploadFileName, FileMode.Create))
                        {
                            viewModel.HinhChanDung.InputStream.CopyTo(fileStream);
                            var tv = _ThanhVienLogic.GetById(id);
                            tv.HinhChanDung = uploadFileName.Replace(physicalWebRootPath, "/").Replace(@"\", @"/").Replace(@"//", @"/");
                            _ThanhVienLogic.Update(tv);
                        }
                    }
                    catch { }
                }

                // Lưu mã vạch
                try
                {
                    // Lấy đường dẫn lưu QR
                    string physicalWebRootPath = Server.MapPath("~/");
                    string uploadFolder = GetUploadFolder(Helpers.UploadFolder.QRCodeUser);

                    var uploadFileNameQR = Path.Combine(physicalWebRootPath, uploadFolder, thanhVien.MaSoThanhVien + "-" + thanhVien.Ten + ".bmp");
                    var uploadFileNameEAN13 = Path.Combine(physicalWebRootPath, uploadFolder, id + "EAN13.bmp");
                    var uploadFileNameISBN = Path.Combine(physicalWebRootPath, uploadFolder, id + "ISBN.bmp");
                    // chuyển đường dẫn vật lý thành đường dẫn ảo
                    var pathQR = uploadFileNameQR.Replace(physicalWebRootPath, "~/").Replace(@"\", @"/").Replace(@"//", @"/");
                    var pathEAN13 = uploadFileNameEAN13.Replace(physicalWebRootPath, "~/").Replace(@"\", @"/").Replace(@"//", @"/");
                    var pathISBN = uploadFileNameISBN.Replace(physicalWebRootPath, "~/").Replace(@"\", @"/").Replace(@"//", @"/");
                    //Tạo mã QR
                    string info = thanhVien.UserName + " " + thanhVien.Ten;

                    bool bolQR = barcode.CreateQRCode(info, pathQR);
                    //string strISBN = "9786045523032";
                    //string strEAN13 = "8936117740497";
                    //bool bolQR = barcode.CreateBarCode(strEAN13, strISBN, pathEAN13, pathISBN);
                }
                catch (Exception ex)
                {
                    TempData["UnSuccess"] = "Tạo mã QR thất bại\r\n" + ex.Message;
                    return View();
                }
                if (id == null || id == "")
                {
                    //Fail
                    ViewBag.UnSuccess = TempData["UnSuccess"] = "Thêm mới thất bại";
                    return View();
                }
                else
                    ViewBag.Success = "Thêm mới thành công";
            }
            else
            {
                ViewBag.Duplicate = TempData["Duplicate"] = "Trùng mã";
                return View();
            }
            return RedirectToAction("Index", "ThanhVien");
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
            tv.NgaySinh.ToString("dd-MM-yyyy");

            EditUserViewModel model = new EditUserViewModel()
            {
                // thông tin cho phép cập nhật
                Ten = tv.Ten,
                DiaChi = tv.DiaChi,
                SDT = tv.SDT,
                GioiTinh = tv.GioiTinh,
                NgaySinh = tv.NgaySinh,
                LopHoc = tv.LopHoc

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
        public ActionResult _Edit(EditUserViewModel viewModel, string id)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            var _ThanhVienLogic = new ThanhVienLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

            var thanhVien = _ThanhVienLogic.GetById(id); //lay 1 tai khoan 
            // thông tin cho phép cập nhật            
            thanhVien.LopHoc = viewModel.LopHoc;
            thanhVien.Ten = viewModel.Ten;
            thanhVien.DiaChi = viewModel.DiaChi;
            thanhVien.GioiTinh = viewModel.GioiTinh;
            thanhVien.NgaySinh = viewModel.NgaySinh;
            thanhVien.SDT = viewModel.SDT;
            // Cập nhật thông tin tài khoản
            bool resultInfo = _ThanhVienLogic.Update(thanhVien);
            bool resultImage = false;
            if (viewModel.HinhChanDung != null)
            {
                try
                {
                    string physicalWebRootPath = Server.MapPath("~/");
                    string uploadFolder = GetUploadFolder(Helpers.UploadFolder.AvatarUser);
                    var uploadFileName = Path.Combine(physicalWebRootPath, uploadFolder, thanhVien.MaSoThanhVien + "-" + viewModel.HinhChanDung.FileName);
                    //var oldFileName = Path.Combine(physicalWebRootPath, uploadFolder, thanhVien.HinhChanDung);
                    //System.IO.File.Delete(oldFileName);
                    string location = Path.GetDirectoryName(uploadFileName);
                    if (!Directory.Exists(location))
                    {
                        Directory.CreateDirectory(location);
                    }

                    using (FileStream fileStream = new FileStream(uploadFileName, FileMode.Create))
                    {
                        viewModel.HinhChanDung.InputStream.CopyTo(fileStream);
                        var tv = _ThanhVienLogic.GetById(id);
                        tv.HinhChanDung = uploadFileName.Replace(physicalWebRootPath, "/").Replace(@"\", @"/").Replace(@"//", @"/");
                        resultImage = _ThanhVienLogic.Update(tv);
                    }

                }
                catch (Exception ex)
                {
                    ViewBag.ErrorImage = ex.ToString();
                    throw ex;
                }
            }
            if (resultInfo == true || resultImage == true)
            {
                TempData["Success"] = "Cập nhật thành công";
                return RedirectToAction("Index", "ThanhVien");
            }
            else
                TempData["UnSuccess"] = "Cập nhật không thành công";
            return View();
        }

        public ActionResult Delete(string id)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            var _ThanhVienLogic = new ThanhVienLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

            ViewBag.Success = TempData["Success"];
            var model = _ThanhVienLogic.GetById(id);
            model.TrangThai = EUser.Deleted;
            bool result = _ThanhVienLogic.Update(model);
            if (result == true)
            {
                TempData["Success"] = "Xóa thành công";
                return RedirectToAction("Index", "ThanhVien");
            }
            else
                TempData["Success"] = "Xóa thất bại";
            return View();
        }
        /// <summary>
        /// Tìm kiếm
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Search(UserViewModel model)
        {
            return RedirectToAction("Index", new { @IdUser = model.MaSoThanhVien });
        }

        /// <summary>F
        /// Giao diện thêm thể loại
        /// </summary>
        /// <returns></returns>
        public ActionResult RequestChucVuGui()
        {
            return PartialView("_NhapChucVu");
        }

    }
}