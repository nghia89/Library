using BiTech.Library.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BiTech.Library.Models
{
    /// <summary>
    /// Class use for register, change info, delete - Id action
    /// </summary>
    public class UserViewModel 
    {
        //public UserViewModel()
        //{
        //    ListGioiTinh = new SelectList(new List<string>() { "Nam", "Nữ" });
        //}
       // public SelectList ListGioiTinh { get; set; }

        public string Id { get; set; } 

        [Required(ErrorMessage = "Bạn chưa nhập Username!")]
        [Display(Name ="User name")]
        [StringLength(40, ErrorMessage = "Tên quá dài")]
        public string UserName{ get; set; }

        [Required(ErrorMessage = "Bạn chưa nhập mật khẩu!")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }
      
        [DataType(DataType.Password)]
       // [System.ComponentModel.DataAnnotations.Compare("Password",ErrorMessage =("Mật khẩu không khớp nhau!"))]
        [Required(ErrorMessage = "Bạn chưa nhập lại Mật khẩu!")]
        [Display(Name = "Nhập lại mật khẩu")]
        public string ConfirmPass { get; set; }

        [Required(ErrorMessage = "Bạn chưa nhập tên thành viên!")]
        [Display(Name = "Tên thành viên")]
        public string Ten { get; set; }

        [Required(ErrorMessage = "Bạn chưa chọn giới tính!")]
        [Display(Name = "Giới tính")]
        public string GioiTinh { get; set; }

        [Required(ErrorMessage = "Bạn chưa chọn ngày sinh!")]
        [Display(Name = "Ngày sinh")]     
        [DataType(DataType.Date)]
        public DateTime NgaySinh { get; set; }

        [Required(ErrorMessage = "Bạn chưa nhập mã số thành viên!")]
        [Display(Name ="Mã số thành viên")]
        public string MaSoThanhVien { get; set; }

        [Display(Name = "Lớp học")]
        public string LopHoc { get; set; }

        [Display(Name = "Địa chỉ")]
        public string DiaChi{ get; set; }

        [Display(Name = "Điện thoại")]
        public string SDT{ get; set; }

        [Display(Name = "Trạng thái")]
        public EUser TrangThai{ get; set; }
        [Display(Name = "Chức vụ")]
        public string ChucVu { get; set; }
        //[Display(Name = "Chức vụ")]
        //public string IdChucVu { get; set; }
        [Display(Name = "Hình ảnh")]
        public HttpPostedFileBase HinhChanDung { get; set; }
    }
    public class EditUserViewModel
    {
        [Required]
        [Display(Name = "Tên người dùng")]
        public string Ten { get; set; }

        [Required]
        [Display(Name = "Giới tính")]
        public string GioiTinh { get; set; }

        [Required]
        [Display(Name = "Ngày sinh")]
        [DataType(DataType.Date)]
        public DateTime NgaySinh { get; set; }       

        [Display(Name = "Lớp học")]
        public string LopHoc { get; set; }

        [Display(Name = "Địa chỉ")]
        public string DiaChi { get; set; }

        [Display(Name = "Điện thoại")]
        public string SDT { get; set; }
        [Display(Name = "Hình ảnh")]
        public HttpPostedFileBase HinhChanDung { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

}