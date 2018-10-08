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
    public class SachCaBietLogic : BaseLogic
    {
        SachCaBietEngine _SachCaBietEngine;
        public SachCaBietLogic(string connectionString, string databaseName)
        {
            Database database = new Database(connectionString);
            _SachCaBietEngine = new SachCaBietEngine(database, databaseName, "SachCaBiet");
        }

        public List<SachCaBiet> GetAll()
        {
            return _SachCaBietEngine.GetAllSachCaBiet();
        }

        public string Add(SachCaBiet TL)
        {
            return _SachCaBietEngine.Insert(TL);
        }
        public SachCaBiet getById(string Id)
        {
            return _SachCaBietEngine.GetById(Id);
        }
        
        public bool Update(SachCaBiet id)
        {
            return _SachCaBietEngine.Update(id);
        }

        public bool Delete(string Id)
        {
            return _SachCaBietEngine.Remove(Id);
        }

        #region Phong

        public SachCaBiet GetAllByMaKSCBorMaCaBienCu(string masach)
        {
            return _SachCaBietEngine.GetAllByMaKSCBorMaCaBienCu(masach);
        }

        #endregion
    }
}
