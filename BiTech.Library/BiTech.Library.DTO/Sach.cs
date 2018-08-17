﻿using Mongo.Migration.Documents;
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

        [Required]
        /// <summary>
        /// Tên của sách
        /// </summary>
        public string TenSach { get; set; }

        [Required]
        /// <summary>
        /// Mã thể loại sách
        /// </summary>
        public string IdTheLoai { get; set; }

        [Required]
        /// <summary>
        /// Vị trí sách ở kệ nào
        /// </summary>
        public string IdKeSach { get; set; }

        [Required]
        /// <summary>
        /// Mã nhà xuất bản
        /// </summary>
        public string IdNhaXuatBan { get; set; }

        /// <summary>
        /// Link ảnh bìa
        /// </summary>
        public string LinkBiaSach { get; set; }

        [Required]
        /// <summary>
        /// Số trang
        /// </summary>
        public string SoTrang { get; set; }

        [Required]
        /// <summary>
        /// Năm xuất bản
        /// </summary>
        public string NamXuatBan { get; set; }

        /// <summary>
        /// Tóm tắt
        /// </summary>
        public string TomTat { get; set; } = "";

        /// <summary>
        /// Giá tiền của sách
        /// </summary>
        public string GiaBia { get; set; }

        /// <summary>
        /// Phí mượn sách nếu có
        /// </summary>
        public double PhiMuonSach { get; set; } = 0;

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

        [Required]
        public bool CongKhai { get; set; } = true;

        public string ISBN { get; set; } = "";

        [Required]
        /// <summary>
        /// Mã ngôn ngữ
        /// </summary>
        public string IdNgonNgu { get; set; }

        /// <summary>
        /// Đã được xóa chưa
        /// </summary>
        public string DDC { get; set; }

        public string MARC21 { get; set; }

        public string XuatXu { get; set; }

        public string TaiBan { get; set; }

        public string NguoiBienDich { get; set; }

        public string QRlink { get; set; }

        public string QRData { get; set; }
        public bool IsDeleted { get; set; } = true;

        /// <summary>
        /// Phiên bản hiện tại của đối tượng
        /// </summary>
        public DocumentVersion Version { get; set; }
        #region Tai
        [BsonIgnore]
        public List<TacGia> listTacGia { get; set; }
        #endregion
    }
}
