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
        private string TableName = "PhieuMuon";
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
        public PhieuMuon GetById(string id)
        {
            return _PhieuMuonEngine.GetById(id);
        }
        /// <summary>
        /// get by UserID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<PhieuMuon> GetByIdUser(string idUser)
        {
            return _PhieuMuonEngine.GetByIdUser(idUser);
        }
        /// <summary>
        /// Insert a PhieuMuon object
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string Insert(PhieuMuon model)
        {
            return _PhieuMuonEngine.Insert(model);
        }
        /// <summary>
        /// update - delete( update status)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Update(PhieuMuon model)
        {
            return _PhieuMuonEngine.Update(model);
        }
        /// <summary>
        /// Remove
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Remove(string id)
        {
            return _PhieuMuonEngine.Remove(id);
        }
    }
}
