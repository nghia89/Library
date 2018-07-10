using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Mongo.Migration.Documents;
using Mongo.Migration.Documents.Attributes;

namespace BiTech.Library.DTO
{
    [CurrentVersion("0.0.1")]
    public class PhieuXuatSach : IModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreateDateTime { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime NgayXuat { get; set; }

        public string IdUserAdmin { get; set; }

        public string UserName { get; set; }

        public string GhiChu { get; set; }

        /// <summary>
        /// Phiên bản hiện tại của đối tượng
        /// </summary>
        public DocumentVersion Version { get; set; }
    }
}
