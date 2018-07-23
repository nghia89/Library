﻿using Mongo.Migration.Documents;
using Mongo.Migration.Documents.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiTech.Library.DTO
{
    [CurrentVersion("0.0.1")]
    public class ThanhVien : IModel
    {        
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string UserName { get; set; }

        public string Password{ get; set; }

        public string MaSoThanhVien { get; set; }

        public string Ten { get; set; }
      
        public string DiaChi { get; set; }

        public string SDT { get; set; }

        public string HinhChanDung { get; set; }
  
        public EUser TrangThai { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreateDateTime { get; set; }

        /// <summary>
        /// Phiên bản hiện tại của đối tượng
        /// </summary>
        public DocumentVersion Version { get; set; }

        #region Tai
        public string GioiTinh { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime NgaySinh { get; set; }
        public string LopHoc { get; set; }
        public string ChucVu { get; set; }
        #endregion
    }
}
