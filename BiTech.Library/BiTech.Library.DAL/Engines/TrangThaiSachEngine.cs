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
        public TrangThaiSachEngine(IDatabase database, string databaseName, string tableName) : base(database, databaseName, tableName)
        {
            _Database = (IMongoDatabase)database.GetConnection(databaseName);
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
        public List<TrangThaiSach> GetAllTT(string id)
        {
            return _DatabaseCollection.Find(x => x.Id != id).ToList();
        }

        #region Phong
        public List<TrangThaiSach> GetAllTT_True()
        {
            return _DatabaseCollection.Find(x => x.TrangThai == true).ToList();
        }
        public List<TrangThaiSach> GetAllTT_False()
        {
            return _DatabaseCollection.Find(x => x.TrangThai == false).ToList();
        }
        #endregion

        #region Tai
        public TrangThaiSach GetBySTT(string idTinhtrang)
        {
            return null;
        }
        public TrangThaiSach GetByName(string tenTrangThai)
        {
            return _DatabaseCollection.Find(_ => _.TenTT.ToLower() == tenTrangThai.ToLower()).FirstOrDefault();
        }
        #endregion

    }
}
