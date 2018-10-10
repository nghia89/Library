using Mongo.Migration.Documents;
using Mongo.Migration.Documents.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BiTech.Library.DTO
{
    [CurrentVersion("0.0.1")]
    public class BoSuuTap : IModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreateDateTime { get; set; }
        
        public DocumentVersion Version { get; set; }

        [Required(ErrorMessage = "Vui lòng không để trống")]
        public string Name { get; set; }

        public string Code { get; set; }
        public bool Status { get; set; }
    }
}
