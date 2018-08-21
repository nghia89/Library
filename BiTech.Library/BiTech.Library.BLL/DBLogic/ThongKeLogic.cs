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
    public class ThongKeLogic
    {              
        private string TableName3 = "Sach";
        SachEngine _sachEngine;

        private string TableName4 = "ThanhVien";
        ThanhVienEngine _thanhVienEngine;       
       
        private string TableName8 = "ThongTinMuonSach";
        ThongTinMuonSachEngine _thongTinMuonSachEngine;

        public ThongKeLogic(string connectionString, string databaseName)
        {
            Database database = new Database(connectionString);          
            _sachEngine = new SachEngine(database, databaseName, TableName3);
            _thanhVienEngine = new ThanhVienEngine(database, databaseName, TableName4);       
            _thongTinMuonSachEngine = new ThongTinMuonSachEngine(database, databaseName, TableName8); 
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

        public List<ThongTinMuonSach> GetTTMSByNgayMuon(DateTime ngayMuon)
        {
            return _thongTinMuonSachEngine.GetTTMSByNgayMuon(ngayMuon);
        }        

        // SÁCH
        public Sach GetSachById(string idSach)
        {
            return _sachEngine.GetById(idSach);
        }
        // THÀNH VIÊN   
        public ThanhVien GetThanhVienById(string idThanhVien)
        {
            return _thanhVienEngine.GetById(idThanhVien);
        }
        public ThanhVien GetThanhVienByMSTV(string maSoThanhVien)
        {
            return _thanhVienEngine.GetByMaSoThanhVien(maSoThanhVien);
        }       
    }
}