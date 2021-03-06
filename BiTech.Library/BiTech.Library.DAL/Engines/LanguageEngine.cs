﻿/*
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

        public void UpdateDBVersion()
        {
            var aa = (typeof(Language).GetCustomAttributes(typeof(Mongo.Migration.Documents.Attributes.CurrentVersion), true).FirstOrDefault() as Mongo.Migration.Documents.Attributes.CurrentVersion);
            var listOld = _DatabaseCollection.Find(x => x.Version != aa.Version).ToList();

            foreach (var ss in listOld)
            {
                this.Update(ss);
            }
        }
        #endregion

        public List<Language> GetByFindName(string Name)
        {
            return _DatabaseCollection.AsQueryable().Where(x => x.Ten.Equals(Name)).ToList();

        }
        public List<Language> GetByFindName1(string Name)
        {
            return _DatabaseCollection.AsQueryable().Where(x => x.Ten.Contains(Name)).ToList();

        }
        public Language GetByNameId(string Name)
        {
            return _DatabaseCollection.AsQueryable().Where(x => x.Ten == Name).FirstOrDefault();

        }
    }
}
