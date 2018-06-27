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
    public class SachEngine : EntityRepository<Sach>
    {
        public SachEngine(IDatabase database, string tableName) : base(database, tableName)
        {
            _Database = (IMongoDatabase)database.GetConnection();
            _DatabaseCollection = _Database.GetCollection<Sach>(tableName);
        }

        #region vinh
        /// <summary>
        /// Get book by idDauSach
        /// </summary>
        /// <param name="idBook"></param>
        /// <returns></returns>
        public Sach GetByIdBook(string idBook)
        {
            return _DatabaseCollection.Find(_ => _.IdDauSach == idBook ).FirstOrDefault();
        }
        public Sach GetBookById(string Id)
        {
            return _DatabaseCollection.Find(x => x.Id == Id).FirstOrDefault();
        }
        #endregion

        public List<Sach> GetAllSach()
        {
            return _DatabaseCollection.Find(p => true).ToList();
        }
    }
}
