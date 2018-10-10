using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BiTech.Library.Models
{
    public class PhieuXuatSachModels
    {
        public string IdPhieuXuat { get; set; }
        public string GhiChu { get; set; } = "";

        public DateTime NgayXuat { get; set; }

        public string IdUserAdmin { get; set; }

        public string UserName { get; set; }

        public List<string> listChiTietJsonString { get; set; } = new List<string>();


    }

    public class ChiTietXuatSachViewModels
    {
        public string Id { get; set; }

        public string IdPhieuXuat { get; set; }

        public string IdSach { get; set; }
     
        public string IdTinhTrang { get; set; }

        public string tenTinhTrang { get; set; }
        
        public int soLuong { get; set; }

        public string ten { get; set; }

        public string GhiChuDon { get; set; }

        public string MaKiemSoat { get; set; }
        public string MaCaBiet { get; set; }
    }

    public class ChiTietXuatModels
    {
        public string Id { get; set; }
        public string TenSach { get; set; }
        public string MaCaBiet { get; set; }
        public string MaKiemSoat { get; set; }
        public string IdTinhTrang { get; set; }
        public string IdSach { get; set; }
        public string TrangThai { get; set; }
    }
}