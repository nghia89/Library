using BiTech.Library.DTO;
using Mongo.Migration.Migrations;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiTech.Library.DAL.MongoMirgrations.PhieuXuatSach_Mirgrations.M001
{
    public class V0_0_2_AddUserName : Migration<PhieuXuatSach>
    {
        public V0_0_2_AddUserName()
            : base("0.0.2")
        {
        }

        public override void Up(BsonDocument document)
        {
            document.Add("UserName", "");
        }

        public override void Down(BsonDocument document)
        {
            document.Remove("UserName");
        }
    }
}