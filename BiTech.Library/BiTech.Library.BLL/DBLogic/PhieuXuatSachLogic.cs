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
    public class PhieuXuatSachLogic
    {
        private string TableName = "PhieuXuatSach";
        public PhieuXuatSachEngine _PhieuXuatSachEngine { get; set; }

        public PhieuXuatSachLogic(string connectionString, string dbName)
        {
            _PhieuXuatSachEngine = new PhieuXuatSachEngine(new Database(connectionString), dbName, TableName);
        }
        public string XuatSach(PhieuXuatSach xs)
        {
            return _PhieuXuatSachEngine.Insert(xs);
        }
        public List<PhieuXuatSach> Getall()
        {
            return _PhieuXuatSachEngine.getAll();
        }
        public PhieuXuatSach GetById(string id)
        {
            return _PhieuXuatSachEngine.GetById(id);
        }
    }
}
