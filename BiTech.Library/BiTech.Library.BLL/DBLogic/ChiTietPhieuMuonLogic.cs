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
        public ChiTietPhieuMuon GetById(string id)
        {
            return _ChiTietPhieuMuonEngine.GetById(id);
        }
        /// <summary>
        /// Insert a PhieuMuon object
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string Insert(ChiTietPhieuMuon model)
        {
            return _ChiTietPhieuMuonEngine.Insert(model);
        }
        /// <summary>
        /// update - delete( update status)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Update(ChiTietPhieuMuon model)
        {
            return _ChiTietPhieuMuonEngine.Update(model);
        }
    }
}
