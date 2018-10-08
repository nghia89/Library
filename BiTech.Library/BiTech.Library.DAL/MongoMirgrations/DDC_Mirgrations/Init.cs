using BiTech.Library.DTO;
using Mongo.Migration.Migrations;
using MongoDB.Bson;

namespace BiTech.Library.DAL.MongoMirgrations.DDC_Mirgrations
{
    class Init : Migration<DDC>
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
