using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace BiTech.Library.DTO
{
    public class Sach : IModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        // Thời gian đối tượng được tạo
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreateDateTime { get; set; }

        public string IdDauSach { get; set; }

        /// <summary>
        /// Mã do thư viện qui định
        /// </summary>
        public string MaKiemSoat { get; set; }

        /// <summary>
        /// Tên của sách
        /// </summary>
        public string TenSach { get; set; }

        /// <summary>
        /// Mã thể loại sách
        /// </summary>
        public string IdTheLoai { get; set; }

        /// <summary>
        /// Vị trí sách ở kệ nào
        /// </summary>
        public string IdKeSach { get; set; }

        /// <summary>
        /// Mã nhà xuất bản
        /// </summary>
        public string IdNhaXuatBan { get; set; }

        /// <summary>
        /// Link ảnh bìa
        /// </summary>
        public string LinkBiaSach { get; set; }

        /// <summary>
        /// Số trang
        /// </summary>
        public int SoTrang { get; set; }

        /// <summary>
        /// Năm xuất bản
        /// </summary>
        public string NamSanXuat { get; set; }

        /// <summary>
        /// Tóm tắt
        /// </summary>
        public string TomTat { get; set; }

        /// <summary>
        /// Giá tiền của sách
        /// </summary>
        public double GiaSach { get; set; }

        /// <summary>
        /// Phí mượn sách nếu có
        /// </summary>
        public double PhiMuonSach { get; set; }

        /// <summary>
        /// Số lượng tổng toàn bộ
        /// </summary>
        public int SoLuongTong { get; set; }

        /// <summary>
        /// Số lường còn lại trong kho
        /// </summary>
        public int SoLuongConLai { get; set; }

        public int SoLanDuocMuon { get; set; }

        /// <summary>
        /// Mã ngôn ngữ
        /// </summary>
        public string NgonNgu { get; set; }
    }
}
