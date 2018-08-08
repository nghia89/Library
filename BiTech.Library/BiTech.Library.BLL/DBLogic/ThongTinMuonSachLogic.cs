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
    public class ThongTinMuonSachLogic
    {
        private string TableName = "ThongTinMuonSach";
        public ThongTinMuonSachEngine _ThongTinMuonSachEngine { get; set; }

        public ThongTinMuonSachLogic(string connectionString, string dbName)
        {
            _ThongTinMuonSachEngine = new ThongTinMuonSachEngine(new Database(connectionString), dbName, TableName);
        }
        /// <summary>
        /// Get all TrangThaiSach object 
        /// </summary>
        /// <returns></returns>
        public List<ThongTinMuonSach> GetAll()
        {
            return _ThongTinMuonSachEngine.GetAll();
        }
        public List<ThongTinMuonSach> GetAllTT(string id)
        {
            return _ThongTinMuonSachEngine.GetAllTT(id);
        }
        /// <summary>
        /// Insert a TinhTrangSach object
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string Insert(ThongTinMuonSach model)
        {
            return _ThongTinMuonSachEngine.Insert(model);
        }

        public string ThemTrangThai(ThongTinMuonSach TT)
        {
            return _ThongTinMuonSachEngine.Insert(TT);
        }

        public ThongTinMuonSach getById(string id)
        {
            return _ThongTinMuonSachEngine.GetById(id);
        }

        public bool SuaTrangThai(ThongTinMuonSach TT)
        {
            return _ThongTinMuonSachEngine.Update(TT);
        }

        public bool XoaTrangThai(string id)
        {
            return _ThongTinMuonSachEngine.Remove(id);
        }
    }
}
