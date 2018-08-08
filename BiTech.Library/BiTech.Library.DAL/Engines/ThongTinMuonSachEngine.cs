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
    public class ThongTinMuonSachEngine : EntityRepository<ThongTinMuonSach>
    {
        public ThongTinMuonSachEngine(IDatabase database, string databaseName, string tableName) : base(database, databaseName, tableName)
        {
            _Database = (IMongoDatabase)database.GetConnection(databaseName);
            _DatabaseCollection = _Database.GetCollection<ThongTinMuonSach>(tableName);
        }
        /// <summary>
        /// Get all ThongTinMuonSach object 
        /// </summary>
        /// <returns></returns>
        public List<ThongTinMuonSach> GetAll()
        {
            return _DatabaseCollection.Find(p => true).ToList();
        }
        public List<ThongTinMuonSach> GetAllTT(string id)
        {
            return _DatabaseCollection.Find(x => x.Id != id).ToList();
        }
    }
}
