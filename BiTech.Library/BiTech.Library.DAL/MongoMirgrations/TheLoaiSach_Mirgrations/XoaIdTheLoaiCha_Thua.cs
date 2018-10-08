using BiTech.Library.DTO;
using Mongo.Migration.Migrations;
using MongoDB.Bson;


namespace BiTech.Library.DAL.MongoMirgrations.TheLoaiSach_Mirgrations
{
    public class XoaIdTheLoaiCha_Thua : Migration<TheLoaiSach>
    {
        public XoaIdTheLoaiCha_Thua() : base("0.0.2")
        {
        }

        public override void Up(BsonDocument document)
        {
            document.Remove("IdTheLoaiCha");
        }

        public override void Down(BsonDocument document)
        {
            document.Add("IdTheLoaiCha", "");
        }
    }
}