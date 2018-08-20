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
        private string TableName1 = "PhieuMuon";
        ThongKeEngine _thongKeEngine;
        PhieuMuonEngine _phieuMuonEngine;


        private string TableName2 = "ChiTietPhieuMuon";
        ChiTietPhieuMuonEngine _chiTietPhieuMuon;

        private string TableName3 = "Sach";
        SachEngine _sachEngine;

        private string TableName4 = "ThanhVien";
        ThanhVienEngine _thanhVienEngine;

        private string TableName5 = "TrangThaiSach";
        TrangThaiSachEngine _trangThaiSachEngine;

        private string TableName6 = "PhieuTra";
        PhieuTraEngine _phieuTraEngine;

        private string TableName7 = "ChiTietPhieuTra";
        ChiTietPhieuTraEngine _chiTietPhieuTraEngine;

        private string TableName8 = "ThongTinMuonSach";
        ThongTinMuonSachEngine _thongTinMuonSachEngine;

        public ThongKeLogic(string connectionString, string databaseName)
        {
            Database database = new Database(connectionString);

            _thongKeEngine = new ThongKeEngine(database, databaseName, TableName1);
            _phieuMuonEngine = new PhieuMuonEngine(database, databaseName, TableName1);
            _chiTietPhieuMuon = new ChiTietPhieuMuonEngine(database, databaseName, TableName2);
            _sachEngine = new SachEngine(database, databaseName, TableName3);
            _thanhVienEngine = new ThanhVienEngine(database, databaseName, TableName4);
            _trangThaiSachEngine = new TrangThaiSachEngine(database, databaseName, TableName5);
            _phieuTraEngine = new PhieuTraEngine(database, databaseName, TableName6);
            _chiTietPhieuTraEngine = new ChiTietPhieuTraEngine(database, databaseName, TableName7);
            _thongTinMuonSachEngine = new ThongTinMuonSachEngine(database, databaseName, TableName8);


            var ngayMuon = DateTime.ParseExact("26-02-2017", "dd-MM-yyyy", null);
            var ngayPhaiTra = DateTime.ParseExact("26-08-2018", "dd-MM-yyyy", null);
            var ngayTra = DateTime.ParseExact("25-04-2017", "dd-MM-yyyy", null);

            //ThongTinMuonSach tt = new ThongTinMuonSach()
            //{
            //    idSach = "5b69574fa9b6e238848a521c",
            //    idUser = "5b6bb015a9b6db05cc289d06",
            //    NgayGioMuon = "26-02-2017",                
            //    NgayPhaiTra = "26-04-2017",
            //};
            //_thongTinMuonSachEngine.Insert(tt);
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

        public List<ThongTinMuonSach> GetTTMSByNgayMuon(string ngayMuon)
        {
            return _thongTinMuonSachEngine.GetTTMSByNgayMuon(ngayMuon);
        }


        // PHIẾU MƯỢN
        public List<PhieuMuon> GetAllPhieuMuon()
        {
            return _thongKeEngine.ListPhieuMuon();
        }
        public List<PhieuMuon> GetPMByIdThanhVien(string maSoThanhVien)
        {
            return _phieuMuonEngine.GetPMByIdUser(maSoThanhVien);
        }
        public List<PhieuMuon> GetPMByNgayMuon(DateTime ngayMuon)
        {
            return _thongKeEngine.GetPMByNgayMuon(ngayMuon);
        }

        // CHI TIẾT PHIẾU MƯỢN
        public List<ChiTietPhieuMuon> GetCTPMById(string idPhieuMuon)
        {
            return _chiTietPhieuMuon.GetCTPMbyId(idPhieuMuon);
        }

        // SÁCH
        public Sach GetSachById(string idSach)
        {
            return _sachEngine.GetById(idSach);
        }
        //public Sach GetSachByMaKiemSoat(string maKS)
        //{
        //    return _sachEngine.GetByMaKiemSoat(maKS);
        //}

        // THÀNH VIÊN 
        //public ThanhVien GetThanhVienByMSTV(string MaSoThanhVien)
        //{
        //    return _thanhVienEngine.GetByMaSoThanhVien(MaSoThanhVien);
        //}
        public ThanhVien GetThanhVienById(string idThanhVien)
        {
            return _thanhVienEngine.GetById(idThanhVien);
        }

        // DANH MỤC TRẢ SÁCH
        public List<PhieuTra> GetPTByIdPM(string idPhieuMuon)
        {
            return _phieuTraEngine.GetPTByIdPM(idPhieuMuon);
        }
        public List<ChiTietPhieuTra> GetCTPTByIdPT(string idPhieuTra)
        {
            return _chiTietPhieuTraEngine.GetCTPTByIdPT(idPhieuTra);
        }


    }
}