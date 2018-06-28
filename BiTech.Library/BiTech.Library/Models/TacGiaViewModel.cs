using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BiTech.Library.Models
{
    public class TacGiaViewModel
    {
        public string Id { get; set; }
        [Display(Name ="Tên Tác Giả")]
        [Required(ErrorMessage ="Vui lòng nhập tên tác giả")]
        public string TenTacGia { get; set; }
        [Display(Name = "Mô Tả")]
        [Required(ErrorMessage = "Vui lòng nhập mô tả")]
        public string MoTa { get; set; }
        [Display(Name = "Quốc Tịch")]
        [Required(ErrorMessage="Vui lòng nhập quốc tịch")]
        public string QuocTich { get; set; }
       
    }
}