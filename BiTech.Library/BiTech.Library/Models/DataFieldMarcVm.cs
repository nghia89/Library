using MARC;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BiTech.Library.Models
{
    public class DataFieldMarcVm
    {
        public string Id { get; set; }

        public DateTime CreateDateTime { get; set; }

        public string MaKiemSoat { get; set; }

        public string TenSach { get; set; }

        public string IdTheLoai { get; set; }

        public string IdKeSach { get; set; }

        public string IdNhaXuatBan { get; set; }

        public string LinkBiaSach { get; set; }

        public string SoTrang { get; set; }

        public string NamXuatBan { get; set; }

        public string TomTat { get; set; } = "";

        public string GiaBia { get; set; }

        public double PhiMuonSach { get; set; } = 0;

        public int SoLuongTong { get; set; } = 0;

        public int SoLuongConLai { get; set; } = 0;

        public int SoLanDuocMuon { get; set; } = 0;

        public bool CongKhai { get; set; } = true;

        public string ISBN { get; set; } = "";

        public string IdNgonNgu { get; set; }

        public string DDC { get; set; }

        public string MARC21 { get; set; }

        public string XuatXu { get; set; }

        public string TaiBan { get; set; }

        public string NguoiBienDich { get; set; }

        public string QRlink { get; set; }

        public string QRData { get; set; }
        public bool IsDeleted { get; set; } = true;

        [Required(ErrorMessage = "Vui lòng chọn file")]
        public HttpPostedFileBase[] Files { get; set; }

    }
}