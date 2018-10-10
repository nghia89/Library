using BiTech.Library.Common;
using BiTech.Library.DAL.Respository;
using BiTech.Library.DTO;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
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

        #region Phong

        public List<SachCaBiet> GetAllSachCaBiet()
        {
            return _DatabaseCollection.Find(_ => true).ToList();
        }

        public SachCaBiet GetByMaKSCBorMaCaBienCu(string idMaCaBiet)
        {
            return _DatabaseCollection.Find(_ => _.MaKSCB == idMaCaBiet || _.MaCaBienCu == idMaCaBiet).FirstOrDefault();
        }

        public List<SachCaBiet> GetAllByMaKSCBorMaCaBienCu(string idMaCaBiet)
        {
            //return _DatabaseCollection.Find(_ => _.MaKSCB == idMaCaBiet || _.MaCaBienCu == idMaCaBiet).FirstOrDefault();

            FilterDefinition<SachCaBiet> filterDefinition = new BsonDocument();
            var builder = Builders<SachCaBiet>.Filter;
            filterDefinition = builder.Where(x => x.MaKSCB.Contains(idMaCaBiet) 
                                            || x.MaCaBienCu.Contains(idMaCaBiet));
            return _DatabaseCollection.Find(filterDefinition).ToList();
        }

        

        #endregion
    }
}
