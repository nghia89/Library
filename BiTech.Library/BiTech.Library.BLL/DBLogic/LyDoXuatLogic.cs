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
    public class LyDoXuatLogic
    {
        private string TableName = "LyDoXuat";
        public LyDoXuatEngine _LyDoXuatEngine { get; set; }
        
        public LyDoXuatLogic(string connectionString, string dbName)
        {
            _LyDoXuatEngine = new LyDoXuatEngine(new Database(connectionString, dbName), TableName);    
        }
    }
}
