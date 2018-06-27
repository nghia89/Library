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
    public class ChiTietNhapSachEngine : EntityRepository<ChiTietNhapSach>
    {
        public ChiTietNhapSachEngine(IDatabase database, string tableName) : base(database, tableName)
        {
            _Database = (IMongoDatabase)database.GetConnection();
            _DatabaseCollection = _Database.GetCollection<ChiTietNhapSach>(tableName);
        }
        public List<ChiTietNhapSach> GetAllChiTietNhapSach()
        {
            return _DatabaseCollection.Find(x => true).ToList();
        }
    }
}
