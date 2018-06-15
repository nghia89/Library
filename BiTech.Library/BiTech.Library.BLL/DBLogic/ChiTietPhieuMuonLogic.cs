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
    public class ChiTietPhieuMuonLogic
    {
        private string TableName = "ChiTietPhieuMuon";
        public ChiTietPhieuMuonEngine _ChiTietPhieuMuonEngine { get; set; }

        public ChiTietPhieuMuonLogic(string connectionString, string dbName)
        {
            _ChiTietPhieuMuonEngine = new ChiTietPhieuMuonEngine(new Database(connectionString, dbName), TableName);
        }
        /// <summary>
        /// Get all ChiTietPhieuMuon object
        /// </summary>
        /// <returns></returns>
        public List<ChiTietPhieuMuon> GetAll()
        {
            return _ChiTietPhieuMuonEngine.GetAll();
        }
    }
}
