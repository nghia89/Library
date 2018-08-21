using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BiTech.Library.Models
{
   public class KesachViewModels
    {
        public string Id { get; set; }
        public DateTime CreateDateTime { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tên kệ sách")]
        public string TenKe { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập vị trí kệ sách")]
        public string ViTri { get; set; }
        public string GhiChu { get; set; }
    }

}