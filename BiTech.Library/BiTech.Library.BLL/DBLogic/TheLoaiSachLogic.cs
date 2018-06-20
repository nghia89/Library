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
    public class TheLoaiSachLogic
    {
        TheLoaiSachEngine _theloaiSachEngine;
        public TheLoaiSachLogic(string connectionString, string databaseName)
        {
            Database database = new Database(connectionString, databaseName);
            _theloaiSachEngine = new TheLoaiSachEngine(database, "TheLoaiSach");         
        }

        public List<TheLoaiSach> GetAllTheLoaiSach()
        {
            return _theloaiSachEngine.GetAllTheLoaiSach();
        }

        public string ThemTheLoaiSach(TheLoaiSach TL)
        {
            return _theloaiSachEngine.Insert(TL);
        }

        public TheLoaiSach getById(string id)
        {
            return _theloaiSachEngine.GetById(id);
        }

        public bool SuaTheLoaiSach(TheLoaiSach TL)
        {
            return _theloaiSachEngine.Update(TL);
        }

        public bool XoaTheLoaiSach(string id)
        {
            return _theloaiSachEngine.Remove(id);
        }
    }
}
