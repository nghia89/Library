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
    public class SachTacGiaLogic
    {
        SachTacGiaEngine _sachTacGiaEngine;
        public SachTacGiaLogic(string connectionString, string databaseName)
        {
            Database database = new Database(connectionString);
            _sachTacGiaEngine = new SachTacGiaEngine(database, databaseName, "SachTacGia");
        }


        public string ThemSachTacGia(SachTacGia s)
        {
            return _sachTacGiaEngine.Insert(s);
        }
        public SachTacGia getById(string id)
        {
            return _sachTacGiaEngine.GetAllBookIdBySachId(id);
            
        }

    }
}
