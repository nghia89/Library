using Mongo.Migration.Documents;
using Mongo.Migration.Documents.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace BiTech.Library.DTO
{
    [CurrentVersion("0.0.2")]
    public class SachCaBiet : IModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        // Thời gian đối tượng được tạo
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreateDateTime { get; set; }

        public DocumentVersion Version { get; set; }

        public string IdSach { get; set; }

        public string IdTrangThai { get; set; }

        public string MaKSCB { get; set; }

        public string MaCaBienCu { get; set; }

        public string QRlink { get; set; }
		
        public string QRData { get; set; }
        
        public string TenSach { get; set; }
    }
}
