using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BiTech.Library.Models
{
    public class ChucVuModels
    {
        public string Id { get; set; }

        [Display(Name = "Mã chức vụ")]
        [Required]
        public string MaChucVu { get; set; }

        [Display(Name = "Tên chức vụ")]
        [Required]
        public string TenChucVu { get; set; }
    }
}