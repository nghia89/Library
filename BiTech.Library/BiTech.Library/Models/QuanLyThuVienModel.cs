using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BiTech.Library.Models
{
    public class ThongTinThuVienModel
    {
        [Required]
        public string TenThuVien { get; set; }

        [Required]
        public string TheHeader1 { get; set; }

        [Required]
        public string TheHeader2 { get; set; }

        [Required]
        public string DiaChi { get; set; }
    }

    public class BackupFileModel
    {
        public string Name { get; set; }

        public string Size { get; set; }

        public string Date { get; set; }
    }
}