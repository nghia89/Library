using BiTech.Library.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BiTech.Library.Models
{
    /// <summary>
    /// Class use for register, change info, delete - Id action
    /// </summary>
    public class UserViewModel 
    {
        public string Id { get; set; } 

        [Required]
        [Display(Name ="User name")]
        public string UserName{ get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [Display(Name ="Tên người dùng")]
        public string MaSoThanhVien { get; set; }

        [Display(Name ="CMND")]
        public string CMND { get; set; }

        [Display(Name = "Địa chỉ")]
        public string DiaChi{ get; set; }

        [Display(Name = "Điện thoại")]
        public string SDT{ get; set; }

        [Display(Name = "Trạng thái")]
        public EUser TrangThai{ get; set; }
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
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

}