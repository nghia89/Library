﻿using Mongo.Migration.Documents;
using Mongo.Migration.Documents.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace BiTech.Library.DTO
{
    [CurrentVersion("0.0.1")]
    public class ThongTinThuVien : IModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        // Thời gian đối tượng được tạo
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreateDateTime { get; set; }

        /// <summary>
        /// Tên thư viện
        /// </summary>
        public string Ten { get; set; }

        /// <summary>
        /// Địa chỉ thư viện
        /// </summary>
        public string DiaChi { get; set; }

        /// <summary>
        /// Số lần mượn tối đa
        /// </summary>
        public int SoLanMuonMax { get; set; }

        /// <summary>
        /// Phiên bản hiện tại của đối tượng
        /// </summary>
        public DocumentVersion Version { get; set; }
    }
}