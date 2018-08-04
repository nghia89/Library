using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BiTech.Library.Models
{
    public class TrangThaiSachViewModels
    {
        public string Id { get; set; }
        public string TenTT { get; set; }
        public bool TrangThai { get; set; } = false;
    }
}