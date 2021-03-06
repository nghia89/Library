﻿using BiTech.Library.DAL;
using BiTech.Library.DAL.Engines;
using BiTech.Library.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiTech.Library.BLL.DBLogic
{
    public class ThongTinThuVienLogic : BaseLogic
    {
        ThongTinThuVienEngine _ThongTinThuVienEngine;
        public ThongTinThuVienLogic(string connectionString, string databaseName)
        {
            Database database = new Database(connectionString);
            _ThongTinThuVienEngine = new ThongTinThuVienEngine(database, databaseName, DBTableNames.ThongTinThuVien_Table);
        }

        #region TenThuVien

        public void SetTenThuVien(string value)
        {
            _ThongTinThuVienEngine.SetTenThuVien(value);
        }

        public string GetTenThuVien()
        {
            return _ThongTinThuVienEngine.GetTenThuVien();
        }

        #endregion

        #region Thẻ Header1

        public void SetTheHeader1(string value)
        {
            _ThongTinThuVienEngine.SetTheHeader1(value);
        }

        public string GetTheHeader1()
        {
            return _ThongTinThuVienEngine.GetTheHeader1();
        }

        #endregion

        #region Thẻ Header2

        public void SetTheHeader2(string value)
        {
            _ThongTinThuVienEngine.SetTheHeader2(value);
        }

        public string GetTheHeader2()
        {
            return _ThongTinThuVienEngine.GetTheHeader2();
        }

        #endregion

        #region DiaChi

        public void SetDiaChi(string value)
        {
            _ThongTinThuVienEngine.SetDiaChi(value);
        }

        public string GetDiaChi()
        {
            return _ThongTinThuVienEngine.GetDiaChi();
        }

        #endregion

        #region SoNgayMuonMax

        public void SetSoNgayMuonMax(int value)
        {
            _ThongTinThuVienEngine.SetSoNgayMuonMax(value.ToString());
        }

        public int GetSoNgayMuonMax()
        {
            var setting = _ThongTinThuVienEngine.GetSoNgayMuonMax();
            int soLan = 0;
            if (!int.TryParse(setting, out soLan))
            {
                _ThongTinThuVienEngine.SetSoNgayMuonMax("15");
                return 15;
            }
            return soLan;
        }

        #endregion

        #region MaKiemSoatSachCount

        public void SetMaKiemSoatSachCount(ulong value)
        {
            _ThongTinThuVienEngine.SetMaKiemSoatSachCount(value.ToString());
        }

        public ulong GetMaKiemSoatSachCount()
        {
            var setting = _ThongTinThuVienEngine.GetMaKiemSoatSachCount();
            ulong max = 0;
            if (!ulong.TryParse(setting, out max))
            {
                _ThongTinThuVienEngine.SetMaKiemSoatSachCount("0");
                return 1;
            }
            return max;
        }

        #endregion

        public void SetCustomKey(ThongTinThuVien tt)
        {
            _ThongTinThuVienEngine.SetValueByKey(tt.Key, tt.Value);
        }

        public void SetCustomKey(string key, string value)
        {
            _ThongTinThuVienEngine.SetValueByKey(key, value);
        }

        public ThongTinThuVien GetCustomKey(string key)
        {
            return _ThongTinThuVienEngine.GetByKey(key);
        }

        public void UpdateDBVersion()
        {
            _ThongTinThuVienEngine.UpdateDBVersion();
        }
    }
}
