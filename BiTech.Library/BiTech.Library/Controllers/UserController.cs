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
    public class UserController : BaseController
    {
        public UserController()
        {          
        }

        // GET: User
        public ActionResult Index(string IdUser)
        {
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
                lstUser.Add(_ThanhVienLogic.GetByIdUser(IdUser));
            }
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
            BarCodeQRManager barcode = new BarCodeQRManager();
            var _ThanhVienLogic = new ThanhVienLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

            ThanhVien model = new ThanhVien()
            {
                UserName = viewModel.UserName,
                Password = viewModel.Password,
                Ten = viewModel.Ten,
                MaSoThanhVien = viewModel.MaSoThanhVien,
                CMND = viewModel.CMND,
                DiaChi = viewModel.DiaChi,
                SDT = viewModel.SDT,
                TrangThai = EUser.Active, // mac dinh la Active
                CreateDateTime = DateTime.Now
            };          
            //insert
            string id = _ThanhVienLogic.Insert(model);
            try
            {
                // Lấy đường dẫn lưu QR
                string physicalWebRootPath = Server.MapPath("~/");

                string uploadFolder = GetUploadFolder(Helpers.UploadFolder.QRCodeUser);

                var uploadFileName = Path.Combine(physicalWebRootPath, uploadFolder, id + ".bmp");
                string location = Path.GetDirectoryName(uploadFileName);
                if (!Directory.Exists(location))
                {
                    Directory.CreateDirectory(location);
                }

                var a = uploadFileName.Replace(physicalWebRootPath, "~/").Replace(@"\", @"/").Replace(@"//", @"/");
                //Tạo mã QR
                bool bolQR = barcode.CreateQRCode(model.Id, a);
            }
            catch (Exception ex)
            {
                
            }
            if(id == null || id == "")
            {
                //Fail              
                TempData["UnSuccess"] = "Thêm mới thất bại";
                return View();
            }
            else
                TempData["Success"] = "Thêm mới thành công";
            return RedirectToAction("Index", "User");
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
            ThanhVien us = _ThanhVienLogic.GetById(id);
            UserViewModel model = new UserViewModel()
            {
                Ten = us.Ten,
                MaSoThanhVien = us.MaSoThanhVien,
                CMND = us.CMND,
                DiaChi  = us.DiaChi,
                SDT = us.SDT,
                TrangThai = us.TrangThai,                
            };
            return View(model);
        }
        [HttpPost]
        public ActionResult _Edit(UserViewModel viewModel, string id)
        {
            #region  Lấy thông tin người dùng
            var userdata = GetUserData();
            if (userdata == null)
                return RedirectToAction("LogOff", "Account");
            #endregion

            var _ThanhVienLogic = new ThanhVienLogic(userdata.MyApps[AppCode].ConnectionString, userdata.MyApps[AppCode].DatabaseName);

            var model = _ThanhVienLogic.GetById(id); //lay 1 tai khoan 
                model.Ten = viewModel.Ten;
                model.MaSoThanhVien = viewModel.MaSoThanhVien;
                model.CMND = viewModel.CMND;
                model.DiaChi = viewModel.DiaChi;
                model.SDT = viewModel.SDT;
                model.TrangThai = viewModel.TrangThai;

                //Cap nhat voi tai khoan moi
                bool result = _ThanhVienLogic.Update(model);
                if (result == true)
                {
                    TempData["Success"] = "Cập nhật thành công";
                    return RedirectToAction("Index", "User");
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
                return RedirectToAction("Index", "User");
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
        public ActionResult Search(UserViewModel model  )
        {
            return RedirectToAction("Index", new { @IdUser = model.MaSoThanhVien });
        }        
    }
}