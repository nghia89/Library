using BiTech.Library.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BiTech.Library.Controllers.BaseClass;
namespace BiTech.Library.Models
{
    /// <summary>
    /// Class use for register, change info, delete - Id action
    /// </summary>
    public class UserViewModel
    {
        ThanhVienCommon thanhVienCommon;
        public UserViewModel()
        {
            thanhVienCommon = new ThanhVienCommon();            
            ListNienKhoa = thanhVienCommon.TaoNienKhoa();
        }
        public string Id { get; set; }

        [Required(ErrorMessage = "Bạn chưa nhập Username!")]
        [Display(Name = "User name")]
        [StringLength(40, ErrorMessage = "Tên quá dài")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Bạn chưa nhập mật khẩu!")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        // [System.ComponentModel.DataAnnotations.Compare("Password",ErrorMessage =("Mật khẩu không khớp nhau!"))]
        [Required(ErrorMessage = "Bạn chưa nhập lại mật khẩu!")]
        [Display(Name = "Nhập lại mật khẩu")]
        public string ConfirmPass { get; set; }

        [Required(ErrorMessage = "Bạn chưa nhập tên thành viên!")]
        [Display(Name = "Tên thành viên")]
        public string Ten { get; set; }

        [Required(ErrorMessage = "Bạn chưa chọn giới tính!")]
        [Display(Name = "Giới tính")]
        public string GioiTinh { get; set; }

        //[Required(ErrorMessage = "Bạn chưa chọn ngày sinh!")]
        [Display(Name = "Ngày sinh")]
        [DataType(DataType.Date)]
        public DateTime NgaySinh { get; set; }

        [Required(ErrorMessage = "Bạn chưa nhập mã số thành viên!")]
        [Display(Name = "Mã số thành viên")]
        public string MaSoThanhVien { get; set; }

        [Display(Name = "Lớp học")]
        public string LopHoc { get; set; }
        [Display(Name = "Niên khóa")]
        public string NienKhoa { get; set; }

        [Display(Name = "Địa chỉ")]
        public string DiaChi { get; set; }

        [Display(Name = "Điện thoại")]
        public string SDT { get; set; }

        [Display(Name = "Trạng thái")]
        public EUser TrangThai { get; set; }        
        //[Display(Name = "Chức vụ")]
        //public string IdChucVu { get; set; }
        [Display(Name = "Hình ảnh")]
        public HttpPostedFileBase HinhChanDung { get; set; }
        [Display(Name = "Mã QR")]
        public string QRLink { get; set; }
        public List<string> ListNienKhoa { get; set; }
        public string LinkAvatar { get; set; }        
        public string[] ListName { get; set; }
        public string[] ListMaTV { get; set; }
        public List<ThanhVien> ListThanhVien { get; set; }
        [Display(Name ="Đường dẫn file Excel")]
        public HttpPostedFileBase LinkExcel { get; set; }
        public HttpPostedFileBase LinkWord { get; set; }
        public string LoaiTK { get; set; }
    }
    public class EditUserViewModel
    {
        public EditUserViewModel()
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
            ListNienKhoa = listNienKhoa;           
        }
        public string Id { get; set; }

        [Required]
        [Display(Name = "Tên người dùng")]
        public string Ten { get; set; }

        [Required]
        [Display(Name = "Giới tính")]
        public string GioiTinh { get; set; }
      
        //[Required]
        [Display(Name = "Ngày sinh")]
        [DataType(DataType.Date)]
        public DateTime NgaySinh { get; set; }

        [Display(Name = "Lớp học")]
        public string LopHoc { get; set; }
        [Display(Name = "Niên khóa")]
        public string NienKhoa { get; set; }

        [Display(Name = "Địa chỉ")]
        public string DiaChi { get; set; }

        [Display(Name = "Điện thoại")]
        public string SDT { get; set; }
        [Display(Name = "Hình ảnh")]
        public HttpPostedFileBase HinhChanDung { get; set; }
        public List<string> ListNienKhoa { get; set; }
        public string LinkAvatar { get; set; }
        public string LoaiTK { get; set; }
    }

    public class ChangePasswordViewModel
    {
        public string Id { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu hiện tại")]
        public string OldPassword { get; set; }


        // [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu mới")]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Nhập lại mật khẩu mới")]
        [System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessage = "Nhập lại mật khẩu không khớp nhau!")]
        public string ConfirmPassword { get; set; }
        public string LoaiTK { get; set; }
    }

}