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
    public class TrangThaiSachLogic
    {
        private string TableName = "TrangThaiSach";
        public TrangThaiSachEngine _TrangThaiSachEngine { get; set; }

        public TrangThaiSachLogic(string connectionString, string dbName)
        {
            _TrangThaiSachEngine = new TrangThaiSachEngine(new Database(connectionString, dbName), TableName);
        }
        /// <summary>
        /// Get all TrangThaiSach object 
        /// </summary>
        /// <returns></returns>
        public List<TrangThaiSach> GetAll()
        {
            return _TrangThaiSachEngine.GetAll();
        }
		
        /// <summary>
        /// Insert a TinhTrangSach object
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string Insert(TrangThaiSach model)
        {
            return _TrangThaiSachEngine.Insert(model);
        }

        public string ThemTrangThai(TrangThaiSach TT)
        {
            return _TrangThaiSachEngine.Insert(TT);
        }

        public TrangThaiSach getById(string id)
        {
            return _TrangThaiSachEngine.GetById(id);
        }

        public bool SuaTrangThai(TrangThaiSach TT)
        {
            return _TrangThaiSachEngine.Update(TT);
        }

        public bool XoaTrangThai(string id)
        {
            return _TrangThaiSachEngine.Remove(id);
        }
    }
}
