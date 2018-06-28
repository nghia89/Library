using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BiTech.Library.Models
{
    public class PhieuXuatSachModels
    {
        public string GhiChu { get; set; } = "";

   
        public List<string> listChiTietJsonString { get; set; } = new List<string>();


    }
    public class ChiTietXuatSachViewModels
    {
        public string Id { get; set; }

        public string IdPhieuNhap { get; set; }

        public string IdSach { get; set; }

        public string IdTinhTrang { get; set; }

        public string tenTinhTrang { get; set; }

        public string IdLydo { get; set; }

        public string lyDo { get; set; }

        public int soLuong { get; set; }

        public string ten { get; set; }

        public string IdDauSach { get; set; }
    }


}