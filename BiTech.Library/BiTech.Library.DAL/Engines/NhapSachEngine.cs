using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiTech.Library.DAL.Respository;
using BiTech.Library.DTO;
using MongoDB.Driver;

namespace BiTech.Library.DAL.Engines
{
    public class NhapSachEngine : EntityRepository<NhapSach>
    {
        public NhapSachEngine(IDatabase database, string tableName) : base(database, tableName)
        {
            _Database = (IMongoDatabase)database.GetConnection();
            _DatabaseCollection = _Database.GetCollection<NhapSach>(tableName);
        }

        public List<NhapSach> getAll()
        {
            return _DatabaseCollection.Find(p => true).ToList();
        }
    }
}
