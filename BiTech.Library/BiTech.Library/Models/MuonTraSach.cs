using BiTech.Library.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BiTech.Library.Models
{
    public class MuonTraSachViewModel
    {
        public string Id { get; set; }
        public string IdUser { get; set; }
        public string MaKiemSoat { get; set; }
        public string TenSach { get; set; }
        public string SoLuong { get; set; }
        public string SoLuongMax { get; set; }
        public string NgayMuon { get; set; }
        public string NgayTra { get; set; }
        public string TinhTrangSach { get; set; }
        public bool TinhTrang { get; set; }
    }

    public class MuonTraSachCheckViewTable
    {
        public string MaKiemSoat { get; set; }
        public string NgayMuon { get; set; }
        public string NgayTra { get; set; }
    }
}