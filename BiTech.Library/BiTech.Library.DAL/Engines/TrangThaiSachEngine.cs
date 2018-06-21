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
    public class TrangThaiSachEngine : EntityRepository<TrangThaiSach>
    {
        public TrangThaiSachEngine(IDatabase database, string tableName) : base(database, tableName)
        {
            _Database = (IMongoDatabase)database.GetConnection();
            _DatabaseCollection = _Database.GetCollection<TrangThaiSach>(tableName);
        }
        /// <summary>
        /// Get all TrangThaiSach object 
        /// </summary>
        /// <returns></returns>
        public List<TrangThaiSach> GetAll()
        {
            return _DatabaseCollection.Find(p => true).ToList();
        }
    }
}
