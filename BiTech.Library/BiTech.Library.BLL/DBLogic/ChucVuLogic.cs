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
    public class ChucVuLogic
    {
        ChucVuEngine _chucVuEngine;
        public ChucVuLogic(string connectionString, string databaseName)
        {
            Database database = new Database(connectionString);
            _chucVuEngine = new ChucVuEngine(database, databaseName, "ChucVu");
        }

        public List<ChucVu> GetAllChucVu()
        {
            return _chucVuEngine.GetAllChucvu();
        }

        public string ThemChucVu(ChucVu TL)
        {
            return _chucVuEngine.Insert(TL);
        }

        public ChucVu getById(string id)
        {
            return _chucVuEngine.GetById(id);
        }

        public bool SuaChucVu(ChucVu TL)
        {
            return _chucVuEngine.Update(TL);
        }

        public bool XoaChucVu(string id)
        {
            return _chucVuEngine.Remove(id);
        }
    }
}
