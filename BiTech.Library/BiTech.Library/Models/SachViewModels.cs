using BiTech.Library.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BiTech.Library.Models
{
    public class SachViewModels
    {
        public string Id { get; set; }

        [Required]
        public string TenSach { get; set; }
        

        [Required]
        public string IdDauSach { get; set; }

        [Required]
        public string IdTheLoai { get; set; }

        [Required]
        public string IdKeSach { get; set; }

        [Required]
        public string IdNhaXuatBan { get; set; }
        
        public string IdTrangThai { get; set; }

        [Required]
        public string MaKiemSoat { get; set; }

        [Required]
        public string Hinh { get; set; }

        [Required]
        public int SoLuong { get; set; }

        [Required]
        public int SoTrang { get; set; }

        [Required]
        public ENgonNgu NgonNgu { get; set; }
        
        public string NamSanXuat { get; set; }

        [Required]
        public double GiaSach { get; set; }

        [Required]
        public string LinkBiaSach { get; set; }

        [Required]
        public string TomTat { get; set; }

    }
    
    public class ListBooks
    {
        public List<BookView> Books { get; set; } = new List<BookView>();
    }

    public class BookView
    {
        public string Id { get; set; }
        
        [Required]
        public string IdDauSach { get; set; }

        [Required]
        public string MaKiemSoat { get; set; }

        [Required]
        public string TenSach { get; set; }

        [Required]
        public string TheLoai { get; set; }

        [Required]
        public string KeSach { get; set; }

        [Required]
        public string NhaXuatBan { get; set; }

        [Required]
        public string NamXuatBan { get; set; }

        [Required]
        public string GiaSach { get; set; }

        [Required]
        public string LinkBiaSach { get; set; }

        [Required]
        public string NgonNgu { get; set; }

        [Required]
        public string TomTat { get; set; }

        [Required]
        public int SoLuongTong { get; set; }

        [Required]
        public int SoLuongConLai { get; set; }
    }
}