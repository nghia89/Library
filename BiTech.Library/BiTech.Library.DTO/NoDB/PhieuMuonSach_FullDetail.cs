using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiTech.Library.DTO
{
    public class PhieuMuonSach_FullDetail : PhieuMuon
    {
        public List<ChiTietPhieuMuon_FullDetail> BookList { get; set; } = new List<ChiTietPhieuMuon_FullDetail>();
    }
}
