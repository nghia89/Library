using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BiTech.Library.Models
{
    public class NhaXuatBanViewModels
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên nhà xuất bản")]
        public string Ten { get; set; }

        public string GhiChu { get; set; }
    }
}