using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BiTech.Library.Common
{
    public class KeySearchViewModel
    {
        public String Keyword { get; set; }
        public String TheLoaiSach { get; set; }
        public String TenTacGia { get; set; }
        public String TenNXB { get; set; }

        // dùng cho lấy sách theo id tác giả và chuyển về regex dạng OR -> không hiện lên view
        public List<string> ListSachIds { get; set; }
    }
}