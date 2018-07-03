﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiTech.Library.DTO
{
    public class ChiTietXuatSach : IModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreateDateTime { get; set; }

        public string IdPhieuNhap { get; set; }

        public string IdSach { get; set; }

        public string IdTinhTrang { get; set; }

        public string IdLyDo { get; set; }

        public int SoLuong { get; set; }
    }
}
