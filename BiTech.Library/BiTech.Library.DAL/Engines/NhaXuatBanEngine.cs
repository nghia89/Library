﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiTech.Library.DAL.Respository;
using BiTech.Library.DTO;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BiTech.Library.DAL.Engines
{
    public class NhaXuatBanEngine : EntityRepository<NhaXuatBan>
    {
        public NhaXuatBanEngine(IDatabase database, string tableName) : base(database, tableName)
        {
            _Database = (IMongoDatabase)database.GetConnection();
            _DatabaseCollection = _Database.GetCollection<NhaXuatBan>(tableName);
        }

        public List<NhaXuatBan> GetAllNhaXuatBan()
        {
            return _DatabaseCollection.Find(p => true).ToList(); 
        }
    }
}
