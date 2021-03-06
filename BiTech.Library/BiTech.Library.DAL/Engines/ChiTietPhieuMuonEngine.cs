﻿using BiTech.Library.DAL.Respository;
using BiTech.Library.DTO;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiTech.Library.DAL.Engines
{
    public class ChiTietPhieuMuonEngine : EntityRepository<ChiTietPhieuMuon>
    {
        public ChiTietPhieuMuonEngine(IDatabase database, string databaseName, string tableName) : base(database, databaseName, tableName)
        {
            _Database = (IMongoDatabase)database.GetConnection(databaseName);
            _DatabaseCollection = _Database.GetCollection<ChiTietPhieuMuon>(tableName);
        }
        /// <summary>
        /// Get all ChiTietPhieuMuon object 
        /// </summary>
        /// <returns></returns>
        public List<ChiTietPhieuMuon> GetAll()
        {
            return _DatabaseCollection.Find(_ => _.Id != null && _.Id != "").ToList();            
        }
		
        #region Tai
        public List<ChiTietPhieuMuon> GetCTPMbyId(string idPhieuMuon)
        {
            return _DatabaseCollection.Find(x => x.IdPhieuMuon == idPhieuMuon).ToList();
        }
        #endregion    
		
        /// <summary>
        /// Get list chitietphieumuon by IdPM
        /// </summary>
        /// <param name="idPM"></param>
        /// <returns></returns>
        public List<ChiTietPhieuMuon> GetByIdPhieuMuon(string idPM)
        {
            return _DatabaseCollection.Find(_ => _.IdPhieuMuon == idPM).ToList();
        }
        /// <summary>
        /// Get list chitietphieumuon by idBook
        /// </summary>
        /// <param name="idPM"></param>
        /// <returns></returns>
        public List<ChiTietPhieuMuon> GetByIdBook(string idBook)
        {
            return _DatabaseCollection.Find(_ => _.IdSach == idBook).ToList();
        }
        /// <summary>
        /// Get list by idBook and idPM
        /// </summary>
        /// <param name="idBook"></param>
        /// <param name="idPM"></param>
        /// <returns></returns>
        public ChiTietPhieuMuon GetByIdBook_IdPM(string idBook, string idPM)
        {
            return _DatabaseCollection.Find(_ => _.IdPhieuMuon == idPM && _.IdSach == idBook).FirstOrDefault();
        }
    }
}
