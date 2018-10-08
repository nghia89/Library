using BiTech.Library.DTO;
using Mongo.Migration.Migrations;
using MongoDB.Bson;

namespace BiTech.Library.DAL.MongoMirgrations.GiaHan_Mirgrations
{
    public class Init : Migration<GiaHan>
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