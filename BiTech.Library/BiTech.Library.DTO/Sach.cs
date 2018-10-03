using Mongo.Migration.Documents;
using Mongo.Migration.Documents.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BiTech.Library.DTO
{
    [CurrentVersion("0.0.2")]
    public class Sach : IModel
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreateDateTime { get; set; }

        //[Required]
        //public string IdDauSach { get; set; }

        /// <summary>
        /// Mã do thư viện qui định
        /// </summary>
        public string MaKiemSoat { get; set; }

        [Required(ErrorMessage = "Vui lòng không để trống")]
        /// <summary>
        /// Tên của sách
        /// </summary>
        public string TenSach { get; set; }

        public string TenSachKhongDau { get; set; }

        
        /// <summary>
        /// Mã thể loại sách
        /// </summary>
        public string IdTheLoai { get; set; }

        /// <summary>
        /// Vị trí sách ở kệ nào
        /// </summary>
        public string IdKeSach { get; set; }

        [Required(ErrorMessage = "Vui lòng không để trống")]
        /// <summary>
        /// Mã nhà xuất bản
        /// </summary>
        public string IdNhaXuatBan { get; set; }

        /// <summary>
        /// Link ảnh bìa
        /// </summary>
        public string LinkBiaSach { get; set; }

        [Required(ErrorMessage = "Vui lòng không để trống")]
        /// <summary>
        /// Số trang
        /// </summary>
        public string SoTrang { get; set; }

        [Required(ErrorMessage = "Vui lòng không để trống")]
        /// <summary>
        /// Năm xuất bản
        /// </summary>
        public string NamXuatBan { get; set; }

        [Required(ErrorMessage = "Vui lòng không để trống")]
        /// <summary>
        /// Tóm tắt
        /// </summary>
        public string TomTat { get; set; } = "";

        /// <summary>
        /// Giá tiền của sách
        /// </summary>
        public string GiaBia { get; set; }

        [Required(ErrorMessage = "Vui lòng không để trống")]
        /// <summary>
        /// Phí mượn sách nếu có
        /// </summary>
        public string PhiMuonSach { get; set; }

        /// <summary>
        /// Số lượng tổng toàn bộ
        /// </summary>
        public int SoLuongTong { get; set; } = 0;

        /// <summary>
        /// Số lường còn lại trong kho
        /// </summary>
        public int SoLuongConLai { get; set; } = 0;

        /// <summary>
        /// Số lần mà cuốn sách được mượn
        /// </summary>
        public int SoLanDuocMuon { get; set; } = 0;

        [Required(ErrorMessage = "Vui lòng không để trống")]
        public bool CongKhai { get; set; } = true;

        public string ISBN { get; set; } = "";

        [Required(ErrorMessage = "Vui lòng không để trống")]
        /// <summary>
        /// Mã ngôn ngữ
        /// </summary>
        public string IdNgonNgu { get; set; }

        /// <summary>
        /// Mã DDC
        /// </summary>
        public string DDC { get; set; }

        public string MARC21 { get; set; }

        public string XuatXu { get; set; }

        public string TaiBan { get; set; }

        public string NguoiBienDich { get; set; }

        public string QRlink { get; set; }

        public string QRData { get; set; }

        public bool IsDeleted { get; set; } = false;


        /// <summary>
        /// Phiên bản hiện tại của đối tượng
        /// </summary>
        public DocumentVersion Version { get; set; }

        #region Tai
        [BsonIgnore]
        public List<TacGia> listTacGia { get; set; } = new List<TacGia>();
        [BsonIgnore]
        public List<string> ListError { get; set; } = new List<string>();
        [BsonIgnore]
        public string Error { get; set; }
        #endregion
    }
}
