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
        public KeSachEngine(IDatabase database, string databaseName, string tableName) : base(database, databaseName, tableName)
        {
            _Database = (IMongoDatabase)database.GetConnection(databaseName);
            _DatabaseCollection = _Database.GetCollection<KeSach>(tableName);
        }
        public List<KeSach> GetAllKeSach()
        {
            return _DatabaseCollection.Find(_ => true).ToList();
        }
        #region Tai
        public KeSach GetByTenKeSach(string tenKeSach)
        {
            return _DatabaseCollection.Find(_ => _.TenKe.ToLower() == tenKeSach.ToLower()).FirstOrDefault();
        }
        #endregion

    }
}
