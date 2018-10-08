using BiTech.Library.DTO;
using Mongo.Migration.Migrations;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiTech.Library.DAL.MongoMirgrations.PhieuNhapSach_Mirgrations
{
    public class V0_0_3_RemoveFields : Migration<PhieuNhapSach>
    {
        public V0_0_3_RemoveFields()
            : base("0.0.3")
        {
        }

        public override void Up(BsonDocument document)
        {
            document.Remove("NgayNhap");
        }

        public override void Down(BsonDocument document)
        {
            document.Add("NgayNhap", new DateTime());
        }
    }
}