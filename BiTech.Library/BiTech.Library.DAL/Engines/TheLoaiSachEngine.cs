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
    public class TheLoaiSachEngine : EntityRepository<TheLoaiSach>
    {
        public TheLoaiSachEngine(IDatabase database, string tableName) : base(database, tableName)
        {
            _Database = (IMongoDatabase)database.GetConnection();
            _DatabaseCollection = _Database.GetCollection<TheLoaiSach>(tableName);
        }

        public List<TheLoaiSach> GetAllTheLoaiSach()
        {
            return _DatabaseCollection.Find(_ => true).ToList();
        }
    }
}
