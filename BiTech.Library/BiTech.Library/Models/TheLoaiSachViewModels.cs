using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BiTech.Library.Models
{
    public class TheLoaiSachViewModels
    {
        public string Id { get; set; }
        public string IdParent { get; set; }
        public string TenTheLoaiParent { get; set; }
        public string TenTheLoai { get; set; }
        public string MoTa { get; set; }
        public HttpPostedFileBase LinkExcel { get; set; }
    }
}