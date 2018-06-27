using BiTech.Library.DAL;
using BiTech.Library.DAL.Engines;
using BiTech.Library.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiTech.Library.BLL.DBLogic
{
    public class SoLuongSachTrangThaiLogic
    {
        SoLuongSachTrangThaiEngine _SoLuongSachTrangThaiEngine;
        private string TableName = "SoLuongSachTrangThai";
        public SoLuongSachTrangThaiLogic(string connectionString, string databaseName)
        {
            Database database = new Database(connectionString, databaseName);
            _SoLuongSachTrangThaiEngine = new SoLuongSachTrangThaiEngine(database, TableName);
        }
        public bool Update(SoLuongSachTrangThai sls)
        {
            return _SoLuongSachTrangThaiEngine.Update(sls);
        }
        public string Insert(SoLuongSachTrangThai sls)
        {
            return _SoLuongSachTrangThaiEngine.Insert(sls);
        }
        public List<SoLuongSachTrangThai> GetAll()
        {
            return _SoLuongSachTrangThaiEngine.GetAll();
        }
        public SoLuongSachTrangThai getBy_IdSach_IdTT(string IdSach,string IdTingTrang)
        {
            return _SoLuongSachTrangThaiEngine.getBy_IdSach_IdTT(IdSach,IdTingTrang);
        }
    }
}
