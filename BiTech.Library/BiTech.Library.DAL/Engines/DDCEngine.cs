using BiTech.Library.DAL.Respository;
using BiTech.Library.DTO;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiTech.Library.DAL.Engines
{
    public class DDCEngine : EntityRepository<DDC>
    {
        public DDCEngine(IDatabase database, string databaseName, string tableName) : base(database, databaseName, tableName)
        {
            _Database = (IMongoDatabase)database.GetConnection(databaseName);
            _DatabaseCollection = _Database.GetCollection<DDC>(tableName);
        }

        public List<DDC> GetAllDDC()
        {
            return _DatabaseCollection.Find(_ => true).ToList();
        }
    }
}
