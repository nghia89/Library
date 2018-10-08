using BiTech.Library.DTO;
using Mongo.Migration.Migrations;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiTech.Library.DAL.MongoMirgrations.Sach_Mirgrations
{
    /// <summary>
    /// Đổi tên NamSanXuat (version 0.0.0) thành NamXuatBan (version 0.0.1)
    /// </summary>
    public class V0_0_1_Rename_NamSanXuat_NamXuatBan : Migration<Sach>
    {
        public V0_0_1_Rename_NamSanXuat_NamXuatBan()
            : base("0.0.1")
        {
        }

        public override void Up(BsonDocument document)
        {
            var doors = document["NamSanXuat"].ToString();
            document.Add("NamXuatBan", doors);
            document.Remove("NamSanXuat");
        }

        public override void Down(BsonDocument document)
        {
            var doors = document["NamXuatBan"].ToString();
            document.Add("NamSanXuat", doors);
            document.Remove("NamXuatBan");
        }
    }
}