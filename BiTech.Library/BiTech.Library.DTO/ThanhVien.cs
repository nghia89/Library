using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiTech.Library.DTO
{
    public class ThanhVien : IModel
    {        
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string UserName { get; set; }

        public string Password{ get; set; }

        public string MaSoThanhVien { get; set; }

        public string Ten { get; set; }

        public string CMND { get; set; }

        public string DiaChi { get; set; }

        public string SDT { get; set; }
        
        public EUser TrangThai { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreateDateTime { get; set; }
    }
}
