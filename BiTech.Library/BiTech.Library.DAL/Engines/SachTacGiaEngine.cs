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
    public class SachTacGiaEngine : EntityRepository<SachTacGia>
    {
        public SachTacGiaEngine(IDatabase database, string databaseName, string tableName) : base(database, databaseName, tableName)
        {
            _Database = (IMongoDatabase)database.GetConnection(databaseName);
            _DatabaseCollection = _Database.GetCollection<SachTacGia>(tableName);
        }

        public List<SachTacGia> GetAllBookIdByAthurId(string id)
        {
            return _DatabaseCollection.Find(m => m.IdTacGia == id).ToList();
        }
    }
}
