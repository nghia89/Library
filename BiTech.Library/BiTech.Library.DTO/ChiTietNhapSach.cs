using Mongo.Migration.Documents;
using Mongo.Migration.Documents.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace BiTech.Library.DTO
{
    [CurrentVersion("0.0.2")]
    public class ChiTietNhapSach : IModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreateDateTime { get; set; }

        public string IdPhieuNhap { get; set; }

        public string IdSach { get; set; }

        public string IdTinhtrang { get; set; }

        public int SoLuong { get; set; }
        //public string tenTinhTrang { get; set; }
        public string GhiChu { get; set; }

        /// <summary>
        /// Phiên bản hiện tại của đối tượng
        /// </summary>
        public DocumentVersion Version { get; set; }
        #region Tai
        [BsonIgnore]
        public int RowExcel { get; set; }
        [BsonIgnore]
        public List<string> ListError { get; set; } = new List<string>();
        [BsonIgnore]
        public bool IsExist { get; set; } = true;
        #endregion
    }
}
