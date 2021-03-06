﻿using BiTech.Library.DAL.Respository;
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
    public class ChucVuEngine : EntityRepository<ChucVu>
    {
        public ChucVuEngine(IDatabase database, string databaseName, string tableName) : base(database, databaseName, tableName)
        {
            _Database = (IMongoDatabase)database.GetConnection(databaseName);
            _DatabaseCollection = _Database.GetCollection<ChucVu>(tableName);
        }

        public List<ChucVu> GetAllChucvu()
        {
            return _DatabaseCollection.Find(_ => true).ToList();
        }

        public void UpdateDBVersion()
        {
            var aa = (typeof(ChucVu).GetCustomAttributes(typeof(Mongo.Migration.Documents.Attributes.CurrentVersion), true).FirstOrDefault() as Mongo.Migration.Documents.Attributes.CurrentVersion);
            var listOld = _DatabaseCollection.Find(x => x.Version != aa.Version).ToList();

            foreach (var ss in listOld)
            {
                this.Update(ss);
            }
        }
    }
}
