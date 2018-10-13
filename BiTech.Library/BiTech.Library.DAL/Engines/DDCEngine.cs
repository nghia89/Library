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
    public class DDCEngine : EntityRepository<DDC>
    {
        public DDCEngine(IDatabase database, string databaseName, string tableName) : base(database, databaseName, tableName)
        {
            _Database = (IMongoDatabase)database.GetConnection(databaseName);
            _DatabaseCollection = _Database.GetCollection<DDC>(tableName);
        }

        public List<DDC> GetAllDDC()
        {
            return _DatabaseCollection.Find(_ => true).ToList();
        }

        public List<DDC> getDDCByKeySearch(KeySearchViewModel KeySearch)
        {
            FilterDefinition<DDC> filterDefinition = new BsonDocument();
            var builder = Builders<DDC>.Filter;

            // Tìm theo UserName
            if (!string.IsNullOrEmpty(KeySearch.Keyword))
            {
                //ToLower chuyễn chữ hoa sang thường
                filterDefinition = filterDefinition & builder.Where(x => x.Ten.ToLower().Contains(KeySearch.Keyword.ToLower())
                || x.MaDDC.ToLower().Contains(KeySearch.Keyword.ToLower())
                );
            }

            return _DatabaseCollection.Find(filterDefinition).ToList();
        }

        public void UpdateDBVersion()
        {
            var aa = (typeof(DDC).GetCustomAttributes(typeof(Mongo.Migration.Documents.Attributes.CurrentVersion), true).FirstOrDefault() as Mongo.Migration.Documents.Attributes.CurrentVersion);
            var listOld = _DatabaseCollection.Find(x => x.Version != aa.Version).ToList();

            foreach (var ss in listOld)
            {
                this.Update(ss);
            }
        }
    }
}
