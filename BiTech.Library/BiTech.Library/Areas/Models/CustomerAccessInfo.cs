using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BiTech.Library.Areas.Models
{
    public class CustomerAccessInfo
    {
        public string IdWorkplace { get; set; }

        public string WebSubDomain { get; set; }

        public string DataBaseName { get; set; }

        // Kích hoạt có thời hạn ?
        public bool IsActivePeriod { get; set; }

        // Ngày kết thúc dịch vụ
        public DateTime EndDate { get; set; }

        public string ConfirmKey { get; set; }
    }
}