using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiTech.Library.DTO;
using BiTech.Library.DAL.Engines;
using BiTech.Library.DAL;
namespace BiTech.Library.BLL.DBLogic
{
    public class SachLogic
    {
        SachEngine _sachEngine;
        public SachLogic(string connectionString, string databaseName)
        {
            Database database = new Database(connectionString);
            _sachEngine = new SachEngine(database, databaseName, "Sach");
        }

        #region Vinh

        public Sach GetById(string id)
        {
            return _sachEngine.GetById(id);
        }
		
        /// <summary>
        /// update - delete( update status)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Update(Sach model)
        {
            return _sachEngine.Update(model);
        }
		
        /// <summary>
        /// Get book by idBook
        /// </summary>
        /// <param name="idBook"></param>
        /// <returns></returns>
        //public Sach GetByIdBook(string idBook)
        //{
        //    return _sachEngine.GetByIdBook(idBook);
        //}

        public Sach GetBookById(string idBook)
        {
            return _sachEngine.GetById(idBook);
        }
        #endregion

        #region Thinh
        public List<Sach> getAllSach()
        {
            return _sachEngine.GetAllSach();
        }

        public string ThemSach(Sach s)
        {
            return _sachEngine.Insert(s);
        }

        public bool XoaSach(string id)
        {
            return _sachEngine.Remove(id);
        }

        public Sach GetByMaMaKiemSoat(string maKS)
        {
            return _sachEngine.GetByMaKiemSoat(maKS);
        }
        #endregion
    }
}
