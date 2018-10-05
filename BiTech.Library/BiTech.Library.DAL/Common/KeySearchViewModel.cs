using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BiTech.Library.Common
{
    public class KeySearchViewModel
    {
        public String Keyword { get; set; }
        public String Keyword1 { get; set; }
        public String Keyword2 { get; set; }
        public String Keyword3 { get; set; }
        public String Keyword4 { get; set; }

        public string ddlLoaiTimKiem0 { get; set; }
        public string ddlLoaiTimKiem1 { get; set; }
        public string ddlLoaiTimKiem2 { get; set; }
        public string ddlLoaiTimKiem3 { get; set; }
        public string ddlLoaiTimKiem4 { get; set; }

        public String TheLoaiSach { get; set; }
        public String TenTacGia { get; set; }
        public String TenNXB { get; set; }
        public String KeSach { get; set; }
        public String KeywordBasic { get; set; }
        public String FrmSearchBasic { get; set; }
        public String FrmSearchHigh { get; set; }

        public string SapXep { get; set; }

        // dùng cho lấy sách theo id tác giả và chuyển về regex dạng OR -> không hiện lên view
        public List<string> ListSachIds { get; set; }

        public List<string> ListIdNXB { get; set; }
    }
}