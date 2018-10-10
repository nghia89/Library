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
    public class ThongTinMuonSach : IModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreateDateTime { get; set; }

        public string idUser { get; set; }

        public string idSach { get; set; }
        public string IdSachCaBiet { get; set; }
        //[BsonDateTimeOptions(Kind = DateTimeKind.Local, Representation = BsonType.Document)]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime NgayGioMuon { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime NgayPhaiTra { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime NgayTraThucTe { get; set; }

        public bool DaTra { get; set; }

        public string TrangThaiTra { get; set; }

        /// <summary>
        /// Phiên bản hiện tại của đối tượng
        /// </summary>
        public DocumentVersion Version { get; set; }

        [BsonIgnore]
        public ETinhTrangPhieuMuon TrangThai { get; set; }
        [BsonIgnore]
        public string TrangThaiString { get; set; }

        [BsonIgnore]
        public string TenTrangThai { get; set; }

        [BsonIgnore]
        public int SoSachTong { get; set; } = 1;

        [BsonIgnore]
        // Số ngày trễ hẹn trả hoặc số ngày gần trả
        public int? SoNgayGiaoDong { get; set; }
        [BsonIgnore]
        public string NgayMuonTemp { get; set; }
        [BsonIgnore]
        public string NgayTraTemp { get; set; }
        [BsonIgnore]
        public string NgayTraThucTeTemp { get; set; }
        [BsonIgnore]
        public List<string> ListSach { get; set; } = new List<string>();
        [BsonIgnore]
        public string TenThanhVien { get; set; }     
        [BsonIgnore]
        public string TenSach { get; set; }
        [BsonIgnore]
        public string STT { get; set; }
        [BsonIgnore]
        public string SoSachDuocMuon { get; set; }
        [BsonIgnore]
        public string SoNguoiMuonSach { get; set; }


    }
}
