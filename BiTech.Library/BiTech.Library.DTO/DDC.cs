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
    public class DDC : IModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreateDateTime { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập mã DDC")]
        public string MaDDC { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tên DDC")]
        public string Ten { get; set; }

        /// <summary>
        /// Phiên bản hiện tại của đối tượng
        /// </summary>
        public DocumentVersion Version { get; set; }
    }
}
