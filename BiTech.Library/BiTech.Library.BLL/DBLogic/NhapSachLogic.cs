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
    public class NhapSachLogic
    {
        private string TableName = "NhapSach";
        public NhapSachEngine _NhapSachEngine { get; set; }

        public NhapSachLogic(string connectionString, string dbName)
        {
            _NhapSachEngine = new NhapSachEngine(new Database(connectionString, dbName), TableName);
        }

        public string NhapSach(NhapSach ns)
        {
            return _NhapSachEngine.Insert(ns);
        }
    }
}
