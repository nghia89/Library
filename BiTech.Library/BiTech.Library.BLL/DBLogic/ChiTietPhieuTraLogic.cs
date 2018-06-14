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
    public class ChiTietPhieuTraLogic
    {
        private string TableName = "ChiTietPhieuTra";
        public ChiTietPhieuTraEngine _ChiTietPhieuTraEngine { get; set; }

        public ChiTietPhieuTraLogic(string connectionString, string dbName)
        {
            _ChiTietPhieuTraEngine = new ChiTietPhieuTraEngine(new Database(connectionString, dbName), TableName);
        }
        /// <summary>
        /// Get all ChiTietPhieuTraEngine object
        /// </summary>
        /// <returns></returns>
        public List<ChiTietPhieuTra> GetAll()
        {
            return _ChiTietPhieuTraEngine.GetAll();
        }
    }
}
