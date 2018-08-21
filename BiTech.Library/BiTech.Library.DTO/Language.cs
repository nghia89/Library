/*
 Thinh
 
 */
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System;
using Mongo.Migration.Documents;
using Mongo.Migration.Documents.Attributes;

namespace BiTech.Library.DTO
{
    [CurrentVersion("0.0.1")]
    /// <summary>
    /// Ngôn Ngữ
    /// </summary>
    public class Language : IModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreateDateTime { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên ngôn ngữ")]
        [Display(Name = "Ngôn ngữ")]
        /// <summary>
        /// Tên ngôn ngữ này
        /// </summary>
        public string Ten { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên tên ngắn")]
        [Display(Name = "Tên ngắn")]
        /// <summary>
        /// Tên ngắn VD: VN, EN, ...
        /// </summary>
        public string TenNgan { get; set; }

        /// <summary>
        /// Phiên bản hiện tại của đối tượng
        /// </summary>
        public DocumentVersion Version { get; set; }
    }
}
