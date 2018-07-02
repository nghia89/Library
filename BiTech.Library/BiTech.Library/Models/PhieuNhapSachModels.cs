using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BiTech.Library.Models
{
    public class PhieuNhapSachModel
    {
        [Display(Name = "Ghi Chú")]
        [Required(ErrorMessage = "Vui lòng nhập ghi chú")]
        public string GhiChu { get; set; } = "";

        public List<string> listChiTietJsonString { get; set; } = new List<string>();

        public DateTime NgayNhap { get; set; }

        public string IdUserAdmin { get; set; }
    }
    public class ChiTietNhapSachViewModels
    {
        public string Id { get; set; }

        public string IdPhieuNhap { get; set; }

        public string IdSach { get; set; }

        public string IdTinhTrang { get; set; }

        public string tenTinhTrang { get; set; }
        [Display(Name = "Số Lượng")]
        [Required(ErrorMessage = "Vui lòng nhập số lượng")]
        [Range(0, 999, ErrorMessage = "Số nhập vào phải lớn hơn 0")]
        public int soLuong { get; set; }

        public string ten { get; set; }

        public string IdDauSach { get; set; }

     

       

    }

}