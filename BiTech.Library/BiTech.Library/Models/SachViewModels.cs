using BiTech.Library.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BiTech.Library.Models
{
    public class SachUploadModel 
    {
        public Sach SachDTO { get; set; }

        public HttpPostedFileBase FileImageCover { get; set; }
        
        public List<Language> Languages { get; set; }
        
        [Required(ErrorMessage ="Vui lòng không để trống")]
        public List<string> ListTacGiaJson { get; set; }

        public SachUploadModel()
        {

        }

        public SachUploadModel(Sach sach)
        {
            SachDTO = sach;
        }
    }

    public class SachViewModels
    {
        public string Id { get; set; }

        [Required]
        public string TenSach { get; set; }
        
        [Required]
        public string IdTheLoai { get; set; }

        [Required]
        public string IdKeSach { get; set; }

        [Required]
        public string IdNhaXuatBan { get; set; }

        public string IdTrangThai { get; set; }

        [Required]
        public string MaKiemSoat { get; set; }

        public string Hinh { get; set; }

        [Required]
        public int SoLuong { get; set; }

        [Required]
        public int SoTrang { get; set; }

        [Required]
        public string NgonNgu { get; set; }

        public string NamSanXuat { get; set; }

        [Required]
        public string GiaBia { get; set; }

        public string LinkBiaSach { get; set; }

        [Required]
        public string TomTat { get; set; }

        public string DDC { get; set; }

        public string MARC21 { get; set; }

        public string XuatXu { get; set; }

        public string TaiBan { get; set; }

        public string NguoiBienDich { get; set; }

        public string QRlink { get; set; }

        public string QRData { get; set; }

        public string ISBN { get; set; } = "";

        [Required]
        public HttpPostedFileBase FileImageCover { get; set; }
        public HttpPostedFileBase LinkExcel { get; set; }
    }

    public class ListBooksModel
    {
        public List<BookView> Books { get; set; } = new List<BookView>();
    }

    public class BookView
    {
        public Sach SachDTO { get; set; }

        public string Ten_KeSach { get; set; }
        
        public string Ten_TheLoai { get; set; }
        
        public string Ten_NhaXuatBan { get; set; }
        
        public string Ten_NgonNgu { get; set; }

        public string Ten_TacGia { get; set; }
        public BookView()
        {

        }

        public BookView(Sach sach)
        {
            SachDTO = sach;
        }

    
    }
}