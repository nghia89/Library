using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BiTech.Library.Models
{
    public class LanguageViewModels
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên ngôn ngữ")]
        [Display(Name = "Ngôn ngữ")]
        public string Ten { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên tên ngắn")]
        [Display(Name = "Tên ngắn")]
        public string TenNgan { get; set; }
    }
}