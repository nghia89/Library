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
        [Required]
        public string TenTacGia { get; set; }
        [Required]
        public string MoTa { get; set; }
        [Required]
        public string QuocTich { get; set; }
       
    }
}