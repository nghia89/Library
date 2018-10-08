using BiTech.Library.DTO;
using Mongo.Migration.Migrations;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiTech.Library.DAL.MongoMirgrations.ChiTietXuatSach_Mirgrations
{
    public class V_0_0_3_RemoveField : Migration<ChiTietXuatSach>
    {
        public V_0_0_3_RemoveField()
           : base("0.0.3")
        {
        }

        public override void Down(BsonDocument document)
        {
            document.Add("MaCaBiet", "");
        }

        public override void Up(BsonDocument document)
        {
            document.Remove("Soluong");
            document.Remove("GhiChu");
        }
    }
}
