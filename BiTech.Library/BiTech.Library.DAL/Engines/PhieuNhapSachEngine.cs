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
    public class PhieuNhapSachEngine : EntityRepository<PhieuNhapSach>
    {
        public PhieuNhapSachEngine(IDatabase database, string databaseName, string tableName) : base(database, databaseName, tableName)
        {
            _Database = (IMongoDatabase)database.GetConnection(databaseName);
            _DatabaseCollection = _Database.GetCollection<PhieuNhapSach>(tableName);
        }

        public List<PhieuNhapSach> getAll()
        {
            return _DatabaseCollection.Find(p => true).ToList();
        }
    }
}
