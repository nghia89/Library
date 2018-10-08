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
    public class SachCaBietEngine : EntityRepository<SachCaBiet>
    {
        public SachCaBietEngine(IDatabase database, string databaseName, string tableName) : base(database, databaseName, tableName)
        {
            _Database = (IMongoDatabase)database.GetConnection(databaseName);
            _DatabaseCollection = _Database.GetCollection<SachCaBiet>(tableName);
        }
        #region Vinh      

        public SachCaBiet GetIdSachFromMaCaBiet(string maCaBiet)
        {
            return _DatabaseCollection.Find(x => x.MaKSCB == maCaBiet).FirstOrDefault();
        }

        public List<SachCaBiet> GetListCaBietFromIdSach(string idSach)
        {
            return _DatabaseCollection.Find(x => x.IdSach == idSach).ToList();
        }
        #endregion
    }
}
