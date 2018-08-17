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
        public string NgayGioMuon { get; set; }
        public string NgayPhaiTra { get; set; }
        public string NgayTraThucTe { get; set; }
        public bool DaTra { get; set; }
        public string TrangThaiTra { get; set; }

        /// <summary>
        /// Phiên bản hiện tại của đối tượng
        /// </summary>
        public DocumentVersion Version { get; set; }
    }
}
