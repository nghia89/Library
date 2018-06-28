﻿using BiTech.Library.DAL.Respository;
using BiTech.Library.DTO;
using MongoDB.Driver;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiTech.Library.DAL.Engines
{
    public class TheLoaiSachEngine : EntityRepository<TheLoaiSach>
    {
        public TheLoaiSachEngine(IDatabase database, string tableName) : base(database, tableName)
        {
            _Database = (IMongoDatabase)database.GetConnection();
            _DatabaseCollection = _Database.GetCollection<TheLoaiSach>(tableName);
        }

        public List<TheLoaiSach> GetAllTheLoaiSach()
        {
            return _DatabaseCollection.Find(_ => true).ToList();
        }

        public List<TheLoaiSach> GetTheCon(string idParent, string mota)
        {
            FilterDefinitionBuilder<TheLoaiSach> builder = Builders<TheLoaiSach>.Filter;

            FilterDefinition<TheLoaiSach> filter = builder.Empty;

            if(idParent.Length > 0)
            {
                filter = filter & builder.Eq(m => m.IdParent, idParent);
            }

            if (mota.Length > 0)
            {
                filter = filter & builder.Eq(m => m.MoTa, mota);
            }

            return _DatabaseCollection.Find(filter).ToList();

            //return _DatabaseCollection.Find(_ => _.IdParent == idParent).ToList();
        }
    }
}
