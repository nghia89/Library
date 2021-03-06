﻿using BiTech.Library.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BiTech.Library.Controllers.BaseClass;
using System.Collections;

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

        [Required(ErrorMessage = "Bạn chưa nhập tên đăng nhập!")]
        [Display(Name = "User name")]
        [StringLength(40, ErrorMessage = "Tên quá dài")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Bạn chưa nhập mật khẩu!")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = ("Mật khẩu không khớp nhau!"))]
        [Required(ErrorMessage = "Bạn chưa nhập lại mật khẩu!")]
        [Display(Name = "Nhập lại mật khẩu")]
        public string ConfirmPass { get; set; }

        [Required(ErrorMessage = "Bạn chưa nhập tên!")]
        [Display(Name = "Tên thành viên")]
        public string Ten { get; set; }

        [Required(ErrorMessage = "Bạn chưa chọn giới tính!")]
        [Display(Name = "Giới tính")]
        public string GioiTinh { get; set; }

        //[Required(ErrorMessage = "Bạn chưa chọn ngày sinh!")]
        [Display(Name = "Ngày sinh")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Bạn chưa chọn ngày sinh!")]
        public DateTime NgaySinh { get; set; }
        [Required(ErrorMessage = "Bạn chưa chọn ngày sinh!")]
        public string TemptNgaySinh { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhập mã số!"), StringLength(12, ErrorMessage = "Độ dài mã số cần < 12")]
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

        [Display(Name = "Hình ảnh")]
        public HttpPostedFileBase HinhChanDung { get; set; }

        [Display(Name = "Mã QR")]
        public string QRLink { get; set; }

        public List<string> ListNienKhoa { get; set; }

        public string LinkAvatar { get; set; }
       
        public string[] ListAll { get; set; }

        public List<ThanhVien> ListThanhVien { get; set; }
       
        public HttpPostedFileBase LinkWord { get; set; }      

        public string TextForSearch { get; set; }

		[Display(Name = "Tổ")]
		public string ChucVu { get; set; } //Sài cho giáo viên        
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

        [Required(ErrorMessage = "Bạn chưa nhập tên người dùng")]
        [Display(Name = "Tên người dùng")]
        public string Ten { get; set; }

        [Required(ErrorMessage = "Bạn chưa chọn giới tính")]
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
		[Display(Name = "Tổ")]
		public string ChucVu { get; set; } //sài cho giáo viên
    }

    public class ChangePasswordViewModel
    {
        public string Id { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Bạn chưa nhập mật khẩu hiện tại!")]
        [Display(Name = "Mật khẩu hiện tại")]
        public string OldPassword { get; set; }
       
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Bạn chưa nhập mật khẩu mới!")]
        [Display(Name = "Mật khẩu mới")]
        public string NewPassword { get; set; }


        [DataType(DataType.Password)]
        [Display(Name = "Nhập lại mật khẩu mới")]
        [Required(ErrorMessage = "Bạn chưa nhập lại mật khẩu!")]
        [System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessage = "Nhập lại mật khẩu không khớp nhau!")]
        public string ConfirmPassword { get; set; }       
    }

    public class ImportExcelTVViewModel
    {
        public List<string[]> RawDataList { get; set; }    
        public int TotalEntry { get; set; }     
        public List<ThanhVien> ListSuccess { get; set; } = new List<ThanhVien>();
        public List<ThanhVien> ListFail { get; set; } = new List<ThanhVien>();
        public List<ArrayList> ListShow { get; set; } = new List<ArrayList>();
        public string FileName { get; set; }
        public string FilePath { get; set; }
        /// <summary>
        /// Mảng chứa các dòng bị trùng Mã Thành Viên
        /// </summary>
        public bool[] ArrRowsMSTV { get; set; }
        public bool[] ArrRowsUser { get; set; }
    }

    public class ListMemberModel
    {
        public List<UserViewModel> Members { get; set; } = new List<UserViewModel>();
    }
	
    public class DeleteMultiViewModel
    {
        public List<UserViewModel> ListThanhVien { get; set; } = new List<UserViewModel>();       
    }
}