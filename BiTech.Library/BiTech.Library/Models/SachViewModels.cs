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
        public string IdDauSach { get; set; }
        public string IdTheLoai { get; set; }
        public string IdKeSach { get; set; }
        public string IdNhaXuatBan { get; set; }
        public string IdTrangThai { get; set; }

        public string MaKiemSoat { get; set; }

        public string Hinh { get; set; }
        public int SoLuong { get; set; }
        public int SoTrang { get; set; }
        public ENgonNgu NgonNgu { get; set; }

        public string NamSanXuat { get; set; }
        public double GiaSach { get; set; }
        public string LinkBiaSach { get; set; }
        public string TomTat { get; set; }

    }
}