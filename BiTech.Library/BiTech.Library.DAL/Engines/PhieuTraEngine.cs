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
    public class PhieuTraEngine : EntityRepository<PhieuTra>
    {
        public PhieuTraEngine(IDatabase database, string databaseName, string tableName) : base(database, databaseName, tableName)
        {
            _Database = (IMongoDatabase)database.GetConnection(databaseName);
            _DatabaseCollection = _Database.GetCollection<PhieuTra>(tableName);
        }
        /// <summary>
        /// Get all PhieuTra object 
        /// </summary>
        /// <returns></returns>
        public List<PhieuTra> GetAll()
        {
            return _DatabaseCollection.Find(_ => _.Id != null && _.Id != "").ToList();
        }        

        #region Tai
        public List<PhieuTra> GetPTByIdPM(string idPhieuMuon)
        {
            return _DatabaseCollection.Find(_ => _.IdPhieuMuon == idPhieuMuon).ToList();
        }
        #endregion
    }
}
