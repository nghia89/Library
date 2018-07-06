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
    public class TacGiaEngine : EntityRepository<TacGia>
    {
        public TacGiaEngine(IDatabase database, string databaseName, string tableName) : base(database, databaseName, tableName)
        {
            _Database = (IMongoDatabase)database.GetConnection(databaseName);
            _DatabaseCollection = _Database.GetCollection<TacGia>(tableName);
        }
        public List<TacGia> GetAllTacGia()
        {
            return _DatabaseCollection.Find(x => true).ToList();
        }
    }
}
