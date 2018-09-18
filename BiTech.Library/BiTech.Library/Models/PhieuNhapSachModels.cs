using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using BiTech.Library.DTO;
using System.Collections;

namespace BiTech.Library.Models
{
    public class PhieuNhapSachModels
    {
        public string IdPhieuNhap { get; set; }
        [Display(Name = "Ghi chú phiếu nhập sách")]
        [Required(ErrorMessage = "Vui lòng nhập ghi chú")]
        public string GhiChu { get; set; } = "";

        public List<string> listChiTietJsonString { get; set; } = new List<string>();

        public DateTime NgayNhap { get; set; }

        public string IdUserAdmin { get; set; }
        public string UserName { get; set; }
        public HttpPostedFileBase LinkExcel { get; set; }
    }

    public class ChiTietNhapSachViewModels
    {
        public string Id { get; set; }

        public string IdPhieuNhap { get; set; }

        public string IdSach { get; set; }

        public string MaKiemSoat { get; set; }

        public string IdTinhTrang { get; set; }

        public string tenTinhTrang { get; set; }
        [Display(Name = "Số Lượng")]
        [Required(ErrorMessage = "Vui lòng nhập số lượng")]
        [Range(0, 999, ErrorMessage = "Số nhập vào phải lớn hơn 0")]
        public int soLuong { get; set; }
        public string GhiChuDon { get; set; }
        public string ten { get; set; }
    }

    public class ImportExcelPNSViewModel
    {
        [Display(Name = "Ghi chú phiếu nhập sách")]
        [Required(ErrorMessage = "Vui lòng nhập ghi chú")]
        public string GhiChu { get; set; } = "";
        public List<string[]> RawDataList { get; set; }
        public int TotalEntry { get; set; }
        public List<ChiTietNhapSach> ListSuccess { get; set; } = new List<ChiTietNhapSach>();
        public List<ChiTietNhapSach> ListFail { get; set; } = new List<ChiTietNhapSach>();
        public List<ArrayList> ListShow { get; set; } = new List<ArrayList>();
        public string FileName { get; set; }
        public string FilePath { get; set; }
        /// <summary>
        /// Mảng chứa các dòng chứ Mã sách bị trùng
        /// </summary>
        public bool[] ArrRows { get; set; }
    }
}