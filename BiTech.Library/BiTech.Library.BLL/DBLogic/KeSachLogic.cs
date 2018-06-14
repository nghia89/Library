using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiTech.Library.DTO;
using BiTech.Library.DAL.Engines;
using BiTech.Library.DAL;

namespace BiTech.Library.BLL.DBLogic
{
    public class KeSachLogic
    {
        KeSachEngine _keSachEngine;
        public KeSachLogic(string connectionString,string databaseName)
        {
            Database database = new Database(connectionString, databaseName);
            _keSachEngine = new KeSachEngine(database, "KeSach");
        }
    }
}
