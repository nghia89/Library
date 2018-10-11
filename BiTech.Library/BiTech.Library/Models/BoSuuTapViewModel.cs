using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BiTech.Library.Models
{
    public class BoSuuTapViewModel
    {
        public string Id { get; set; }

        public DateTime CreateDateTime { get; set; }

        [Required(ErrorMessage = "Vui lòng không để trống")]
        public string Name { get; set; }

        public string Code { get; set; }
        public bool Status { get; set; }
    }
}