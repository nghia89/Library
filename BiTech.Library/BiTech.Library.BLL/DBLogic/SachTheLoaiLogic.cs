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
    public class SachTheLoaiLogic : BaseLogic
    {
        SachTheLoaiEngine _sachTacGiaEngine;
        public SachTheLoaiLogic(string connectionString, string databaseName)
        {
            Database database = new Database(connectionString);
            _sachTacGiaEngine = new SachTheLoaiEngine(database, databaseName, "SachTheLoai");
        }

        public string ThemSachTheLoai(SachTheLoai s)
        {
            return _sachTacGiaEngine.Insert(s);
        }

        public bool DeleteAllTheLoaiByidSach(string idSach)
        {
            return _sachTacGiaEngine.DeleteAllTheLoaiByidSach(idSach);
        }

        public List<SachTheLoai> getListById(string id)
        {
            return _sachTacGiaEngine.GetAllBookIdBySachId_list(id);

        }

        public SachTheLoai getById(string id)
        {
            return _sachTacGiaEngine.GetAllBookIdBySachId(id);

        }
    }
}
