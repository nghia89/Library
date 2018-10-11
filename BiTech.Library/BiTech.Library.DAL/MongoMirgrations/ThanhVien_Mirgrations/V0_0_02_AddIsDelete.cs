using BiTech.Library.DTO;
using Mongo.Migration.Migrations;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiTech.Library.DAL.MongoMirgrations.ThanhVien_Mirgrations
{
    public class V0_0_02_AddIsDelete : Migration<ThanhVien>
    {
        public V0_0_02_AddIsDelete() 
            : base("0.0.2")
        {
        }

        public override void Up(BsonDocument document)
        {
            document.Add("IsDeleted",false);
        }

        public override void Down(BsonDocument document)
        {
            document.Remove("IsDeleted");
        }
    }
}
