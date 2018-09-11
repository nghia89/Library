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
    public class AccessInfoEngine : EntityRepository<AccessInfo>
    {
        public AccessInfoEngine(IDatabase database, string databaseName, string tableName) : base(database, databaseName, tableName)
        {
            _Database = (IMongoDatabase)database.GetConnection(databaseName);
            _DatabaseCollection = _Database.GetCollection<AccessInfo>(tableName);
        }

        public List<AccessInfo> GetAllChiTietNhapSach()
        {
            return _DatabaseCollection.Find(x => true).ToList();
        }

        public AccessInfo GetWorkPlaceByIdWorkplace(string IdWp)
        {
            return _DatabaseCollection.Find(x => x.IdWorkplace == IdWp).FirstOrDefault();
        }

        public AccessInfo GetWorkPlaceBySubDomain(string subdomain)
        {
            return _DatabaseCollection.Find(x => x.WebSubDomain == subdomain).FirstOrDefault();
        }
    }
}
