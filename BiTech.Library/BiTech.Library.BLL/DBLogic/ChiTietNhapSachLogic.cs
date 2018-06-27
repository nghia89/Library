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
    public class ChiTietNhapSachLogic
    {
        ChiTietNhapSachEngine _ChiTietNhapSachEngine;
        private string TableName = "ChiTietNhapSach";
        public ChiTietNhapSachLogic(string connectionString, string databaseName)
        {
            Database database = new Database(connectionString, databaseName);
            _ChiTietNhapSachEngine = new ChiTietNhapSachEngine(database, TableName);
        }
        public List<ChiTietNhapSach> GetAllTacGia()
        {
            return _ChiTietNhapSachEngine.GetAllChiTietNhapSach();
        }
        public string Insert(ChiTietNhapSach ctns)
        {
            return _ChiTietNhapSachEngine.Insert(ctns);
        }

        public  ChiTietNhapSach GetById(string id)
        {
            return _ChiTietNhapSachEngine.GetById(id);
        }

        public bool Update(ChiTietNhapSach ctns)
        {
            return _ChiTietNhapSachEngine.Update(ctns);
        }

        public bool Delete(string id)
        {
            return _ChiTietNhapSachEngine.Remove(id);
        }
    }
}
