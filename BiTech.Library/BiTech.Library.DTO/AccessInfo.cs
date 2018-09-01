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
    public class AccessInfo : IModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreateDateTime { get; set; }

        public DocumentVersion Version { get; set; }

        public string IdWorkplace { get; set; }

        public string WebSubDomain { get; set; }

        public string DataBaseName { get; set; }

        public int AccessLevel { get; set; }
        
        // Kích hoạt có thời hạn ?
        public bool IsActivePeriod { get; set; }
        
        // Ngày kết thúc dịch vụ
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime EndDate { get; set; }
    }
}
