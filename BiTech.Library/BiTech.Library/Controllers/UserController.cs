using BiTech.Library.BLL.DBLogic;
using BiTech.Library.DTO;
using BiTech.Library.Helpers;
using BiTech.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BiTech.Library.Controllers
{
    public class UserController : BaseController
    {
        private ThanhVienLogic _ThanhVienLogic;
        public UserController()
        {
            _ThanhVienLogic = new ThanhVienLogic(Tool.GetConfiguration("ConnectionString"), Tool.GetConfiguration("DatabaseName"));
        }

        // GET: User
        public ActionResult Index()
        {
            return View();
        }
        public PartialViewResult _PartialUser()
        {
            List<ThanhVien> lstUser = _ThanhVienLogic.GetAll();
            return PartialView(lstUser);
        }

        public ActionResult _CreateUser()
        {
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
            string id = _ThanhVienLogic.Insert(model);
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
    }
}