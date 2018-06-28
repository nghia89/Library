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
    public class ChiTietXuatSachEngine : EntityRepository<ChiTietXuatSach>
    {
        public ChiTietXuatSachEngine(IDatabase database, string tableName) : base(database, tableName)
        {
            _Database = (IMongoDatabase)database.GetConnection();
            _DatabaseCollection = _Database.GetCollection<ChiTietXuatSach>(tableName);
        }
        public List<ChiTietXuatSach> GetAllChiTietXuatSach()
        {
            return _DatabaseCollection.Find(x => true).ToList();
        }
    }
}
