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
    /// (version 0.0.3) thành (version 0.0.4)
    /// </summary>
    public class V0_0_4_thembo_truong : Migration<Sach>
    {
        public V0_0_4_thembo_truong():base("0.0.4")
        { }

        public override void Down(BsonDocument document)
        {
            document.Remove("STTMaCB");
        }

        public override void Up(BsonDocument document)
        {
            document.Add("STTMaCB", 0);
        }
    }
}
