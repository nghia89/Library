using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiTech.Library.DTO
{
    public class ChiTietPhieuMuon_FullDetail : ChiTietPhieuMuon
    {
        public int SoLuongSachDaTra { get; set; }

        public bool IsPaid { get { return SoLuong == SoLuongSachDaTra; } }
    }
}
