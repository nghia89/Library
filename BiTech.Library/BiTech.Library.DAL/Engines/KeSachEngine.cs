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
    public class KeSachEngine : EntityRepository<KeSach>
    {
        public KeSachEngine(IDatabase database, string tableName) : base(database, tableName)
        {
            _Database = (IMongoDatabase)database.GetConnection();
            _DatabaseCollection = _Database.GetCollection<KeSach>(tableName);
        }
        public List<KeSach> GetAllKeSach()
        {
            return _DatabaseCollection.Find(_ => true).ToList();
        }

      
    }
}
