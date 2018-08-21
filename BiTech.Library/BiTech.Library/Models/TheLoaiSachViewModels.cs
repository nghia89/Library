using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BiTech.Library.Models
{
    public class TheLoaiSachViewModels
    {
        public string Id { get; set; }
        public string IdParent { get; set; }
        public string TenTheLoaiParent { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tên thể loại")]
        public string TenTheLoai { get; set; }
        public string MoTa { get; set; }
        public string MaDDC { get; set; }
        public HttpPostedFileBase LinkExcel { get; set; }
    }
}