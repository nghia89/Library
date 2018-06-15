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
    public class PhieuMuonLogic
    {
        private string TableName = "ThanhVien";
        public PhieuMuonEngine _PhieuMuonEngine { get; set; }

        public PhieuMuonLogic(string connectionString, string dbName)
        {
            _PhieuMuonEngine = new PhieuMuonEngine(new Database(connectionString, dbName), TableName);
        }
        /// <summary>
        /// Get all PhieuMuon object 
        /// </summary>
        /// <returns></returns>
        public List<PhieuMuon> GetAll()
        {
            return _PhieuMuonEngine.GetAll();
        }
    }
}
