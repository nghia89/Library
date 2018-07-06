using Mongo.Migration.Documents;
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
    public class SoLuongSachTrangThai : IModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        // Thời gian đối tượng được tạo
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreateDateTime { get; set; }

        public string IdSach { get; set; }

        public string IdTrangThai { get; set; }

        public int SoLuong { get; set; }
        
        /// <summary>
        /// Phiên bản hiện tại của đối tượng
        /// </summary>
        public DocumentVersion Version { get; set; }
    }
}
