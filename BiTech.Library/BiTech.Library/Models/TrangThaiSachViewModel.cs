using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BiTech.Library.Models
{
    public class TrangThaiSachViewModel
    {
        public string Id { get; set; }

        [Display(Name = "Tên trạng thái sách")]
        public string TenTT { get; set; }

        [Display(Name = "Ký hiệu")]
        public string KyHieu { get; set; }
    }
}