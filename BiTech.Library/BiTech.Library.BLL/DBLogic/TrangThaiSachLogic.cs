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
            _TrangThaiSachEngine = new TrangThaiSachEngine(new Database(connectionString), dbName, TableName);
        }
        /// <summary>
        /// Get all TrangThaiSach object 
        /// </summary>
        /// <returns></returns>
        public List<TrangThaiSach> GetAll()
        {
            return _TrangThaiSachEngine.GetAll();
        }
        public List<TrangThaiSach> GetAllTT(string id)
        {
            return _TrangThaiSachEngine.GetAllTT(id);
        }
        public List<TrangThaiSach> GetAllTT_True()
        {
            return _TrangThaiSachEngine.GetAllTT_True();
        }
        public List<TrangThaiSach> GetAllTT_False()
        {
            return _TrangThaiSachEngine.GetAllTT_False();
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
        #region Tai
        public TrangThaiSach GetBySTT(string idTinhtrang)
        {
            return _TrangThaiSachEngine.GetBySTT(idTinhtrang);
        }
        #endregion


    }
}
