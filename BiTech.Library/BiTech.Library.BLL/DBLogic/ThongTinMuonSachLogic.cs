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
    public class ThongTinMuonSachLogic : BaseLogic
    {
        private string TableName = "ThongTinMuonSach";
        public ThongTinMuonSachEngine _ThongTinMuonSachEngine { get; set; }

        public ThongTinMuonSachLogic(string connectionString, string dbName)
        {
            _ThongTinMuonSachEngine = new ThongTinMuonSachEngine(new Database(connectionString), dbName, TableName);
        }

        /// <summary>
        /// Get all TrangThaiSach object 
        /// </summary>
        /// <returns></returns>
        public List<ThongTinMuonSach> GetAll()
        {
            return _ThongTinMuonSachEngine.GetAll();
        }
        public List<ThongTinMuonSach> GetAllTT(string id)
        {
            return _ThongTinMuonSachEngine.GetAllTT(id);
        }

        public List<ThongTinMuonSach> GetAllbyIdUser(string IdUser)
        {
            return _ThongTinMuonSachEngine.GetByidUser(IdUser);
        }

        public List<ThongTinMuonSach> GetAllbyIdSach(string IdSach)
        {
            return _ThongTinMuonSachEngine.GetByidSach(IdSach);
        }

        public List<ThongTinMuonSach> GetAllIdUser_ChuaTra(string IdUser)
        {
            return _ThongTinMuonSachEngine.GetByidUser_ChuaTra(IdUser);
        }

        public int Count_ChuaTra_byIdSach(string idSach)
        {
            return _ThongTinMuonSachEngine.GetBy_ChuaTra_byidSach(idSach).Count();
        }

        /// <summary>
        /// Insert a TinhTrangSach object
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string Insert(ThongTinMuonSach model)
        {
            return _ThongTinMuonSachEngine.Insert(model);
        }

        public bool Update(ThongTinMuonSach model)
        {
            return _ThongTinMuonSachEngine.Update(model);
        }

        public string ThemTrangThai(ThongTinMuonSach TT)
        {
            return _ThongTinMuonSachEngine.Insert(TT);
        }

        public ThongTinMuonSach getById(string id)
        {
            return _ThongTinMuonSachEngine.GetById(id);
        }

        public ThongTinMuonSach getByThongTinMuonSach(ThongTinMuonSach TT)
        {
            List<ThongTinMuonSach> team = _ThongTinMuonSachEngine.GetByThongTinMuonSach(TT);
            if(team.Count > 0)
                return team[0];
            return null;
        }

        public List<ThongTinMuonSach> getByThongTinMuonSachList(ThongTinMuonSach TT)
        {
            return _ThongTinMuonSachEngine.GetByThongTinMuonSach(TT);
        }

        public bool SuaTrangThai(ThongTinMuonSach TT)
        {
            return _ThongTinMuonSachEngine.Update(TT);
        }

        public bool XoaTrangThai(string id)
        {
            return _ThongTinMuonSachEngine.Remove(id);
        }
    }
}
