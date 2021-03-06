﻿using BiTech.Library.DAL.Respository;
using BiTech.Library.DTO;
using MongoDB.Driver;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace BiTech.Library.DAL.Engines
{
    public class TheLoaiSachEngine : EntityRepository<TheLoaiSach>
    {
        public TheLoaiSachEngine(IDatabase database, string databaseName, string tableName) : base(database, databaseName, tableName)
        {
            _Database = (IMongoDatabase)database.GetConnection(databaseName);
            _DatabaseCollection = _Database.GetCollection<TheLoaiSach>(tableName);
        }

        public List<TheLoaiSach> GetAllTheLoaiSach()
        {
            return _DatabaseCollection.Find(_ => true).ToList();
        }

        public List<TheLoaiSach> GetTheTheLoaiSachRoot()
        {
            FilterDefinitionBuilder<TheLoaiSach> builder = Builders<TheLoaiSach>.Filter;

            FilterDefinition<TheLoaiSach> filter = builder.Empty;

            filter = filter & builder.Eq(m => m.IdParent, null);

            return _DatabaseCollection.Find(filter).ToList();
        }

        public List<TheLoaiSach> GetTheTheLoaiSachChildren(string idParent)
        {
            FilterDefinitionBuilder<TheLoaiSach> builder = Builders<TheLoaiSach>.Filter;

            FilterDefinition<TheLoaiSach> filter = builder.Empty;

            if (idParent.Length > 0)
            {
                filter = filter & builder.Eq(m => m.IdParent, idParent);
            }

            return _DatabaseCollection.Find(filter).ToList();
        }

        public List<TheLoaiSach> GetTheCon(string idParent, string mota)
        {
            FilterDefinitionBuilder<TheLoaiSach> builder = Builders<TheLoaiSach>.Filter;

            FilterDefinition<TheLoaiSach> filter = builder.Empty;

            if (idParent.Length > 0)
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

        public bool ktrTrung(TheLoaiSach tls)
        {
            var item = _DatabaseCollection.Find(_ => _.MaDDC == tls.MaDDC).FirstOrDefault();
            if (item != null)
            {
                return true;
            }
            return false;
        }

        public List<TheLoaiSach> FindTheLoai(string q)
        {
            return _DatabaseCollection.AsQueryable().Where(delegate (TheLoaiSach c)
            {
                if (ConvertToUnSign(c.TenTheLoai.ToLower()).Contains(ConvertToUnSign(q.ToLower())))
                    return true;
                else
                    return false;
            }).ToList(); //(name => ConvertToUnSign(name.TenTacGia.ToLower()).Contains(ConvertToUnSign(q.ToLower()))).ToList();
        }

        public string ConvertToUnSign(string input)
        {
            input = input.Trim();
            for (int i = 0x20; i < 0x30; i++)
            {
                input = input.Replace(((char)i).ToString(), " ");
            }
            Regex regex = new Regex(@"\p{IsCombiningDiacriticalMarks}+");
            string str = input.Normalize(NormalizationForm.FormD);
            string str2 = regex.Replace(str, string.Empty).Replace('đ', 'd').Replace('Đ', 'D');
            while (str2.IndexOf("?") >= 0)
            {
                str2 = str2.Remove(str2.IndexOf("?"), 1);
            }
            return str2;
        }

        #region Tai
        public TheLoaiSach GetIdByDDC(string maDDC)
        {
            return _DatabaseCollection.Find(_ => _.MaDDC == maDDC).FirstOrDefault();
        }
        public TheLoaiSach GetByTenTheLoai(string tenTheLoai)
        {
            return _DatabaseCollection.Find(_ => _.TenTheLoai.ToLower() == tenTheLoai.ToLower()).FirstOrDefault();
        }

        public void UpdateDBVersion()
        {
            var aa = (typeof(TheLoaiSach).GetCustomAttributes(typeof(Mongo.Migration.Documents.Attributes.CurrentVersion), true).FirstOrDefault() as Mongo.Migration.Documents.Attributes.CurrentVersion);
            var listOld = _DatabaseCollection.Find(x => x.Version != aa.Version).ToList();

            foreach (var ss in listOld)
            {
                this.Update(ss);
            }
        }
        #endregion

    }
}
