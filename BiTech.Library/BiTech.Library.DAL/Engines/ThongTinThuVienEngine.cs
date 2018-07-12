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
    public class ThongTinThuVienEngine : EntityRepository<ThongTinThuVien>
    {
        public ThongTinThuVienEngine(IDatabase database, string databaseName, string tableName) : base(database, databaseName, tableName)
        {
            _Database = (IMongoDatabase)database.GetConnection(databaseName);
            _DatabaseCollection = _Database.GetCollection<ThongTinThuVien>(tableName);
        }

        #region TenThuVien

        public void SetTenThuVien(string value)
        {
            _DatabaseCollection.InsertOne(new ThongTinThuVien() { Key = "TenThuVien", Value = value, CreateDateTime = DateTime.Now });
        }

        public string GetTenThuVien()
        {
            var setting = _DatabaseCollection.Find(_ => _.Key == "TenThuVien").FirstOrDefault();
            if (setting != null)
                return setting.Value;
            return "";
        }

        #endregion

        #region DiaChi

        public void SetDiaChi(string value)
        {
            _DatabaseCollection.InsertOne(new ThongTinThuVien() { Key = "DiaChi", Value = value, CreateDateTime = DateTime.Now });
        }

        public string GetDiaChi()
        {
            var setting = _DatabaseCollection.Find(_ => _.Key == "DiaChi").FirstOrDefault();
            if (setting != null)
                return setting.Value;
            return "";
        }

        #endregion

        #region SoNgayMuonMax

        public void SetSoNgayMuonMax(string value)
        {
            _DatabaseCollection.InsertOne(new ThongTinThuVien() { Key = "SoNgayMuonMax", Value = value, CreateDateTime = DateTime.Now });
        }

        public string GetSoNgayMuonMax()
        {
            var setting = _DatabaseCollection.Find(_ => _.Key == "SoNgayMuonMax").FirstOrDefault();
            if (setting != null)
                return setting.Value;
            return "";
        }

        #endregion

        #region MaKiemSoatSachCount

        public void SetMaKiemSoatSachCount(string value)
        {
            _DatabaseCollection.InsertOne(new ThongTinThuVien() { Key = "MaKiemSoatSachCount", Value = value, CreateDateTime = DateTime.Now });
        }

        public string GetMaKiemSoatSachCount()
        {
            var setting = _DatabaseCollection.Find(_ => _.Key == "MaKiemSoatSachCount").FirstOrDefault();
            if (setting != null)
                return setting.Value;
            return "";
        }

        #endregion


        public ThongTinThuVien GetByKey(string key)
        {
            return _DatabaseCollection.Find(_ => _.Key == key).FirstOrDefault();
        }
    }


    // Tên thư viện : TenThuVien
    // Địa chỉ thư viện : DiaChi
    // Số ngày mượn tối đa : SoNgayMuonMax
    // Mã kiểm soát sách : MaKiemSoatSachCount
}