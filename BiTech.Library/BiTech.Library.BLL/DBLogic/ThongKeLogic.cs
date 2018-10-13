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
    public class ThongKeLogic : BaseLogic
    {              
        private string TableName1 = "Sach";
        SachEngine _sachEngine;

        private string TableName2 = "ThanhVien";
        ThanhVienEngine _thanhVienEngine;       
       
        private string TableName3 = "ThongTinMuonSach";
        ThongTinMuonSachEngine _thongTinMuonSachEngine;

        public ThongKeLogic(string connectionString, string databaseName)
        {
            Database database = new Database(connectionString);          
            _sachEngine = new SachEngine(database, databaseName, TableName1);
            _thanhVienEngine = new ThanhVienEngine(database, databaseName, TableName2);       
            _thongTinMuonSachEngine = new ThongTinMuonSachEngine(database, databaseName, TableName3);          
        }

        //  THÔNG TIN MƯỢN SÁCH
        public List<ThongTinMuonSach> GetAllTTMS()
        {
            return _thongTinMuonSachEngine.GetAll();
        }

        public List<ThongTinMuonSach> GetTTMSByIdUser(string idUser)
        {
            return _thongTinMuonSachEngine.GetTTMSByIdUser(idUser);
        }       

        // SÁCH
        public Sach GetSachById(string idSach)
        {
            return _sachEngine.GetById(idSach);
        }
        
        // THÀNH VIÊN         
        public ThanhVien GetThanhVienByMSTV(string maSoThanhVien)
        {
            return _thanhVienEngine.GetByMaSoThanhVien(maSoThanhVien);
        }       
    }
}