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
    public class ChiTietPhieuMuonEngine : EntityRepository<ChiTietPhieuMuon>
    {
        public ChiTietPhieuMuonEngine(IDatabase database, string tableName) : base(database, tableName)
        {
            _Database = (IMongoDatabase)database.GetConnection();
            _DatabaseCollection = _Database.GetCollection<ChiTietPhieuMuon>(tableName);
        }
        /// <summary>
        /// Get all ChiTietPhieuMuon object 
        /// </summary>
        /// <returns></returns>
        public List<ChiTietPhieuMuon> GetAll()
        {
            return _DatabaseCollection.Find(_ => _.Id != null && _.Id != "").ToList();            
        }

        #region Tai
        public List<ChiTietPhieuMuon> GetCTPMbyId(string idPhieuMuon)
        {
            return _DatabaseCollection.Find(x => x.IdPhieuMuon == idPhieuMuon).ToList();
        }
        #endregion    
    }
}
