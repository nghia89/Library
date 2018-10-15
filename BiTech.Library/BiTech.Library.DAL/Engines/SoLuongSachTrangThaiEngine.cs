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
    public class SoLuongSachTrangThaiEngine : EntityRepository<SoLuongSachTrangThai>
    {
        public SoLuongSachTrangThaiEngine(IDatabase database, string databaseName, string tableName) : base(database, databaseName, tableName)
        {
            _Database = (IMongoDatabase)database.GetConnection(databaseName);
            _DatabaseCollection = _Database.GetCollection<SoLuongSachTrangThai>(tableName);
        }

        public List<SoLuongSachTrangThai> GetAll()
        {
            return _DatabaseCollection.Find(x => true).ToList();
        }

        public SoLuongSachTrangThai getBy_IdSach_IdTT(string IdSach,string IdTinhTrang)
        {
            return _DatabaseCollection.Find(x => x.IdSach == IdSach && x.IdTrangThai == IdTinhTrang).FirstOrDefault();
        }
        
        public List<SoLuongSachTrangThai> GetByIdSach(string id)
        {
        return _DatabaseCollection.Find(x => x.IdSach == id).ToList();
        }

        public SoLuongSachTrangThai GetByIdTT(string id,string IdSach)
        {
            return _DatabaseCollection.Find(x => x.IdTrangThai == id&&x.IdSach== IdSach).FirstOrDefault();
        }

        public bool DeleteAllSoLuongSachTrangThaiByidSach(string idSach)
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

        public void UpdateDBVersion()
        {
            var aa = (typeof(SoLuongSachTrangThai).GetCustomAttributes(typeof(Mongo.Migration.Documents.Attributes.CurrentVersion), true).FirstOrDefault() as Mongo.Migration.Documents.Attributes.CurrentVersion);
            var listOld = _DatabaseCollection.Find(x => x.Version != aa.Version).ToList();

            foreach (var ss in listOld)
            {
                this.Update(ss);
            }
        }
    }
}
