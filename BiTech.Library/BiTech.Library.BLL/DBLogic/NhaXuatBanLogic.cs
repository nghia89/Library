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
    public class NhaXuatBanLogic
    {
        private string TableName = "NhaXuatBan";
        public NhaXuatBanEngine _NhaXuatBanEngine { get; set; }

        public NhaXuatBanLogic(string connectionString, string dbName)
        {
            _NhaXuatBanEngine = new NhaXuatBanEngine(new Database(connectionString, dbName), TableName);
        }
    }
}
