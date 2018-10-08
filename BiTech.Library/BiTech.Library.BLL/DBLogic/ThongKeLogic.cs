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

        private string TableName4 = "SachCaBiet";
        SachCaBietEngine _sachCaBietEngine;

        public ThongKeLogic(string connectionString, string databaseName)
        {
            Database database = new Database(connectionString);          
            _sachEngine = new SachEngine(database, databaseName, TableName1);
            _thanhVienEngine = new ThanhVienEngine(database, databaseName, TableName2);       
            _thongTinMuonSachEngine = new ThongTinMuonSachEngine(database, databaseName, TableName3);
            _sachCaBietEngine = new SachCaBietEngine(database, databaseName, TableName4);


            SachCaBiet xx = new SachCaBiet
            {
                IdSach= "5bb477638e034f1db04be80f"               
            };
            //_sachCaBietEngine.Insert(xx);
            DateTime ngayMuon = new DateTime(2018, 10, 5);
            ThongTinMuonSach tt = new ThongTinMuonSach
            {
                idSach = "5bb6db018e03521ee8ad0888",
                idUser = "6",
                NgayGioMuon=new DateTime(2018, 10, 5),
                NgayPhaiTra=new DateTime(2018,10,20)

            };
           // _thongTinMuonSachEngine.Insert(tt);

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

        public List<ThongTinMuonSach> GetTTMSByNgayTra(DateTime ngayTra)
        {
            return _thongTinMuonSachEngine.GetTTMSByNgayTra(ngayTra);
        }

        // SÁCH
        public Sach GetSachById(string idSach)
        {
            return _sachEngine.GetById(idSach);
        }

        public SachCaBiet GetSachCaBietById(string idSachCaBiet)
        {
            return _sachCaBietEngine.GetById(idSachCaBiet);
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