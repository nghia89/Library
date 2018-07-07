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
    public class SoLuongSachTrangThaiEngine : EntityRepository<SoLuongSachTrangThai>
    {
        public SoLuongSachTrangThaiEngine(IDatabase database, string databaseName, string tableName) : base(database, databaseName, tableName)
        {
            _Database = (IMongoDatabase)database.GetConnection(databaseName);
            _DatabaseCollection = _Database.GetCollection<SoLuongSachTrangThai>(tableName);
        }
        public List<SoLuongSachTrangThai> GetAll()
        {
            return _DatabaseCollection.Find(x => true).ToList();
        }
        public SoLuongSachTrangThai getBy_IdSach_IdTT(string IdSach,string IdTinhTrang)
        {
            return _DatabaseCollection.Find(x => x.IdSach == IdSach && x.IdTrangThai == IdTinhTrang).FirstOrDefault();
        }
    }
}
