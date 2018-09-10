using BiTech.Library.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
                    Value = (index + 1).ToString(),
                    Text = monthName
                });
            }
        }

        public List<ThongTinMuonSach> ListPhieuMuon { get; set; }
        public IEnumerable<ChiTietPhieuMuon> ListCTMP { get; set; }
        public IEnumerable<Sach> ListSach { get; set; }
        public List<List<Sach>> ListChaCuaSach { get; set; }
        public List<int> ListSoLuong { get; set; }
        public List<Sach> ListSach1 { get; set; }


        [Display(Name = "Tên đọc giả")]
        public string TenDocGia { get; set; }

        [Display(Name = "Địa chỉ")]
        public string DiaChi { get; set; }
        [Display(Name = "Số điện thoại")]
        public string SDT { get; set; }
        [Display(Name = "Hình")]
        public string HinhChanDung { get; set; }
        [Display(Name = "Hình độc giả")]
        public string LinkAvatar { get; set; }
        [Display(Name = "Trạng thái")]
        public EUser TrangThaiUser { get; set; }

        [Display(Name = "Lớp học")]
        public string LopHoc { get; set; }
        [Display(Name = "Niên khóa")]
        public string NienKhoa { get; set; }
        [Display(Name = "Mã QR")]
        public string QRLink { get; set; }
        [Display(Name = "Ngày sinh")]
        [DataType(DataType.Date)]
        public DateTime NgaySinh { get; set; }
        [Display(Name = "Giới tính")]
        public string GioiTinh { get; set; }
        public string idUser { get; set; }
    }
    public class BieuDoPhieuMuonViewModel
    {
        // List chứa dữ liệu thống kê trong 12 tháng
        public int[] lsoPhieuMuonTrongNam { get; set; }
        public int[] lsoNguoiMuonSachTrongNam { get; set; }
        public int[] lsoSachDuocMuonTrongNam { get; set; }
        public int[] lsoNguoiKhongTraTrongNam { get; set; }
        public int[] lsoNguoiTraTreTrongNam { get; set; }
        public int[] lsoSachDuocTraTrongNam { get; set; }
        public int[] lsoSachKhongTraTrongNam { get; set; }
        public int[] soNguoiTraSachTrongNam { get; set; }
        // List chứa dữ liệu thống kê trong 4 quý
        public int[] lsoPhieuMuonTrongQuy { get; set; }
        public int[] lsoNguoiMuonSachTrongQuy { get; set; }
        public int[] lsoSachDuocMuonTrongQuy { get; set; }
        public int[] lsoNguoiKhongTraTrongQuy { get; set; }
        public int[] lsoNguoiTraTreTrongQuy { get; set; }
        public int[] lsoSachDuocTraTrongQuy { get; set; }
        public int[] lsoSachKhongTraTrongQuy { get; set; }
        // List chứa dữ liệu thống kê trong 31 ngày
        public int SoNgayTrongThang { get; set; }
        public int[] lsoNgayTrongThang { get; set; }
        public int[] lsoPMTrongNgay { get; set; }
        public int[] lsoNguoiMuonTrongNgay { get; set; }
        public int[] lsoSachDuocMuonTrongNgay { get; set; }
        public int[] lsoNguoiKhongTraTrongNgay { get; set; }
        public int[] lsoNguoiTraTreTrongNgay { get; set; }
        public int[] lsoSachDuocTraTrongNgay { get; set; }
        public int[] lsoSachKhongTraTrongNgay { get; set; }
        public int[] lsoNguoiTraTrongNgay { get; set; }
        // List thống kê theo tuần   
        public List<int[]> thongKeTheoTuan { get; set; }
        public List<string> ListNgayTrongTuan { get; set; }
        public int[] lsoNguoiMuonTrongTuan { get; set; }
        public int[] lsoSachDuocMuonTrongTuan { get; set; }
        public int[] lsoSachDuocTraTrongTuan { get; set; }
        public int[] lsoSachKhongTraTrongTuan { get; set; }
    }



}