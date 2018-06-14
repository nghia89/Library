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
    public class PhieuMuonEngine : EntityRepository<PhieuMuon>
    {
        public PhieuMuonEngine(IDatabase database, string tableName) : base(database, tableName)
        {
            _Database = (IMongoDatabase)database.GetConnection();
            _DatabaseCollection = _Database.GetCollection<PhieuMuon>(tableName);
        }
        /// <summary>
        /// Get all PhieuMuon object 
        /// </summary>
        /// <returns></returns>
        public List<PhieuMuon> GetAll()
        {
            return _DatabaseCollection.Find(_ => _.TrangThaiPhieu != EPhieuMuon.Deleted).ToList();            
        }   

        //public List<PhieuMuon> GetByDate()
    }
}
