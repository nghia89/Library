using System;
using BiTech.Library.DTO;
using Mongo.Migration.Migrations;
using MongoDB.Bson;

namespace BiTech.Library.DAL.MongoMirgrations.Sach_Mirgrations.M003
{
    /// <summary>
    /// (version 0.0.2) thành (version 0.0.3)
    /// </summary>
    public class V0_0_3_thembo_truong : Migration<Sach>
    {
        public V0_0_3_thembo_truong()
            : base("0.0.3")
        {
        }

        public override void Up(BsonDocument document)
        {
            document.Add("IdBoSuuTap", "");
            document.Add("TaiLieuDinhKem", "");
            document.Add("ISSN", "");
            document.Add("LLC", "");
        }

        public override void Down(BsonDocument document)
        {
            document.Remove("IdBoSuuTap");
            document.Remove("TaiLieuDinhKem");
            document.Remove("ISSN");
            document.Remove("LLC");
        }
    }
}