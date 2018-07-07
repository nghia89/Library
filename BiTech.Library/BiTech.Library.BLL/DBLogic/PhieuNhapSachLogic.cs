using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiTech.Library.DAL;
using BiTech.Library.DAL.Engines;
using BiTech.Library.DTO;

namespace BiTech.Library.BLL.DBLogic
{
    public class PhieuNhapSachLogic
    {
        private string TableName = "PhieuNhapSach";
        public PhieuNhapSachEngine _NhapSachEngine { get; set; }

        public PhieuNhapSachLogic(string connectionString,string dbName)
        {
            _NhapSachEngine = new PhieuNhapSachEngine(new Database(connectionString), dbName, TableName);
        }

        public string NhapSach(PhieuNhapSach ns)
        {
            return _NhapSachEngine.Insert(ns);
        }
        public List<PhieuNhapSach> Getall()
        {
            return _NhapSachEngine.getAll();
        }
        public PhieuNhapSach GetById(string id)
        {
            return _NhapSachEngine.GetById(id);
        }
    }
}
