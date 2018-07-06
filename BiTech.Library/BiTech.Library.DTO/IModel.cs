using Mongo.Migration.Documents;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace BiTech.Library.DTO
{
    public interface IModel : IDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        string Id { get; set; } // Null when insert new

        // Thời gian đối tượng được tạo
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        DateTime CreateDateTime { get; set; }
    }
}
