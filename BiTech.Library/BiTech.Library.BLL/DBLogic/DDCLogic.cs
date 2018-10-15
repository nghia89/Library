using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiTech.Library.DTO;
using BiTech.Library.DAL.Engines;
using BiTech.Library.DAL;
using BiTech.Library.Common;

namespace BiTech.Library.BLL.DBLogic
{
    public class DDCLogic
    {
        DDCEngine _DDCEngine;
        public DDCLogic(string connectionString, string databaseName)
        {
            Database database = new Database(connectionString);
            _DDCEngine = new DDCEngine(database, databaseName, "DDC");
        }

        public List<DDC> GetAllDDC()
        {
            return _DDCEngine.GetAllDDC();
        }
        public List<DDC> getDDCByKeySearch(KeySearchViewModel KeySearch)
        {
            return _DDCEngine.getDDCByKeySearch(KeySearch);
        }
        
        public string Add(DDC TL)
        {
            return _DDCEngine.Insert(TL);
        }

        public string ThemDDC(DDC TL)
        {
            return _DDCEngine.Insert(TL);
        }

        public DDC getById(string id)
        {
            return _DDCEngine.GetById(id);
        }

        public bool SuaDDC(DDC TL)
        {
            return _DDCEngine.Update(TL);
        }

        public bool XoaDDC(string id)
        {
            return _DDCEngine.Remove(id);
        }

        public bool Update(DDC id)
        {
            return _DDCEngine.Update(id);
        }

        public bool Delete(string Id)
        {
            return _DDCEngine.Remove(Id);
        }

        public void UpdateDBVersion()
        {
            _DDCEngine.UpdateDBVersion();
        }
    }
}
