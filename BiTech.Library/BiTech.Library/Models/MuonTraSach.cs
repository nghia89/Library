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
        public List<string> MaSach { get; set; }

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

        [Display(Name = "Trạng thái")]
        public EPhieuMuon TrangThaiPhieu { get; set; }
    }
}