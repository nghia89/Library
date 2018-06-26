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
    public class ChiTietPhieuTraEngine : EntityRepository<ChiTietPhieuTra>
    {
        public ChiTietPhieuTraEngine(IDatabase database, string tableName) : base(database, tableName)
        {
            _Database = (IMongoDatabase)database.GetConnection();
            _DatabaseCollection = _Database.GetCollection<ChiTietPhieuTra>(tableName);
        }
        /// <summary>
        /// Get all ChiTietPhieuTra object 
        /// </summary>
        /// <returns></returns>
        public List<ChiTietPhieuTra> GetAll()
        {
            return _DatabaseCollection.Find(_ => _.Id != null && _.Id != "").ToList();            
        }
        /// <summary>
        /// Get by ChiTietPhieuTra by IdBook
        /// </summary>
        /// <returns></returns>
        public List<ChiTietPhieuTra> GetByIdBook(string IdBook)
        {
            return _DatabaseCollection.Find(_ => _.IdSach == IdBook ).ToList();
        }
    }
}
