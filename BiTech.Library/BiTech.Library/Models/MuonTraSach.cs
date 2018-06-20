using BiTech.Library.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BiTech.Library.Models
{
    public class PhieuMuonModelView
    {
        public string Id { get; set; }
        
        [Display(Name = "Người mượn")]
        public string TenNguoiMuon { get; set; }

        [Required]
        [Display(Name = "Mã người mượn")]
        public string IdUser { get; set; }

        [Required]
        [Display(Name = "Mã sách")]
        public List<String> MaSach { get; set; }

        [Display(Name = "Tên sách")]
        public string TenSach{ get; set; }

        [Required]
        [Display(Name = "Số lượng")]
        public int SoLuong { get; set; }

        [Required]
        [Display(Name = "Ngày mượn")]
        public DateTime NgayMuon { get; set; }
        
        [Display(Name = "Ngày phải trả")]
        public DateTime NgayPhaiTra { get; set; }

        [Display(Name = "Ngày trả")]
        public DateTime? NgayTra { get; set; }

        [Display(Name = "Trạng thái")]
        public string TrangThaiPhieu { get; set; }

        [Display(Name = "Gia hạn")]
        public string GiaHan{ get; set; }

        [Display(Name = "Ghi chú")]
        public string GhiChu{ get; set; }
    }

    public class ChiTietPMViewModel
    {
        public string IdPM { get; set; }

        [Display(Name = "Mã sách")]
        public string MaSach { get; set; }

        [Display(Name = "Tên sách")]
        public string TenSach { get; set; }

        [Display(Name = "Số lượng mượn")]
        public int SoLuong { get; set; }

        [Display(Name = "Ngày mượn")]
        public DateTime NgayMuon { get; set; }

        [Display(Name = "Ngày trả")]
        public DateTime NgayTra { get; set; }
    }
}