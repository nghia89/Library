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
        /// Get all PhieuMuon object _ chưa trả
        /// </summary>
        /// <returns></returns>
        public List<PhieuMuon> GetAll()
        {
            return _DatabaseCollection.Find(_ => _.TrangThaiPhieu != EPhieuMuon.Deleted && _.NgayTra == null).ToList();            
        }   
        /// <summary>
        /// Get list PhieuMuon chưa trả
        /// </summary>
        /// <param name="idUser"></param>
        /// <returns></returns>
        public List<PhieuMuon> GetByIdUser(string idUser)
        {
            return _DatabaseCollection.Find(_ => _.IdUser == idUser && _.NgayTra == null).ToList();
        }
        //public List<PhieuMuon> GetByDate()

        #region Tai
        public List<PhieuMuon> GetPMByIdUser(string idUser)
        {
            return _DatabaseCollection.Find(_ => _.IdUser == idUser).ToList();
        }
        #endregion
    }
}
