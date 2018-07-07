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
        
        [Display(Name = "Ngày mượn")]
        [DataType(DataType.Date)]
        public DateTime NgayMuon { get; set; }
        
        [Display(Name = "Ngày phải trả")]
        [DataType(DataType.Date)]
        public DateTime NgayPhaiTra { get; set; }

        [Display(Name = "Ngày trả")]
        [DataType(DataType.Date)]
        public DateTime? NgayTra { get; set; }

        [Display(Name = "Trạng thái")]
        public EPhieuMuon TrangThaiPhieu { get; set; }

        [Display(Name = "Trạng thái")]
        public string TrangThai{ get; set; }

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

    public class PhieuTraViewModel
    {
        public string IdPM { get; set; }

        [Display(Name = "Mã người mượn")]
        public string IdNguoiMuon { get; set; }

        [Display(Name = "Người mượn")]
        public string NguoiMuon { get; set; }

        public List<TrangThaiSach> ListTrangThai { get; set; } = new List<TrangThaiSach>();

        public PhieuMuonSach_FullDetail detail { get; set; }

        public List<string> listChiTietJsonString { get; set; } = new List<string>();
    }

    public class ChiTietPhieuTraViewModel
    {
        public string IdPT { get; set; }

        [Display(Name = "Mã sách")]
        public string IdSach { get; set; }

        [Display(Name = "Tên sách")]
        public string TenSach { get; set; }

        [Display(Name = "Số lượng")]
        public int SoLuong { get; set; }

        [Display(Name = "Người mượn")]
        public string NguoiMuon { get; set; }

        [Display(Name = "Mã người mượn")]
        public string IdNguoiMuon { get; set; }

        public string IdTrangThaiSach { get; set; }

        public string TrangThaiSach { get; set; }
    }

    public class TrangThai
    {
        public string Id { get; set; }

        public string TenTT { get; set; }
    }
    
    public class SachPreLoad
    {
        public string Id { get; set; }

        public string IdDauSach { get; set; }

        public string MaKiemSoat { get; set; }

        public string TenSach { get; set; }

        public int SoLuongMuon { get; set; }

        public bool Status { get; set; }
    }

}