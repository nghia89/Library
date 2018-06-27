using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BiTech.Library.Models
{
    public class PhieuNhapSachModel
    {
        public string GhiChu { get; set; } = "";

        public List<TrangThai> ListTrangThai { get; set; } = new List<TrangThai>();

        public List<string> listChiTietJsonString { get; set; } = new List<string>();
    }

    public class TrangThai
    {
        public string Id { get; set; }

        public string Ten { get; set; }
    }
}