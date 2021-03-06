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
    public class ChiTietPhieuTraEngine : EntityRepository<ChiTietPhieuTra>
    {
        public ChiTietPhieuTraEngine(IDatabase database, string databaseName, string tableName) : base(database, databaseName, tableName)
        {
            _Database = (IMongoDatabase)database.GetConnection(databaseName);
            _DatabaseCollection = _Database.GetCollection<ChiTietPhieuTra>(tableName);
        }
        /// <summary>
        /// Get all ChiTietPhieuTra object 
        /// </summary>
        /// <returns></returns>
        public List<ChiTietPhieuTra> GetAll()
        {
            return _DatabaseCollection.Find(_ => _.Id != null && _.Id != "").ToList();
        }


        #region Tai
		
        public List<ChiTietPhieuTra> GetCTPTByIdPT(string idPhieuTra)
        {
            return _DatabaseCollection.Find(_ => _.IdPhieuTra == idPhieuTra).ToList();
        }
		
        #endregion

        /// <summary>
        /// Get by ChiTietPhieuTra by IdBook
        /// </summary>
        /// <returns></returns>
        public List<ChiTietPhieuTra> GetByIdBook(string IdBook)
        {
            return _DatabaseCollection.Find(_ => _.IdSach == IdBook ).ToList();
        }

        /// <summary>
        /// Get by ChiTietPhieuTra by IdPhieuTra
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<ChiTietPhieuTra> GetByIdPhieuTra(string id)
        {
            return _DatabaseCollection.Find(_ => _.IdPhieuTra == id).ToList();
        }
        public ChiTietPhieuTra GetByIdBook_IdPT(string idBook, string idPT)
        {
            return _DatabaseCollection.Find(_ => _.IdPhieuTra == idPT && _.IdSach == idBook).FirstOrDefault();
        }
    }
}
