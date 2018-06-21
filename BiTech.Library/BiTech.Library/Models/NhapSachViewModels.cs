using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BiTech.Library.Models
{
    public class NhapSachViewModels
    {
        public string Id { get; set; }

        public DateTime NgayNhap { get; set; }

        public int SoLuong { get; set; }

        public string IdSach { get; set; }

        public string TenSach { get; set; }
    }
}