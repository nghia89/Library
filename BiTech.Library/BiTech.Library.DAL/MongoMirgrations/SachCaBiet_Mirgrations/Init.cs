using BiTech.Library.DTO;
using Mongo.Migration.Migrations;
using MongoDB.Bson;

namespace BiTech.Library.DAL.MongoMirgrations.SachCaBiet_Mirgrations
{
    public class Init : Migration<SachCaBiet>
    {
        public Init()
           : base("0.0.1")
        {
        }
        public override void Down(BsonDocument document)
        {
        }

        public override void Up(BsonDocument document)
        {
        }
    }
}
