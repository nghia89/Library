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
    public class ThanhVienLogic : BaseLogic
    {
        private string TableName = "ThanhVien";
        public ThanhVienEngine _ThanhVienEngine { get; set; }

        public ThanhVienLogic(string connectionString, string dbName)
        {
            _ThanhVienEngine = new ThanhVienEngine(new Database(connectionString ), dbName, TableName);
        }
        /// <summary>
        /// Get all ThanhVien object (Active - DeActive)
        /// </summary>
        /// <returns></returns>
        public List<ThanhVien> GetAll()
        {
            return _ThanhVienEngine.GetAll();
        }
        /// <summary>
        /// Get all ThanhVien object Active
        /// </summary>
        /// <returns></returns>
        public List<ThanhVien> GetAllActive()
        {
            return _ThanhVienEngine.GetAllActive();
        }
        /// <summary>
        /// Get ThanhVien object DeActive
        /// </summary>
        /// <returns></returns>
        public ThanhVien GetByMaSoThanhVienDeActive(string idUser)
        {
            return _ThanhVienEngine.GetByMaSoThanhVienDeActive(idUser);
        }
        public ThanhVien GetById(string id)
        {
            return _ThanhVienEngine.GetById(id);
        }
        /// <summary>
        /// get by idUser
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ThanhVien GetByMaSoThanhVien(string idUser)
        {
            return _ThanhVienEngine.GetByMaSoThanhVien(idUser);
        }
        /// <summary>
        /// Insert a ThanhVien object
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string Insert(ThanhVien model)
        {
            return _ThanhVienEngine.Insert(model);
        }
        /// <summary>
        /// update - delete( update status)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Update(ThanhVien model)
        {
            return _ThanhVienEngine.Update(model);
        }
        #region Tai
        public List<ThanhVien> GetByName(string ten)
        {
            return _ThanhVienEngine.GetByName(ten);
        }
        public ThanhVien GetByUserName(string userName)
        {
            return _ThanhVienEngine.GetByUserName(userName);
        }
        public List<ThanhVien> GetAllHS()
        {
            return _ThanhVienEngine.GetAllHS();
        }
        public List<ThanhVien> GetAllGV()
        {
            return _ThanhVienEngine.GetAllGV();
        }
        #endregion

    }
}
