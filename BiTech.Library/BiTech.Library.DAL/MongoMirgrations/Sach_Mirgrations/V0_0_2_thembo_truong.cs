using System;
using BiTech.Library.DTO;
using Mongo.Migration.Migrations;
using MongoDB.Bson;

namespace BiTech.Library.DAL.MongoMirgrations.Sach_Mirgrations
{
    /// <summary>
    /// (version 0.0.1) thành (version 0.0.2)
    /// remove IdDauSach
    /// add CongKhai & ISBN
    /// </summary>
    public class V0_0_2_thembo_truong : Migration<Sach>
    {
        public V0_0_2_thembo_truong()
            : base("0.0.2")
        {
        }

        public override void Up(BsonDocument document)
        {
            document.Remove("IdDauSach");
            document.Add("CongKhai", true);
            document.Add("ISBN", "");
        }

        public override void Down(BsonDocument document)
        {
            document.Add("IdDauSach","");
            document.Remove("CongKhai");
            document.Remove("ISBN");
        }
    }
}