using BiTech.Library.DTO;
using Mongo.Migration.Migrations;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiTech.Library.DAL.MongoMirgrations.ThongTinThuVien_Mirgrations.M000
{
    public class Init : Migration<ThongTinThuVien>
    {
        public Init()
            : base("0.0.1")
        {
        }

        public override void Up(BsonDocument document)
        {
        }

        public override void Down(BsonDocument document)
        {
        }
    }
}