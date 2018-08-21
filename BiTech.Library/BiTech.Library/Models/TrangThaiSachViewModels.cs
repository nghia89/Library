using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BiTech.Library.Models
{
    public class TrangThaiSachViewModels
    {
        public string Id { get; set; }
        [Display(Name = "Tên trạng thái")]
        [Required(ErrorMessage = "Vui lòng nhập tên trạng thái")]
        public string TenTT { get; set; }
        public bool TrangThai { get; set; } = false;
    }
}