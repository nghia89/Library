﻿using Mongo.Migration.Documents;
using Mongo.Migration.Documents.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace BiTech.Library.DTO
{
    [CurrentVersion("0.0.2")]
    public class TheLoaiSach : IModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string IdParent { get; set; }

        public string TenTheLoai { get; set; }
        
        public string MoTa { get; set; }

        public string MaDDC { get; set; }

        // Thời gian đối tượng được tạo
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreateDateTime { get; set; }
        
        public DocumentVersion Version { get; set; }
    }
}
