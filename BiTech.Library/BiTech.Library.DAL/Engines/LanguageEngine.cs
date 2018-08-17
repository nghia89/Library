/*
 Thinh
 */
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
    /// <summary>
    /// Engine hỗ trợ kế nối và thao tác tới bản ngôn ngữ
    /// </summary>
    public class LanguageEngine : EntityRepository<Language>
    {
        public LanguageEngine(IDatabase database, string databaseName, string tableName) : base(database, databaseName, tableName)
        {
            _Database = (IMongoDatabase)database.GetConnection(databaseName);
            _DatabaseCollection = _Database.GetCollection<Language>(tableName);
        }

        public List<Language> GetAll()
        {
            return _DatabaseCollection.Find(_ => true).ToList();
        }
		
        #region Tai
        public Language GetByTenNgonNgu(string tenNgonNgu)
        {
            return _DatabaseCollection.Find(_ => _.Ten.ToLower() == tenNgonNgu.ToLower()).FirstOrDefault();
        }
        #endregion
		
        public List<Language> GetByFindName(string Name)
        {
            return _DatabaseCollection.AsQueryable().Where(x => x.Ten == Name).ToList();

        }
        public Language GetByNameId(string Name)
        {
            return _DatabaseCollection.AsQueryable().Where(x => x.Ten == Name).FirstOrDefault();

        }
    }
}
