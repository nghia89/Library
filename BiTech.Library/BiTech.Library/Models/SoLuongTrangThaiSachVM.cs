using BiTech.Library.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BiTech.Library.Models
{
    public class SoLuongTrangThaiSachVM
    {
        public string Id { get; set; }

        public string IdSach { get; set; }

        public string TrangThai { get; set; }

        public string IdTrangThai { get; set; }

		//[RegularExpression("(^[0-9])", ErrorMessage = "Count must be a natural number")]
		public int SoLuong { get; set; }
		
    }
}