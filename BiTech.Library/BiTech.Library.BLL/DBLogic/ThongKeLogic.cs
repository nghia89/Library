﻿using System;
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

        private string TableName2 = "ChiTietPhieuMuon";
        ChiTietPhieuMuonEngine _chiTietPhieuMuon;

        private string TableName3 = "Sach";
        SachEngine _sachEngine;

        private string TableName4 = "ThanhVien";
        ThanhVienEngine _thanhVienEngine;


        public ThongKeLogic(string connectionString, string databaseName)
        {
            Database database = new Database(connectionString, databaseName);

            _thongKeEngine = new ThongKeEngine(database, TableName1);
            _chiTietPhieuMuon = new ChiTietPhieuMuonEngine(database, TableName2);
            _sachEngine = new SachEngine(database, TableName3);
            _thanhVienEngine = new ThanhVienEngine(database, TableName4);

            var ngayMuon = DateTime.ParseExact("26-02-2017", "dd-MM-yyyy", null);
            var ngayPhaiTra = DateTime.ParseExact("26-08-2018", "dd-MM-yyyy",null);
            var ngayTra = DateTime.ParseExact("25-04-2017", "dd-MM-yyyy", null);

            PhieuMuon p = new PhieuMuon
            {
                NgayMuon= ngayMuon,
                NgayPhaiTra = ngayPhaiTra
                
            };
           // _thongKeEngine.Insert(p);

        }

        public List<PhieuMuon> ListPhieuMuon()
        {
            return _thongKeEngine.ListPhieuMuon();
        }

        public Sach GetSachById(string idSach)
        {
            return _sachEngine.GetById(idSach);
        }

        public List<ChiTietPhieuMuon> GetCTPMById(string idPhieuMuon)
        {
            return _chiTietPhieuMuon.GetCTPMbyId(idPhieuMuon);
        }

        public ThanhVien GetThanhVienById(string idThanhVien)
        {                    
            return _thanhVienEngine.GetById(idThanhVien);
        }

             
    }
}