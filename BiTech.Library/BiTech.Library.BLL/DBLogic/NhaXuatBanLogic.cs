using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiTech.Library.DAL;
using BiTech.Library.DAL.Engines;
using BiTech.Library.DTO;


namespace BiTech.Library.BLL.DBLogic
{
    public class NhaXuatBanLogic
    {
        private string TableName = "NhaXuatBan";
        
        public Database _Database { get; set; }

        public NhaXuatBanEngine _NhaXuatBanEngine { get; set; }

        public NhaXuatBanLogic(string connectionString, string dbName)
        {
            _Database = new Database(connectionString, dbName);
            _NhaXuatBanEngine = new NhaXuatBanEngine(new Database(connectionString, dbName), TableName);
        }

        public List<NhaXuatBan> getAllNhaXuatBan()
        {
            return _NhaXuatBanEngine.GetAllNhaXuatBan();
        }

        public NhaXuatBan getById(string id)
        {
            return _NhaXuatBanEngine.GetById(id);
        }

        public string ThemNXB(NhaXuatBan NXB)
        {
            return _NhaXuatBanEngine.Insert(NXB);
        }

        public bool SuaNXB(NhaXuatBan NXB)
        {
            return _NhaXuatBanEngine.Update(NXB);
        }
        public bool XoaNXB(string id)
        {
            return _NhaXuatBanEngine.Remove(id);
        }
    }
}
