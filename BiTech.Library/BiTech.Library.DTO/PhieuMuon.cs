using Mongo.Migration.Documents;
using Mongo.Migration.Documents.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiTech.Library.DTO
{
    [CurrentVersion("0.0.1")]
    public class PhieuMuon : IModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string IdUser { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime NgayMuon { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime NgayPhaiTra { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? NgayTra { get; set; }
        
        public EPhieuMuon TrangThaiPhieu { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreateDateTime { get; set; }

        public string GiaHan { get; set; }

        public string GhiChu { get; set; }
        
        /// <summary>
        /// Phiên bản hiện tại của đối tượng
        /// </summary>
        public DocumentVersion Version { get; set; }

        #region Tai - NoDB

        [BsonIgnore]
        public  ETinhTrangPhieuMuon TrangThai { get; set; }

        [BsonIgnore]
        public string TenTrangThai { get; set; }

        [BsonIgnore]
        // Số ngày trễ hẹn trả hoặc số ngày gần trả
        public int?SoNgayGiaoDong { get; set; }

        [BsonIgnore]
        public int STT { get; set; }

        [BsonIgnore]
        public string IdGiaHan { get; set; }

        #endregion
    }
}
