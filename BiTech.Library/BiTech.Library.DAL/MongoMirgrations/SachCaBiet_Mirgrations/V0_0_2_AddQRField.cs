using BiTech.Library.DTO;
using Mongo.Migration.Migrations;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiTech.Library.DAL.MongoMirgrations.SachCaBiet_Mirgrations
{
    public class V0_0_2_AddQRField : Migration<SachCaBiet>
    {
        public V0_0_2_AddQRField()
          : base("0.0.2")
        {
        }
        public override void Down(BsonDocument document)
        {            
        }

        public override void Up(BsonDocument document)
        {
            document.Add("QRlink", "");
            document.Add("QRData", "");
        }
    }
}
