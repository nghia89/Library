using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiTech.Library.DAL;
using BiTech.Library.DAL.Engines;
using BiTech.Library.DTO;


namespace BiTech.Library.BLL.DBLogic
{
    public class XuatSachLogic
    {
        private string TableName = "XuatSach";
        public XuatSachEngine _XuatSachEngine { get; set; }

        public XuatSachLogic(string connectionString, string dbName)
        {
            _XuatSachEngine = new XuatSachEngine(new Database(connectionString, dbName), TableName);
        }
    }
}
