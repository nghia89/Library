using BiTech.Library.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BiTech.Library.Helpers
{
    public class SachViewModel
    {
        public string Id { get; set; }
        public string TenSach { get; set; }
        public string IdDauSach { get; set; }
        public string IdTheLoai { get; set; }
        public string IdNhaXuatBan { get; set; }
        public string IdTrangThai { get; set; }

        public string Hinh { get; set; }
        public string SoLuong { get; set; }
        public int SoTrang { get; set; }
        public ENgonNgu NgonNgu { get; set; }

        public string NamSanXuat { get; set; }
        public string GiaSach { get; set; }
        public string LinkBiaSach { get; set; }
        public string TomTat { get; set; }
    }
    public class TheLoaiSachViewModel
    {
        [Display(Name = "Tên thể loại sách")]
        public string TenTheLoai { get; set; }
        [Display(Name = "Mô tả")]
        public string MoTa { get; set; }
    }
}