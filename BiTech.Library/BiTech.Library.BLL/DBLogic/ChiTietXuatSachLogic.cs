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
    public class ChiTietXuatSachLogic : BaseLogic
    {
        ChiTietXuatSachEngine _ChiTietXuatSachEngine;
        private string TableName = "ChiTietXuatSach";
        public ChiTietXuatSachLogic(string connectionString, string databaseName)
        {
            Database database = new Database(connectionString);
            _ChiTietXuatSachEngine = new ChiTietXuatSachEngine(database, databaseName, TableName);
        }
        public List<ChiTietXuatSach> GetAll()
        {
            return _ChiTietXuatSachEngine.GetAllChiTietXuatSach();
        }
        public string Insert(ChiTietXuatSach ctxs)
        {
            return _ChiTietXuatSachEngine.Insert(ctxs);
        }

        public List<ChiTietXuatSach> GetAllChiTietById(string id)
        {
            return _ChiTietXuatSachEngine.GetAllChiTietById(id);
        }
        public List<ChiTietXuatSach> GetAllChiTietByIdSach(string idSach)
        {
            return _ChiTietXuatSachEngine.GetAllChiTietById(idSach);
        }
        public ChiTietXuatSach GetById(string id)
        {
            return _ChiTietXuatSachEngine.GetById(id);
        }

        public bool Update(ChiTietXuatSach ctxs)
        {
            return _ChiTietXuatSachEngine.Update(ctxs);
        }

        public bool Delete(string id)
        {
            return _ChiTietXuatSachEngine.Remove(id);
        }

        public void UpdateDBVersion()
        {
            _ChiTietXuatSachEngine.UpdateDBVersion();
        }
    }
}
