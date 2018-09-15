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
            var setting = _DatabaseCollection.Find(_ => _.Key == "TenThuVien").FirstOrDefault();
            if (setting == null)
            {
                _DatabaseCollection.InsertOne(new ThongTinThuVien() { Key = "TenThuVien", Value = value, CreateDateTime = DateTime.Now });
            }
            else
            {
                setting.Value = value;
                _DatabaseCollection.ReplaceOne(m => m.Id == setting.Id, setting);
            }
        }

        public string GetTenThuVien()
        {
            var setting = _DatabaseCollection.Find(_ => _.Key == "TenThuVien").FirstOrDefault();
            if (setting != null)
                return setting.Value;
            return "";
        }

        #endregion

        #region Thẻ Header1

        public void SetTheHeader1(string value)
        {
            var setting = _DatabaseCollection.Find(_ => _.Key == "TheHeader1").FirstOrDefault();
            if (setting == null)
            {
                _DatabaseCollection.InsertOne(new ThongTinThuVien() { Key = "TheHeader1", Value = value, CreateDateTime = DateTime.Now });
            }
            else
            {
                setting.Value = value;
                _DatabaseCollection.ReplaceOne(m => m.Id == setting.Id, setting);
            }
        }

        public string GetTheHeader1()
        {
            var setting = _DatabaseCollection.Find(_ => _.Key == "TheHeader1").FirstOrDefault();
            if (setting != null)
                return setting.Value;
            return "";
        }

        #endregion

        #region Thẻ Header2

        public void SetTheHeader2(string value)
        {
            var setting = _DatabaseCollection.Find(_ => _.Key == "TheHeader2").FirstOrDefault();
            if (setting == null)
            {
                _DatabaseCollection.InsertOne(new ThongTinThuVien() { Key = "TheHeader2", Value = value, CreateDateTime = DateTime.Now });
            }
            else
            {
                setting.Value = value;
                _DatabaseCollection.ReplaceOne(m => m.Id == setting.Id, setting);
            }
        }

        public string GetTheHeader2()
        {
            var setting = _DatabaseCollection.Find(_ => _.Key == "TheHeader2").FirstOrDefault();
            if (setting != null)
                return setting.Value;
            return "";
        }

        #endregion

        #region DiaChi

        public void SetDiaChi(string value)
        {
            var setting = _DatabaseCollection.Find(_ => _.Key == "DiaChi").FirstOrDefault();
            if (setting == null)
            {
                _DatabaseCollection.InsertOne(new ThongTinThuVien() { Key = "DiaChi", Value = value, CreateDateTime = DateTime.Now });
            }
            else
            {
                setting.Value = value;
                _DatabaseCollection.ReplaceOne(m => m.Id == setting.Id, setting);
            }
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
            var setting = _DatabaseCollection.Find(_ => _.Key == "SoNgayMuonMax").FirstOrDefault();
            if (setting == null)
            {
                _DatabaseCollection.InsertOne(new ThongTinThuVien() { Key = "SoNgayMuonMax", Value = value, CreateDateTime = DateTime.Now });
            }
            else
            {
                setting.Value = value;
                _DatabaseCollection.ReplaceOne(m => m.Id == setting.Id, setting);
            }
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
            var setting = _DatabaseCollection.Find(_ => _.Key == "MaKiemSoatSachCount").FirstOrDefault();
            if (setting == null)
            {
                _DatabaseCollection.InsertOne(new ThongTinThuVien() { Key = "MaKiemSoatSachCount", Value = value, CreateDateTime = DateTime.Now });
            }
            else
            {
                setting.Value = value;
                _DatabaseCollection.ReplaceOne(m => m.Id == setting.Id, setting);
            }
        }

        public string GetMaKiemSoatSachCount()
        {
            var setting = _DatabaseCollection.Find(_ => _.Key == "MaKiemSoatSachCount").FirstOrDefault();
            if (setting != null)
                return setting.Value;
            return "";
        }

        #endregion

        #region Custom setting

        public ThongTinThuVien GetByKey(string key)
        {
            return _DatabaseCollection.Find(_ => _.Key == key).FirstOrDefault();
        }

        public void SetValueByKey(string key, string value)
        {
            var setting = _DatabaseCollection.Find(_ => _.Key == key).FirstOrDefault();
            if (setting == null)
            {
                _DatabaseCollection.InsertOne(new ThongTinThuVien() { Key = key, Value = value, CreateDateTime = DateTime.Now });
            }
            else
            {
                setting.Value = value;
                _DatabaseCollection.ReplaceOne(m => m.Id == setting.Id, setting);
            }
        }

        #endregion
    }


    // Tên thư viện : TenThuVien
    // Địa chỉ thư viện : DiaChi
    // Số ngày mượn tối đa : SoNgayMuonMax
    // Mã kiểm soát sách : MaKiemSoatSachCount
}
