using BiTech.Library.DTO;
using Mongo.Migration.Migrations;
using MongoDB.Bson;

<<<<<<< HEAD:BiTech.Library/BiTech.Library.DAL/MongoMirgrations/BoSuutap_Mirgrations/Init.cs
namespace BiTech.Library.DAL.MongoMirgrations.BoSuutap_Mirgrations
=======
namespace BiTech.Library.DAL.MongoMirgrations.BoSuuTap_Mirgrations
>>>>>>> 0a36e7834924fb21471f885876dce3c92bcd0259:BiTech.Library/BiTech.Library.DAL/MongoMirgrations/BoSuutap_Mirgrations/Init.cs
{
    public class Init : Migration<BoSuuTap>
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
