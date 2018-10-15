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
    public class LanguageLogic : BaseLogic
    {
        LanguageEngine _LanguageEngine;
        public LanguageLogic(string connectionString, string databaseName)
        {
            Database database = new Database(connectionString);
            _LanguageEngine = new LanguageEngine(database, databaseName, DBTableNames.Languages_Table);
        }

        public List<Language> GetAll()
        {
            return _LanguageEngine.GetAll();
        }

        public Language GetById(string id)
        {
            return _LanguageEngine.GetById(id);
        }

        public string InsertNew(Language dto)
        {
          
            return _LanguageEngine.Insert(dto);
        }

        public bool Update(Language dto)
        {
            return _LanguageEngine.Update(dto);
        }

        public bool Remove(string id)
        {
            return _LanguageEngine.Remove(id);
        }
        #region Tai
        public Language GetByTenNgonNgu(string tenNgonNgu)
        {
           return _LanguageEngine.GetByTenNgonNgu(tenNgonNgu);
        }

        public void UpdateDBVersion()
        {
            _LanguageEngine.UpdateDBVersion();
        }
        #endregion

        public List<Language> FindNamelanguge(string q)
        {
            return _LanguageEngine.GetByFindName(q);
        }
        public Language FindNameId(string q)
        {
            return _LanguageEngine.GetByNameId(q);
        }
    }
}
