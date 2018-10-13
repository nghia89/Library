using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiTech.Library.DTO;
using BiTech.Library.DAL.Engines;
using BiTech.Library.DAL;

namespace BiTech.Library.BLL.DBLogic
{    
    public class TacGiaLogic : BaseLogic
    {
        TacGiaEngine _tacGiaEngine;
        SachTacGiaEngine _sachTacGiaEngine;
        private string TableName = "TacGia";
        public TacGiaLogic(string connectionString, string databaseName)
        {
            Database database = new Database(connectionString);
            _tacGiaEngine = new TacGiaEngine(database, databaseName, TableName);
            _sachTacGiaEngine = new SachTacGiaEngine(database, databaseName, TableName);
        }
        public List<TacGia> GetAllTacGia()
        {
            return _tacGiaEngine.GetAllTacGia();
        }

        public List<TacGia> FindTacGia(string q)
        {
            return _tacGiaEngine.FindTacGia(q);
        }
        public List<TacGia> FindNameTacGia(string q)
        {
            return _tacGiaEngine.GetByFindName(q);
        }
        public TacGia FindNameId(string q)
        {
            return _tacGiaEngine.GetByNameId(q);
        }

        public string Insert(TacGia tacgia)
        {          
            return _tacGiaEngine.Insert(tacgia);
        }

        public TacGia GetById(string id)
        {
          
            return _tacGiaEngine.GetById(id);
        }
        public TacGia GetByIdTG(string id)
        {
            //var IdSachTG = _sachTacGiaEngine.GetAllBookIdBySachId(id);
            return _tacGiaEngine.GetById(id);
        }
        public bool Update(TacGia tacgia)
        {   
            return _tacGiaEngine.Update(tacgia);
        }

        public bool Delete(string id)
        {
            return _tacGiaEngine.Remove(id);
        }

        #region Tai
        public TacGia GetByTenTacGia(string tenTacGia)
        {
            return _tacGiaEngine.GetByTenTacGia(tenTacGia);
        }

        public void UpdateDBVersion()
        {
            _tacGiaEngine.UpdateDBVersion();
        }
        #endregion
    }
}
