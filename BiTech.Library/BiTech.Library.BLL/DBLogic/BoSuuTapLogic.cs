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
  public  class BoSuuTapLogic: BaseLogic
    {
        BoSuuTapEngines _BoSuuTapEngines;
        public BoSuuTapLogic(string connectionString, string databaseName)
        {
            Database database = new Database(connectionString);
            _BoSuuTapEngines = new BoSuuTapEngines(database, databaseName, "BoSuuTap");
        }

        public List<BoSuuTap> GetAll()
        {
            return _BoSuuTapEngines.GetAll();
        }

        public BoSuuTap FindById(string Id)
        {
            return _BoSuuTapEngines.GetById(Id);
        }

        public string Insert(BoSuuTap entity)
        {
            return _BoSuuTapEngines.Insert(entity);
        }

        public bool Update(BoSuuTap Id)
        {
            return _BoSuuTapEngines.Update(Id);
        }

        public virtual bool Delete(string Id)
        {
            return _BoSuuTapEngines.Remove(Id);
        }
          public BoSuuTap GetName(string name)
        {
            var Getname = _BoSuuTapEngines.Getname(name);
            return _BoSuuTapEngines.GetById(Getname.Id);
        }
    }
}
