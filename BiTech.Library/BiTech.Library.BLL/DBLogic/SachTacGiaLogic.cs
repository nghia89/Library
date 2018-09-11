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
    public class SachTacGiaLogic : BaseLogic
    {
        SachTacGiaEngine _sachTacGiaEngine;
        public SachTacGiaLogic(string connectionString, string databaseName)
        {
            Database database = new Database(connectionString);
            _sachTacGiaEngine = new SachTacGiaEngine(database, databaseName, "SachTacGia");
        }


        public string ThemSachTacGia(SachTacGia s)
        {
            return _sachTacGiaEngine.Insert(s);
        }
        public bool DeleteAllTacGiaByidSach(string idSach)
        {
            return _sachTacGiaEngine.DeleteAllTacGiaByidSach(idSach);
        }
        public List<SachTacGia> getListById(string id)
        {
            return _sachTacGiaEngine.GetAllBookIdBySachId_list(id);

        }

        public SachTacGia getById(string id)
        {
            return _sachTacGiaEngine.GetAllBookIdBySachId(id);

        }

    }
}
