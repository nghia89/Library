using BiTech.Library.DTO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BiTech.Library.Models
{
    public class ThongKeViewModel
    {

        public string Id { get; set; }
        public List<ThanhVien> TenNguoiMuon { get; set; }
        public List<Sach> Sach { get; set; }
        public string TenSach { get; set; }
        public string NgayMuon { get; set; }
        public string NgayPhaiTra { get; set; }
        public string TinhTrang { get; set; }
        public string GiaHan { get; set; }
        public string GhiChu { get; set; }

        public IEnumerable<SelectListItem> Months
        {
            get
            {
                return DateTimeFormatInfo.InvariantInfo.MonthNames.Select((monthName, index) => new SelectListItem
                {
                    Value=(index+1).ToString(),
                    Text=monthName
                });
            }
        }
    }

}