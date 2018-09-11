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
        public ChiTietXuatSachEngine(IDatabase database, string databaseName, string tableName) : base(database, databaseName, tableName)
        {
            _Database = (IMongoDatabase)database.GetConnection(databaseName);
            _DatabaseCollection = _Database.GetCollection<ChiTietXuatSach>(tableName);
        }
        public List<ChiTietXuatSach> GetAllChiTietXuatSach()
        {
            return _DatabaseCollection.Find(x => true).ToList();
        }

        public List<ChiTietXuatSach> GetAllChiTietById(string id)
        {
            return _DatabaseCollection.Find(x => x.IdPhieuXuat == id).ToList();
        }
        public List<ChiTietXuatSach> GetAllChiTietByIdSach(string idSach)
        {
            return _DatabaseCollection.Find(x => x.IdSach == idSach).ToList();
        }
    }
}
