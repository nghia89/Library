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
    public class PhieuTraLogic
    {
        private string TableName = "PhieuTra";
        public PhieuTraEngine _PhieuTraEngine { get; set; }

        public PhieuTraLogic(string connectionString,string dbName)
        {
            _PhieuTraEngine = new PhieuTraEngine(new Database(connectionString),  dbName, TableName);
        }

        /// <summary>
        /// Get all PhieuTra object 
        /// </summary>
        /// <returns></returns>
        public List<PhieuTra> GetAll()
        {
            return _PhieuTraEngine.GetAll();
        }

        public PhieuTra GetById(string id)
        {
            return _PhieuTraEngine.GetById(id);
        }
        
        /// <summary>
        /// Insert a PhieuTra object
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string Insert(PhieuTra model)
        {
            return _PhieuTraEngine.Insert(model);
        }

        /// <summary>
        /// update - delete( update status)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Update(PhieuTra model)
        {
            return _PhieuTraEngine.Update(model);
        }

        public bool Remove (string id)
        {
            return _PhieuTraEngine.Remove(id);
        }
    }
}
