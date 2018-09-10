using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BiTech.Library.Areas.Models
{
    public class WorkPlaceApiModel
    {
        public string WpId { get; set; }
        public string Name { get; set; }
        public string Site { get; set; }
        public ECustomerWorkplaceType Type { get; set; }
    }
}