﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiTech.Library.DAL.Respository;
using BiTech.Library.DTO;
using MongoDB.Driver;

namespace BiTech.Library.DAL.Engines
{
    public class LyDoXuatEngine : EntityRepository<LyDoXuat>
    {
        public LyDoXuatEngine(IDatabase database, string databaseName, string tableName) : base(database, databaseName, tableName)
        {
            _Database = (IMongoDatabase)database.GetConnection(databaseName);
            _DatabaseCollection = _Database.GetCollection<LyDoXuat>(tableName);
        }
        public List<LyDoXuat> GetAll()
        {
            return _DatabaseCollection.Find(x => true).ToList();
        }
    }
}
