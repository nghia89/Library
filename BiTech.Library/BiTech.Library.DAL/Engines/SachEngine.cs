using BiTech.Library.DAL.Respository;
using BiTech.Library.DTO;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiTech.Library.DAL.Engines
{
    public class SachEngine : EntityRepository<Sach>
    {
        public SachEngine(IDatabase database, string databaseName, string tableName) : base(database, databaseName, tableName)
        {
            _Database = (IMongoDatabase)database.GetConnection(databaseName);
            _DatabaseCollection = _Database.GetCollection<Sach>(tableName);
        }

        public override string Insert(Sach entity)
        {
            entity.Id = null;
            entity.CreateDateTime = DateTime.Now;

            var list = _DatabaseCollection.Find(m => m.MaKiemSoat == entity.MaKiemSoat).ToList();

            if(list != null && list.Count > 0)
                return "";

            _DatabaseCollection.InsertOne(entity);
            return entity.Id.ToString();
        }

        #region vinh
        /// <summary>
        /// Get book by idDauSach
        /// </summary>
        /// <param name="idBook"></param>
        /// <returns></returns>
        //public Sach GetByIdBook(string idBook)
        //{
        //    return _DatabaseCollection.Find(_ => _.IdDauSach == idBook ).FirstOrDefault();
        //}

        public Sach GetByMaKiemSoat(string MaKS)
        {
            return _DatabaseCollection.Find(x => x.MaKiemSoat == MaKS).FirstOrDefault();
        }
        #endregion

        public List<Sach> GetAllSach()
        {
            return _DatabaseCollection.Find(p => true).ToList();
        }
    }
}
