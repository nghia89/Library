using BiTech.Library.Areas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BiTech.Library.Models
{
    public class SubDomainPacket
    {
        public List<WorkPlaceApiModel> ListPhng { get; set; } = new List<WorkPlaceApiModel>();
        public List<WorkPlaceApiModel> ListCap3 { get; set; } = new List<WorkPlaceApiModel>();
        public List<WorkPlaceApiModel> ListTTGD { get; set; } = new List<WorkPlaceApiModel>();
        public List<WorkPlaceApiModel> ListCap2 { get; set; } = new List<WorkPlaceApiModel>();
        public List<WorkPlaceApiModel> ListCap1 { get; set; } = new List<WorkPlaceApiModel>();
        public List<WorkPlaceApiModel> ListMNMG { get; set; } = new List<WorkPlaceApiModel>();
    }
}