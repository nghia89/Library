using System;
using BiTech.Library.DTO;
using Mongo.Migration.Migrations;
using MongoDB.Bson;

namespace BiTech.Library.DAL.MongoMirgrations.Sach_Mirgrations
{
    /// <summary>
    /// (version 0.0.2) thành (version 0.0.3)
    /// </summary>
    public class V0_0_3_thembo_BoSuuTap : Migration<BoSuuTap>
    {
        public V0_0_3_thembo_BoSuuTap()
            : base("0.0.3")
        {
        }

        public override void Up(BsonDocument document)
        {
            document.Add("Code", "");
            document.Add("Status", "");
  
        }

        public override void Down(BsonDocument document)
        {
            document.Remove("Code");
            document.Remove("Status");
        }
    }
}