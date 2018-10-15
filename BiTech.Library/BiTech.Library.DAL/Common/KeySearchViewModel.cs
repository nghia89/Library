using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BiTech.Library.Common
{
    public class KeySearchViewModel
    {

        public string Keyword { get; set; }
        public string Keyword1 { get; set; }
        public string Keyword2 { get; set; }
        public string Keyword3 { get; set; }
        public string Keyword4 { get; set; }

        public string ddlLoaiTimKiem0 { get; set; }
        public string ddlLoaiTimKiem1 { get; set; }
        public string ddlLoaiTimKiem2 { get; set; }
        public string ddlLoaiTimKiem3 { get; set; }
        public string ddlLoaiTimKiem4 { get; set; }

        public string TheLoaiSach { get; set; }
        public string TenTacGia { get; set; }
        public string TenNXB { get; set; }
        public string KeSach { get; set; }
        public string BoSuuTap { get; set; }
        public string KeywordBasic { get; set; }
        public string FrmSearchBasic { get; set; }
        public string FrmSearchHigh { get; set; }

        public string Condition { get; set; }
        public string Condition1 { get; set; }
        public string Condition2 { get; set; }
        public string Condition3 { get; set; }
        public string Condition4 { get; set; }

        public string dlOperator1 { get; set; }
        public string dlOperator2 { get; set; }
        public string dlOperator3 { get; set; }
        public string dlOperator4 { get; set; }

        public string SapXep { get; set; }

        // dùng cho lấy sách theo id tác giả và chuyển về regex dạng OR -> không hiện lên view
        public List<string> ListSachIds { get; set; }
        public List<string> ListSachIds1 { get; set; }
        public List<string> ListSachIds2 { get; set; }
        public List<string> ListSachIds3 { get; set; }
        public List<string> ListSachIds4 { get; set; }


        public List<string> ListIdNXB { get; set; }
        public List<string> ListIdNXB1 { get; set; }
        public List<string> ListIdNXB2 { get; set; }
        public List<string> ListIdNXB3 { get; set; }
        public List<string> ListIdNXB4 { get; set; }

        public List<string> ListLanguage { get; set; }
        public List<string> ListLanguage1 { get; set; }
        public List<string> ListLanguage2 { get; set; }
        public List<string> ListLanguage3 { get; set; }
        public List<string> ListLanguage4 { get; set; }
    }
}