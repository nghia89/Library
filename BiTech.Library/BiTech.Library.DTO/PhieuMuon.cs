using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiTech.Library.DTO
{
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


        #region Tai
        public  ETinhTrangPhieuMuon TrangThai { get; set; }
        public string TenTrangThai { get; set; }
        // Số ngày trễ hẹn trả hoặc số ngày gần trả
        public int?SoNgayGiaoDong { get; set; }
        public int STT { get; set; }
        public string IdGiaHan { get; set; }

        #endregion
    }
}
