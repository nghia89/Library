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
    public class SachTheLoaiEngine : EntityRepository<SachTheLoai>
    {
        public SachTheLoaiEngine(IDatabase database, string databaseName, string tableName) : base(database, databaseName, tableName)
        {
            _Database = (IMongoDatabase)database.GetConnection(databaseName);
            _DatabaseCollection = _Database.GetCollection<SachTheLoai>(tableName);
        }

        public List<SachTheLoai> GetAllBookIdByAthurId(string id)
        {
            return _DatabaseCollection.Find(m => m.IdTheLoai == id).ToList();
        }
        public SachTheLoai GetAllBookIdBySachId(string id)
        {
            return _DatabaseCollection.Find(x => x.IdSach == id).FirstOrDefault();
        }

        public void UpdateDBVersion()
        {
            var aa = (typeof(SachTheLoai).GetCustomAttributes(typeof(Mongo.Migration.Documents.Attributes.CurrentVersion), true).FirstOrDefault() as Mongo.Migration.Documents.Attributes.CurrentVersion);
            var listOld = _DatabaseCollection.Find(x => x.Version != aa.Version).ToList();

            foreach (var ss in listOld)
            {
                this.Update(ss);
            }
        }

        #region Phong
        public List<SachTheLoai> GetAllBookIdBySachId_list(string id)
        {
            return _DatabaseCollection.Find(x => x.IdSach == id).ToList();
        }
        public bool DeleteAllTheLoaiByidSach(string idSach)
        {
            try
            {
                _DatabaseCollection.DeleteMany(x => x.IdSach == idSach);
            }
            catch
            {
                return false;
            }
            return true;
        }
        #endregion
    }
}
