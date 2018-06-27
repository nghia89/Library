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
    public class TacGiaLogic
    {
        TacGiaEngine _tacGiaEngine;
        private string TableName = "TacGia";
        public TacGiaLogic(string connectionString, string databaseName)
        {
            Database database = new Database(connectionString, databaseName);
            _tacGiaEngine = new TacGiaEngine(database, TableName);
        }
        public List<TacGia> GetAllTacGia()
        {
            return _tacGiaEngine.GetAllTacGia();
        }
        public string Insert(TacGia tacgia)
        {
            return _tacGiaEngine.Insert(tacgia);
        }

        public TacGia GetById(string id)
        {
            return _tacGiaEngine.GetById(id);
        }

        public bool Update(TacGia tacgia)
        {
            return _tacGiaEngine.Update(tacgia);
        }

        public bool Delete(string id)
        {
            return _tacGiaEngine.Remove(id);
        }
    }
}
