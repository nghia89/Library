/*
 Thinh
 
 */
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System;

namespace BiTech.Library.DTO
{
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

        [Required]
        [Display(Name = "Ngôn ngữ")]
        /// <summary>
        /// Tên ngôn ngữ này
        /// </summary>
        public string Ten { get; set; }

        [Required]
        [Display(Name = "Viêt tắt")]
        /// <summary>
        /// Tên ngắn VD: VN, EN, ...
        /// </summary>
        public string TenNgan { get; set; }
    }
}
